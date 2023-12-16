using PhlegmaticOne.DataStorage.Provider.Configs;

namespace PhlegmaticOne.DataStorage.Provider.Base
{
    public interface IDataStorageProviderConfig
    {
        IChangeTrackerConfig ChangeTrackerConfig { get; }
        IDataStorageLoggerConfig LoggerConfig { get; }
        IDataStorageConfig DataStorageConfig { get; }
        IOperationsQueueConfig OperationsQueueConfig { get; }
    }
}