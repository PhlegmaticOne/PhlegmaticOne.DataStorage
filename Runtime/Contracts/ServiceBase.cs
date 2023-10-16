using System;
using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Storage.Base;

namespace PhlegmaticOne.DataStorage.Contracts {
    public abstract class ServiceBase<T> : IService where T : class, IModel {
        public IDataStorage DataStorage { get; set; }
        protected IValueSource<T> Model;

        public Type ModelType => typeof(T);
        
        public async Task<object> ForceReadAsync(IDataStorage dataStorage, CancellationToken ct = default) {
            var value = await dataStorage.ReadAsync<T>(ct);
            Model = value;
            return value.AsNoTrackable();
        }
        
        protected async Task<IValueSource<T>> ReadAsync(IDataStorage dataStorage, CancellationToken ct = default) {
            var value = await dataStorage.ReadAsync<T>(ct);
            Model = value;
            return value;
        }

        public Task SaveStateAsync(IDataStorage dataStorage, CancellationToken ct = default) {
            return dataStorage.SaveAsync(Model.AsNoTrackable(), ct);
        }

        public Task DeleteAsync(IDataStorage dataStorage, CancellationToken ct = default) {
            return dataStorage.DeleteAsync<T>(ct);
        }
    }
}