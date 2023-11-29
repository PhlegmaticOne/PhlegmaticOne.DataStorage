using System.Threading;
using PhlegmaticOne.DataStorage.Storage.Base;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker.Base;

namespace PhlegmaticOne.DataStorage.Provider.Base {
    public class DataStorageCreationResult {
        public DataStorageCreationResult(IDataStorage dataStorage, IChangeTracker changeTracker, CancellationTokenSource tokenSource) {
            DataStorage = dataStorage;
            ChangeTracker = changeTracker;
            TokenSource = tokenSource;
        }

        public IDataStorage DataStorage { get; }
        public IChangeTracker ChangeTracker { get; }
        public CancellationTokenSource TokenSource { get; }
    }
}