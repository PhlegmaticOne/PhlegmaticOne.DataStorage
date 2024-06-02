using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.Storage.Base;
using PhlegmaticOne.DataStorage.Storage.Queue.Base;

namespace PhlegmaticOne.DataStorage.Storage.Queue.Operations
{
    internal sealed class QueueOperationDeleteState<T> : IQueueOperation where T : class, IModel
    {
        private readonly IDataStorage _dataStorage;
        private readonly string _key;

        public QueueOperationDeleteState(IDataStorage dataStorage, string key)
        {
            _dataStorage = dataStorage;
            _key = key;
        }

        public Task ExecuteAsync()
        {
            return _dataStorage.DeleteAsync<T>(_key);
        }
    }
}