using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.Infrastructure;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;
using PhlegmaticOne.DataStorage.Storage.Base;

namespace PhlegmaticOne.DataStorage.Storage {
    public class DataStorage : IDataStorage {
        private readonly DataStorageSourcesContainer _sourcesContainer;
        private readonly Dictionary<Type, object> _objects;
        private readonly IList<IService> _services;

        public DataStorage(DataStorageSourcesContainer dataStorageSourcesContainer, IList<IService> services) {
            _sourcesContainer = ExceptionHelper.EnsureNotNull(dataStorageSourcesContainer, nameof(dataStorageSourcesContainer));
            _services = ExceptionHelper.EnsureNotNull(services, nameof(services));
            _objects = new Dictionary<Type, object>();
        }

        public async Task InitializeAsync(CancellationToken ct = default) {
            foreach (var service in _services) {
                await service.InitializeAsync(this, ct);
            }
        }

        public Task SaveAsync<T>(T value, CancellationToken ct, StorageOperationType operationType) {
            var source = _sourcesContainer.GetSource<T>(operationType);
            return source.WriteAsync(value, ct);
        }

        public async Task<T> ReadAsync<T>(CancellationToken ct, StorageOperationType operationType) {
            var type = typeof(T);
            if (_objects.TryGetValue(type, out var cached)) {
                return (T)cached;
            }
            
            var source = _sourcesContainer.GetSource<T>(operationType);
            var value = await source.ReadAsync(ct);
            _objects.Add(type, value);
            return value;
        }

        public Task DeleteAsync<T>(CancellationToken ct = default, StorageOperationType operationType = StorageOperationType.Auto) {
            var source = _sourcesContainer.GetSource<T>(operationType);
            return source.DeleteAsync(ct);
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
    }
}