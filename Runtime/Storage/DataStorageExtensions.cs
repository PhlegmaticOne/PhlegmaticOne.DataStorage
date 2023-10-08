using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.Storage.Base;

namespace PhlegmaticOne.DataStorage.Storage {
    public static class DataStorageExtensions {

        public static Task<IValueSource<T>> ReadLocalAsync<T>(this IDataStorage dataStorage, CancellationToken ct = default) where T: class, IModel =>
            dataStorage.ReadAsync<T>(ct, StorageOperationType.Local);

        public static Task<IValueSource<T>> ReadOnlineAsync<T>(this IDataStorage dataStorage, CancellationToken ct = default) where T: class, IModel =>
            dataStorage.ReadAsync<T>(ct, StorageOperationType.Online);

        public static bool NoValue<T>(this IValueSource<T> valueSource) where T : class, IModel => 
            valueSource is null || valueSource.AsNoTrackable() is null;

        public static bool HasValue<T>(this IValueSource<T> valueSource) where T : class, IModel => 
            valueSource != null && valueSource.AsNoTrackable() != null;
    }
}