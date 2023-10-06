using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;

namespace PhlegmaticOne.DataStorage.Storage.Base {
    public interface IDataStorage {
        void Initialize();
        Task SaveAsync<T>(T value, CancellationToken ct = default, StorageOperationType operationType = StorageOperationType.Auto) where T: class, IModel;
        Task<IValueSource<T>> ReadAsync<T>(CancellationToken ct = default, StorageOperationType operationType = StorageOperationType.Auto) where T: class, IModel;
        Task DeleteAsync<T>(CancellationToken ct = default, StorageOperationType operationType = StorageOperationType.Auto) where T: class, IModel;
        Task SaveStateAsync(CancellationToken ct = default);
        Task ClearAsync(CancellationToken ct = default);
    }
}