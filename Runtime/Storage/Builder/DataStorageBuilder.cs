using System;
using PhlegmaticOne.DataStorage.DataSources;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.InMemorySource.Factory;
using PhlegmaticOne.DataStorage.Infrastructure.Cancellation;
using PhlegmaticOne.DataStorage.Infrastructure.Dispatcher;
using PhlegmaticOne.DataStorage.Storage.Base;
using PhlegmaticOne.DataStorage.Storage.Builder.Base;
using PhlegmaticOne.DataStorage.Storage.ChangeTracking;
using PhlegmaticOne.DataStorage.Storage.Logger;
using PhlegmaticOne.DataStorage.Storage.Queue;

namespace PhlegmaticOne.DataStorage.Storage.Builder
{
    public class DataStorageBuilder : IDataSourceBuildStep, IDataStorageBuilder
    {
        private IDataStorageLogger _logger;
        private ChangeTrackerConfig _changeTrackerConfig;
        private IDataSourceFactory _dataSourceFactory;

        public static IDataSourceBuildStep Create() => new DataStorageBuilder();
        
        internal DataStorageBuilder()
        {
            _logger = new DataStorageLoggerEmpty();
            _dataSourceFactory = new DataSourceFactoryInMemory();
        }

        public IDataStorageBuilder UseDataSource(Func<DataStorageDataSourceBuilder, IDataSourceFactory> sourceBuilderAction)
        {
            var builder = new DataStorageDataSourceBuilder();
            _dataSourceFactory = sourceBuilderAction(builder);
            return this;
        }

        public IDataStorageBuilder UseDataSource(IDataSourceFactory dataSourceFactory)
        {
            _dataSourceFactory = dataSourceFactory;
            return this;
        }

        public IDataStorageBuilder UseLogger(IDataStorageLogger logger)
        {
            _logger = logger;
            return this;
        }

        public IDataStorageBuilder UseLogger(Action<DataStorageLoggerConfig> configAction = null)
        {
            var config = new DataStorageLoggerConfig();
            configAction?.Invoke(config);
            _logger = new DataStorageLoggerDebug(config);
            return this;
        }

        public IDataStorageBuilder UseChangeTracker(Action<ChangeTrackerConfig> configAction)
        {
            var config = new ChangeTrackerConfig();
            configAction?.Invoke(config);
            _changeTrackerConfig = config;
            return this;
        }

        public IDataStorage Build()
        {
            var cancellationProvider = new DataStorageCancellationProvider();
            var threadDispatcher = new MainThreadDispatcher(cancellationProvider);
            var operationsQueue = new OperationsQueue(_logger, cancellationProvider);
            var factoryContext = new DataSourceFactoryContext(threadDispatcher);
            var dataSourcesSet = new DataSourcesSet(_dataSourceFactory, factoryContext);
            var dataStorage = new DataStorage(_logger, dataSourcesSet, operationsQueue, cancellationProvider);
            
            operationsQueue.Run();
            threadDispatcher.Run();

            if (_changeTrackerConfig is not null)
            {
                ChangeTracker.Run(dataStorage, _changeTrackerConfig, _logger, cancellationProvider);
            }

            return dataStorage;
        }
    }
}