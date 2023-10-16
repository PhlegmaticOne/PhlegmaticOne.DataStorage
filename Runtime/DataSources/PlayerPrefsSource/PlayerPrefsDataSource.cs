using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Serializers;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;
using PhlegmaticOne.DataStorage.KeyResolvers.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource {
    internal sealed class PlayerPrefsDataSource<T> : DataSourceBase<T> where T: class, IModel {
        private readonly IEntitySerializer _entitySerializer;
        private readonly IKeyResolver _keyResolver;

        public PlayerPrefsDataSource(IEntitySerializer entitySerializer, IKeyResolver keyResolver) {
            _entitySerializer = ExceptionHelper.EnsureNotNull(entitySerializer, nameof(entitySerializer));
            _keyResolver = ExceptionHelper.EnsureNotNull(keyResolver, nameof(keyResolver));
        }

        protected override async Task WriteAsync(T value, CancellationToken cancellationToken = default) {
            var key = _keyResolver.ResolveKey<T>();
            var entityString = await Task.Run(() => _entitySerializer.Serialize(value), cancellationToken);
            PlayerPrefs.SetString(key, entityString);
        }

        public override Task DeleteAsync(CancellationToken cancellationToken = default) {
            var key = _keyResolver.ResolveKey<T>();
            PlayerPrefs.DeleteKey(key);
            return Task.CompletedTask;
        }

        public override async Task<T> ReadAsync(CancellationToken cancellationToken = default) {
            var key = _keyResolver.ResolveKey<T>();
            var entityString = PlayerPrefs.GetString(key, string.Empty);
            
            if (string.IsNullOrEmpty(entityString)) {
                return default;
            }

            var parsed = await Task.Run(() => _entitySerializer.Deserialize<T>(entityString), cancellationToken); 
            return parsed;
        }
    }
}