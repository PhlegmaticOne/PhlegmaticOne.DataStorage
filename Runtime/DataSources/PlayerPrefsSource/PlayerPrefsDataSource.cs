using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Actions;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Crypto;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Serializers;
using PhlegmaticOne.DataStorage.Infrastructure.Dispatcher;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;
using PhlegmaticOne.DataStorage.Infrastructure.KeyResolvers.Base;

namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource
{
    public class PlayerPrefsDataSource<T> : IDataSource<T> where T : class, IModel
    {
        private readonly IEntitySerializer _entitySerializer;
        private readonly IKeyResolver _keyResolver;
        private readonly IMainThreadDispatcher _mainThreadDispatcher;
        private readonly IStringCryptoProvider _stringCryptoProvider;

        public PlayerPrefsDataSource(IEntitySerializer entitySerializer,
            IStringCryptoProvider stringCryptoProvider,
            IMainThreadDispatcher mainThreadDispatcher,
            IKeyResolver keyResolver)
        {
            _stringCryptoProvider = ExceptionHelper.EnsureNotNull(stringCryptoProvider, nameof(stringCryptoProvider));
            _mainThreadDispatcher = ExceptionHelper.EnsureNotNull(mainThreadDispatcher, nameof(mainThreadDispatcher));
            _entitySerializer = ExceptionHelper.EnsureNotNull(entitySerializer, nameof(entitySerializer));
            _keyResolver = ExceptionHelper.EnsureNotNull(keyResolver, nameof(keyResolver));
        }

        public Task WriteAsync(T value, CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                var key = _keyResolver.ResolveKey<T>();
                var entityString = _entitySerializer.Serialize(value);
                var encrypted = _stringCryptoProvider.Encrypt(entityString);
                _mainThreadDispatcher.EnqueueForExecution(new MainThreadPlayerPrefsSetString(key, encrypted));
            }, cancellationToken);
        }

        public Task DeleteAsync(CancellationToken cancellationToken = default)
        {
            var key = _keyResolver.ResolveKey<T>();
            return _mainThreadDispatcher.AwaitExecution(new MainThreadPlayerPrefsDeleteKey(key));
        }

        public Task<T> ReadAsync(CancellationToken cancellationToken = default)
        {
            return Task.Run(async () =>
            {
                var key = _keyResolver.ResolveKey<T>();
                var entityString = await _mainThreadDispatcher.AwaitExecution(new MainThreadPlayerPrefsGetString(key));

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