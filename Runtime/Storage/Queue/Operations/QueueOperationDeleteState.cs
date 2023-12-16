using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.Storage.Base;
using PhlegmaticOne.DataStorage.Storage.Queue.Base;

namespace PhlegmaticOne.DataStorage.Storage.Queue.Operations
{
    public class QueueOperationDeleteState<T> : IQueueOperation where T : class, IModel
    {
        private readonly IDataStorage _dataStorage;
        public QueueOperationDeleteState(IDataStorage dataStorage) => _dataStorage = dataStorage;
        public string OperationMessage => $"Deleting {typeof(T)}";

        public Task ExecuteAsync(CancellationToken cancellationToken = default) =>
            _dataStorage.DeleteAsync<T>(cancellationToken);
    }
}