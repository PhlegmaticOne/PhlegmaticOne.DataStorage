using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.Storage.Base;
using PhlegmaticOne.DataStorage.Storage.Logger;
using PhlegmaticOne.DataStorage.Storage.Queue.Base;
using PhlegmaticOne.DataStorage.Storage.ValueSources;

namespace PhlegmaticOne.DataStorage.Storage.Queue.Operations
{
    internal sealed class QueueOperationSaveState<T> : IQueueOperation where T : class, IModel
    {
        private readonly IDataStorage _dataStorage;
        private readonly IDataStorageLogger _logger;
        private readonly string _key;
        private readonly T _value;

        public QueueOperationSaveState(string key, T value, IDataStorageLogger logger, IDataStorage dataStorage)
        {
            _key = key;
            _value = value;
            _dataStorage = dataStorage;
            _logger = logger;
        }

        public Task ExecuteAsync()
        {
            _logger.LogSaving(_key);
            return _dataStorage.SaveAsync(_key, _value);
        }
    }
}