using System;
using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Storage.Base;

namespace PhlegmaticOne.DataStorage.Contracts {
    public abstract class ServiceBase<T> : IService {
        protected IDataStorage DataStorage;
        protected T Model;

        public Type ModelType => typeof(T);
        protected virtual StorageOperationType OperationType => StorageOperationType.Auto;
        
        public async Task<object> ForceReadAsync(IDataStorage dataStorage, CancellationToken ct = default) {
            var value = await dataStorage.ReadAsync<T>(ct, OperationType);
            Model = value;
            return value;
        }

        public Task SaveStateAsync(IDataStorage dataStorage, CancellationToken ct) {
            return dataStorage.SaveAsync(Model, ct, OperationType);
        }

        public Task DeleteAsync(IDataStorage dataStorage, CancellationToken ct = default) {
            return dataStorage.DeleteAsync<T>(ct, OperationType);
        }

        public Task InitializeAsync(IDataStorage dataStorage, CancellationToken ct) {
            DataStorage = dataStorage;
            return OnInitializingAsync(ct);
        }

        protected virtual Task OnInitializingAsync(CancellationToken ct) => Task.CompletedTask;
        protected Task SaveModelAsync(CancellationToken ct = default) => DataStorage.SaveAsync(Model, ct);
    }
}