using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Actions;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Crypto;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Serializers;
using PhlegmaticOne.DataStorage.Infrastructure.Dispatcher;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;

namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource
{
    internal sealed class PlayerPrefsDataSource<T> : IDataSource<T> where T : class, IModel
    {
        private readonly IEntitySerializer _entitySerializer;
        private readonly IMainThreadDispatcher _mainThreadDispatcher;
        private readonly IStringCryptoProvider _stringCryptoProvider;

        public PlayerPrefsDataSource(IEntitySerializer entitySerializer,
            IStringCryptoProvider stringCryptoProvider,
            IMainThreadDispatcher mainThreadDispatcher)
        {
            _stringCryptoProvider = ExceptionHelper.EnsureNotNull(stringCryptoProvider, nameof(stringCryptoProvider));
            _mainThreadDispatcher = ExceptionHelper.EnsureNotNull(mainThreadDispatcher, nameof(mainThreadDispatcher));
            _entitySerializer = ExceptionHelper.EnsureNotNull(entitySerializer, nameof(entitySerializer));
        }

        public Task WriteAsync(string key, T value, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                var entityString = _entitySerializer.Serialize(value);
                var encrypted = _stringCryptoProvider.Encrypt(entityString);
                _mainThreadDispatcher.EnqueueForExecution(new MainThreadPlayerPrefsSetString(key, encrypted));
            }, cancellationToken);
        }

        public Task DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            return _mainThreadDispatcher.Await(new MainThreadPlayerPrefsDeleteKey(key));
        }

        public Task<T> ReadAsync(string key, CancellationToken cancellationToken = default)
        {
            return Task.Run(async () =>
            {
                var entityString = await _mainThreadDispatcher.Await(new MainThreadPlayerPrefsGetString(key));

                if (string.IsNullOrEmpty(entityString))
                {
                    return default;
                }

                var decrypted = _stringCryptoProvider.Decrypt(entityString);
                return _entitySerializer.Deserialize<T>(decrypted);
            }, cancellationToken);
        }
    }
}