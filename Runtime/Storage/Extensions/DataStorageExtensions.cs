using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.Storage.Base;
using PhlegmaticOne.DataStorage.Storage.ValueSources;

namespace PhlegmaticOne.DataStorage.Storage.Extensions {
    public static class DataStorageExtensions {
        public static async Task<IValueSource<T>> GetInitializedValueSource<T>(
            this IDataStorage dataStorage, CancellationToken cancellationToken = default) where T : class, IModel {
            var valueSource = dataStorage.GetOrCreateValueSource<T>();
            await valueSource.InitializeAsync(cancellationToken);
            return valueSource;
        }
    }
}