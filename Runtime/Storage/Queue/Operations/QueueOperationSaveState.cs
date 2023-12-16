using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.Storage.Base;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker;
using PhlegmaticOne.DataStorage.Storage.Queue.Base;
using PhlegmaticOne.DataStorage.Storage.ValueSources;

namespace PhlegmaticOne.DataStorage.Storage.Queue.Operations
{
    public class QueueOperationSaveState<T> : IQueueOperation where T : class, IModel
    {
        private readonly IDataStorage _dataStorage;
        private readonly IDataStorageLogger _logger;
        private readonly IValueSource<T> _value;

        public QueueOperationSaveState(IValueSource<T> value, IDataStorageLogger logger, IDataStorage dataStorage)
        {
            _value = value;
            _dataStorage = dataStorage;
            _logger = logger;
        }

        public string OperationMessage => $"Saving {_value.DisplayName}";

        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogSaving(_value);
            return _dataStorage.SaveAsync(_value.Value, cancellationToken);
        }
    }
}