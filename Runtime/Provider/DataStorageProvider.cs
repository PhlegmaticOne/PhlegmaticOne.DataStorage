using System.Threading;
using PhlegmaticOne.DataStorage.DataSources;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.Infrastructure.Dispatcher;
using PhlegmaticOne.DataStorage.Provider.Base;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker;
using PhlegmaticOne.DataStorage.Storage.Queue;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Provider {
    public class DataStorageProvider {
        private readonly IDataStorageProviderConfig _providerConfig;

        public DataStorageProvider(IDataStorageProviderConfig providerConfig) {
            _providerConfig = providerConfig;
        }

        public DataStorageCreationResult CreateDataStorage() {
            return CreateDataStorage(_providerConfig);
        }
        
        public static DataStorageCreationResult CreateDataStorage(IDataStorageProviderConfig providerConfig) {
            var dataStorageConfig = providerConfig.DataStorageConfig.GetSourceFactory();
            var logger = providerConfig.LoggerConfig.GetLogger();
            var operationsQueueConfig = providerConfig.OperationsQueueConfig.GetOperationsQueueConfig();
            var changeTrackerConfig = providerConfig.ChangeTrackerConfig.GetChangeTrackerConfig();
            var threadDispatcher = CreateMainThreadDispatcher();

            var tokenSource = new CancellationTokenSource();
            var operationsQueue = new OperationsQueue(logger, operationsQueueConfig, tokenSource);
            var context = new DataSourceFactoryContext(threadDispatcher, logger, operationsQueue, dataStorageConfig);
            var dataStorage = CreateDataStorage(context, tokenSource);
            var changeTracker = new ChangeTrackerTimeInterval(dataStorage, changeTrackerConfig, logger, tokenSource);

            return new DataStorageCreationResult(dataStorage, changeTracker, tokenSource);
        }
        
        private static Storage.DataStorage CreateDataStorage(DataSourceFactoryContext factoryContext, CancellationTokenSource cts) {
            var dataSourceFactory = factoryContext.DataSourceFactory;
            var logger = factoryContext.Logger;
            var operationsQueue = factoryContext.OperationsQueue;
            var dataSourcesSet = new DataSourcesSet(dataSourceFactory, factoryContext);
            var storage = new Storage.DataStorage(logger, dataSourcesSet, operationsQueue, cts);
            _ = operationsQueue.ExecuteOperationsAsync();
            return storage;
        }
        
        private static IMainThreadDispatcher CreateMainThreadDispatcher() {
            var dispatcher = new GameObject(nameof(UnityMainThreadDispatcher));
            return dispatcher.AddComponent<UnityMainThreadDispatcher>();
        }
    }
}