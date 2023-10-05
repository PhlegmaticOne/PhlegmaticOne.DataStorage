using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Storage.Base;

namespace PhlegmaticOne.DataStorage.Storage {
    public static class DataStorageExtensions {

        public static Task<T> ReadLocalAsync<T>(this IDataStorage dataStorage, CancellationToken ct = default) =>
            dataStorage.ReadAsync<T>(ct, StorageOperationType.Local);

        public static Task<T> ReadOnlineAsync<T>(this IDataStorage dataStorage, CancellationToken ct = default) =>
            dataStorage.ReadAsync<T>(ct, StorageOperationType.Online);
    }
}