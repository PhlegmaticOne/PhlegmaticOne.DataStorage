using PhlegmaticOne.DataStorage.DataSources;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.Infrastructure.Cancellation;
using PhlegmaticOne.DataStorage.Infrastructure.Dispatcher;
using PhlegmaticOne.DataStorage.Provider.Base;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker;
using PhlegmaticOne.DataStorage.Storage.Queue;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Provider
{
    public class DataStorageProvider
    {
        private readonly IDataStorageProviderConfig _providerConfig;

        public DataStorageProvider(IDataStorageProviderConfig providerConfig) => _providerConfig = providerConfig;

        public DataStorageCreationResult CreateDataStorage() => CreateDataStorage(_providerConfig);

        public static DataStorageCreationResult CreateDataStorage(IDataStorageProviderConfig providerConfig)
        {
            var dataStorageConfig = providerConfig.DataStorageConfig.GetSourceFactory();
            var logger = providerConfig.LoggerConfig.GetLogger();
            var operationsQueueConfig = providerConfig.OperationsQueueConfig.GetOperationsQueueConfig();
            var changeTrackerConfig = providerConfig.ChangeTrackerConfig.GetChangeTrackerConfig();
            var threadDispatcher = CreateMainThreadDispatcher();

            var cancellationProvider = new DataStorageCancellationProvider();
            var operationsQueue = new OperationsQueue(logger, operationsQueueConfig, cancellationProvider);
            var context = new DataSourceFactoryContext(threadDispatcher, logger, operationsQueue, dataStorageConfig,
                cancellationProvider);
            var dataStorage = CreateDataStorage(context);
            var changeTracker =
                new ChangeTrackerTimeInterval(dataStorage, changeTrackerConfig, logger, cancellationProvider);

            return new DataStorageCreationResult(dataStorage, changeTracker, cancellationProvider);
        }

        private static Storage.DataStorage CreateDataStorage(DataSourceFactoryContext factoryContext)
        {
            var dataSourceFactory = factoryContext.DataSourceFactory;
            var logger = factoryContext.Logger;
            var operationsQueue = factoryContext.OperationsQueue;
            var cancellationProvider = factoryContext.CancellationProvider;
            var dataSourcesSet = new DataSourcesSet(dataSourceFactory, factoryContext);
            var storage = new Storage.DataStorage(logger, dataSourcesSet, operationsQueue, cancellationProvider);
            _ = operationsQueue.ExecuteOperationsAsync();
            return storage;
        }

        private static IMainThreadDispatcher CreateMainThreadDispatcher()
        {
            var dispatcher = new GameObject(nameof(UnityMainThreadDispatcher));
            return dispatcher.AddComponent<UnityMainThreadDispatcher>();
        }
    }
}