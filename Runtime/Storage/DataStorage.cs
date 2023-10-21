using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KeyedSemaphores;
using PhlegmaticOne.DataStorage.Configs;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;
using PhlegmaticOne.DataStorage.Storage.Base;

namespace PhlegmaticOne.DataStorage.Storage {
    public class DataStorage : IDataStorage {
        private readonly DataSourcesSet _dataSourcesSet;
        private readonly Dictionary<string, IValueSource> _valueSources;
        private CancellationToken _cancellationToken;
        
        public DataStorage(DataSourcesSet dataSourcesSet) {
            _dataSourcesSet = ExceptionHelper.EnsureNotNull(dataSourcesSet, nameof(dataSourcesSet));
            _valueSources = new Dictionary<string, IValueSource>();
        }

        public static DataStorage FromConfig(IDataStorageSourceFactoryConfig dataStorageSourceFactoryConfig) {
            var dataSourceFactory = dataStorageSourceFactoryConfig.GetSourceFactory();
            var dataSourcesSet = new DataSourcesSet(dataSourceFactory);
            return new DataStorage(dataSourcesSet);
        }

        public DataStorage WithCancellation(CancellationToken cancellationToken) {
            _cancellationToken = cancellationToken;
            return this;
        }
        
        public async Task SaveAsync<T>(T value, CancellationToken ct = default) where T: class, IModel {
            var key = LockKey<T>();
            var token = Token(ct);
            using var _ = await KeyedSemaphore.LockAsync(key, ct);
            var source = _dataSourcesSet.Source<T>();
            await source.WriteAsync(value, token);
        }

        public async Task<IValueSource<T>> ReadAsync<T>(CancellationToken ct = default) where T: class, IModel {
            var key = LockKey<T>();
            var token = Token(ct);
            using var _ = await KeyedSemaphore.LockAsync(key, token);
            
            if (_valueSources.TryGetValue(key, out var cached)) {
                return (IValueSource<T>)cached;
            }
            
            var source = _dataSourcesSet.Source<T>();
            var value = await source.ReadAsync(token);
            var valueSource = new ValueSource<T>(this, value);
            _valueSources.Add(key, valueSource);
            return valueSource;
        }

        public async Task<T> ReadRawValueAsync<T>(CancellationToken ct = default) where T : class, IModel {
            var key = LockKey<T>();
            var token = Token(ct);
            using var _ = await KeyedSemaphore.LockAsync(key, token);
            var source = _dataSourcesSet.Source<T>();
            return await source.ReadAsync(token);
        }

        public async Task DeleteAsync<T>(CancellationToken ct = default) where T: class, IModel {
            var key = LockKey<T>();
            var token = Token(ct);
            using var _ = await KeyedSemaphore.LockAsync(key, token);
            var source = _dataSourcesSet.Source<T>();
            await source.DeleteAsync(token);
        }

        internal ICollection<IValueSource> ValueSources => _valueSources.Values;
        
        internal void AddValueSource<T>(IValueSource<T> valueSource) {
            var key = LockKey<T>();
            using var _ = KeyedSemaphore.Lock(key);
            
            if (_valueSources.ContainsKey(key) == false) {
                _valueSources.Add(key, valueSource);
            }
        }

        private CancellationToken Token(CancellationToken ct) => ct == default ? _cancellationToken : ct;
        private static string LockKey<T>() => typeof(T).Name;
    }
}