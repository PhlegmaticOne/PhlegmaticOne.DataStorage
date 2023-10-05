using System.Threading;
using System.Threading.Tasks;

namespace PhlegmaticOne.DataStorage.Storage.Base {
    public interface IDataStorage {
        Task InitializeAsync(CancellationToken ct = default);
        Task SaveAsync<T>(T value, CancellationToken ct = default, StorageOperationType operationType = StorageOperationType.Auto);
        Task<T> ReadAsync<T>(CancellationToken ct = default, StorageOperationType operationType = StorageOperationType.Auto);
        Task DeleteAsync<T>(CancellationToken ct = default, StorageOperationType operationType = StorageOperationType.Auto);
        Task SaveStateAsync(CancellationToken ct = default);
        Task ClearAsync(CancellationToken ct = default);
    }
}