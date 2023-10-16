using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KeyedSemaphores;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;
using PhlegmaticOne.DataStorage.Storage.Base;

namespace PhlegmaticOne.DataStorage.Storage {
    public class DataStorage : IDataStorage {
        private readonly DataSourcesSet _dataSourcesSet;
        private readonly Dictionary<string, IValueSource> _valueSources;
        private readonly IList<IService> _services;
        
        public DataStorage(DataSourcesSet dataSourcesSet, IList<IService> services = null) {
            _dataSourcesSet = ExceptionHelper.EnsureNotNull(dataSourcesSet, nameof(dataSourcesSet));
            _services = services ?? new List<IService>();
            _valueSources = new Dictionary<string, IValueSource>();
        }

        internal ICollection<IValueSource> ValueSources => _valueSources.Values;

        public void Initialize() {
            foreach (var service in _services) {
                service.DataStorage = this;
            }
        }

        public async Task SaveAsync<T>(T value, CancellationToken ct = default) where T: class, IModel {
            var key = LockKey<T>();
            using var _ = await KeyedSemaphore.LockAsync(key, ct);
            var source = _dataSourcesSet.Source<T>();
            await source.WriteAsync(value, ct);
        }

        public async Task<IValueSource<T>> ReadAsync<T>(CancellationToken ct = default) where T: class, IModel {
            var key = LockKey<T>();
            using var _ = await KeyedSemaphore.LockAsync(key, ct);
            
            if (_valueSources.TryGetValue(key, out var cached)) {
                return (IValueSource<T>)cached;
            }
            
            var source = _dataSourcesSet.Source<T>();
            var value = await source.ReadAsync(ct);
            var valueSource = new ValueSource<T>(this, value);
            _valueSources.Add(key, valueSource);
            return valueSource;
        }

        public async Task DeleteAsync<T>(CancellationToken ct = default) where T: class, IModel {
            var key = LockKey<T>();
            using var _ = await KeyedSemaphore.LockAsync(key, ct);
            var source = _dataSourcesSet.Source<T>();
            await source.DeleteAsync(ct);
        }

        public async Task SaveStateAsync(CancellationToken ct = default) {
            var serviceTypes = new HashSet<Type>();

            foreach (var service in _services) {
                if (serviceTypes.Add(service.ModelType) == false) {
                    continue;
                }
            
                await service.SaveStateAsync(this, ct);
            }
        }

        public async Task ClearAsync(CancellationToken ct = default) {
            var serviceTypes = new HashSet<Type>();

            foreach (var service in _services) {
                if (serviceTypes.Add(service.ModelType) == false) {
                    continue;
                }
            
                await service.DeleteAsync(this, ct);
            }
        }

        private static string LockKey<T>() => typeof(T).Name;
    }
}