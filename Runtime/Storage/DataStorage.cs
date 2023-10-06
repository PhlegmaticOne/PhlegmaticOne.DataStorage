using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KeyedSemaphores;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.Infrastructure;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;
using PhlegmaticOne.DataStorage.Storage.Base;

namespace PhlegmaticOne.DataStorage.Storage {
    public class DataStorage : IDataStorage {
        private readonly Dictionary<string, IValueSource> _valueSources;
        private readonly DataStorageSourcesContainer _sourcesContainer;
        private readonly IList<IService> _services;
        
        public DataStorage(DataStorageSourcesContainer dataStorageSourcesContainer, IList<IService> services) {
            _sourcesContainer = ExceptionHelper.EnsureNotNull(dataStorageSourcesContainer, nameof(dataStorageSourcesContainer));
            _services = ExceptionHelper.EnsureNotNull(services, nameof(services));
            _valueSources = new Dictionary<string, IValueSource>();
        }

        internal ICollection<IValueSource> ValueSources => _valueSources.Values;

        public void Initialize() {
            foreach (var service in _services) {
                service.DataStorage = this;
            }
        }

        public async Task SaveAsync<T>(T value, CancellationToken ct, StorageOperationType operationType) where T: class, IModel {
            using var _ = await KeyedSemaphore.LockAsync(LockKey<T>(), ct);
            var source = _sourcesContainer.GetSource<T>(operationType);
            await source.WriteAsync(value, ct);
        }

        public async Task<IValueSource<T>> ReadAsync<T>(CancellationToken ct, StorageOperationType operationType) where T: class, IModel {
            var key = LockKey<T>();
            using var _ = await KeyedSemaphore.LockAsync(key, ct);
            
            if (_valueSources.TryGetValue(key, out var cached)) {
                return (IValueSource<T>)cached;
            }
            
            var source = _sourcesContainer.GetSource<T>(operationType);
            var value = await source.ReadAsync(ct);
            var valueSource = new ValueSource<T>(this, value);
            _valueSources.Add(key, valueSource);
            return valueSource;
        }

        public async Task DeleteAsync<T>(CancellationToken ct = default, StorageOperationType operationType = StorageOperationType.Auto) where T: class, IModel {
            using var _ = await KeyedSemaphore.LockAsync(LockKey<T>(), ct);
            var source = _sourcesContainer.GetSource<T>(operationType);
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