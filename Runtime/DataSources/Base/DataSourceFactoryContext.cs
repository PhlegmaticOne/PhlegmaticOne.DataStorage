using PhlegmaticOne.DataStorage.Infrastructure.Dispatcher;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker;
using PhlegmaticOne.DataStorage.Storage.Queue.Base;

namespace PhlegmaticOne.DataStorage.DataSources.Base {
    public class DataSourceFactoryContext {
        public IDataSourceFactory DataSourceFactory { get; }
        public IOperationsQueue OperationsQueue { get; }
        public IMainThreadDispatcher MainThreadDispatcher { get; }
        public IDataStorageLogger Logger { get; }

        public DataSourceFactoryContext(
            IMainThreadDispatcher dispatcher,
            IDataStorageLogger logger,
            IOperationsQueue operationsQueue,
            IDataSourceFactory factoryConfig) {
            DataSourceFactory = ExceptionHelper.EnsureNotNull(factoryConfig, nameof(factoryConfig));
            OperationsQueue = ExceptionHelper.EnsureNotNull(operationsQueue, nameof(operationsQueue));
            MainThreadDispatcher = ExceptionHelper.EnsureNotNull(dispatcher, nameof(dispatcher));
            Logger = ExceptionHelper.EnsureNotNull(logger, nameof(logger));
        }
    }
}