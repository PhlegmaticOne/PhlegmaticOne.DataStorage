using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.Infrastructure;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;
using PhlegmaticOne.DataStorage.Storage.Base;

namespace PhlegmaticOne.DataStorage.Storage {
    public class DataStorage : IDataStorage {
        private static readonly ReaderWriterLock ReaderWriterLock = new ReaderWriterLock();
        
        private readonly ConcurrentDictionary<Type, IValueSource> _valueSources;
        private readonly DataStorageSourcesContainer _sourcesContainer;
        private readonly IList<IService> _services;
        
        public DataStorage(DataStorageSourcesContainer dataStorageSourcesContainer, IList<IService> services) {
            _sourcesContainer = ExceptionHelper.EnsureNotNull(dataStorageSourcesContainer, nameof(dataStorageSourcesContainer));
            _services = ExceptionHelper.EnsureNotNull(services, nameof(services));
            _valueSources = new ConcurrentDictionary<Type, IValueSource>();
        }

        internal ICollection<IValueSource> ValueSources => _valueSources.Values;

        public void Initialize() {
            foreach (var service in _services) {
                service.DataStorage = this;
            }
        }

        public Task SaveAsync<T>(T value, CancellationToken ct, StorageOperationType operationType) where T: class, IModel {
            ReaderWriterLock.AcquireWriterLock(-1);
            try {
                var source = _sourcesContainer.GetSource<T>(operationType);
                return source.WriteAsync(value, ct);
            }
            finally {
                ReaderWriterLock.ReleaseWriterLock();
            }
        }

        public async Task<IValueSource<T>> ReadAsync<T>(CancellationToken ct, StorageOperationType operationType) where T: class, IModel {
            ReaderWriterLock.AcquireReaderLock(-1);
            try {
                var type = typeof(T);
                if (_valueSources.TryGetValue(type, out var cached)) {
                    return (IValueSource<T>)cached;
                }
            
                var source = _sourcesContainer.GetSource<T>(operationType);
                var value = await source.ReadAsync(ct);
                var valueSource = new ValueSource<T>(this, value);
                _valueSources.TryAdd(type, valueSource);
                return valueSource;
            }
            finally {
                ReaderWriterLock.ReleaseReaderLock();
            }
        }

        public Task DeleteAsync<T>(CancellationToken ct = default, StorageOperationType operationType = StorageOperationType.Auto) where T: class, IModel {
            ReaderWriterLock.AcquireWriterLock(-1);
            try {
                var source = _sourcesContainer.GetSource<T>(operationType);
                return source.DeleteAsync(ct);
            }
            finally {
                ReaderWriterLock.ReleaseWriterLock();
            }
        }

        public async Task SaveStateAsync(CancellationToken ct = default) {
            var serviceTypes = new HashSet<Type>();
            ReaderWriterLock.AcquireWriterLock(-1);

            try {
                foreach (var service in _services) {
                    if (serviceTypes.Add(service.ModelType) == false) {
                        continue;
                    }
                
                    await service.SaveStateAsync(this, ct);
                }
            }
            finally {
                ReaderWriterLock.ReleaseWriterLock();
            }
        }

        public async Task ClearAsync(CancellationToken ct = default) {
            var serviceTypes = new HashSet<Type>();
            ReaderWriterLock.AcquireWriterLock(-1);

            try {
                foreach (var service in _services) {
                    if (serviceTypes.Add(service.ModelType) == false) {
                        continue;
                    }
                
                    await service.DeleteAsync(this, ct);
                }
            }
            finally {
                ReaderWriterLock.ReleaseWriterLock();
            }
        }
    }
}