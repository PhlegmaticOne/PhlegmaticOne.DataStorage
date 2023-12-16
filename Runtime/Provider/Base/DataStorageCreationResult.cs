using PhlegmaticOne.DataStorage.Infrastructure.Cancellation;
using PhlegmaticOne.DataStorage.Storage.Base;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker.Base;

namespace PhlegmaticOne.DataStorage.Provider.Base
{
    public class DataStorageCreationResult
    {
        public DataStorageCreationResult(IDataStorage dataStorage, IChangeTracker changeTracker,
            IDataStorageCancellationProvider cancellationProvider)
        {
            DataStorage = dataStorage;
            ChangeTracker = changeTracker;
            CancellationProvider = cancellationProvider;
        }

        public IDataStorage DataStorage { get; }
        public IChangeTracker ChangeTracker { get; }
        public IDataStorageCancellationProvider CancellationProvider { get; }
    }
}