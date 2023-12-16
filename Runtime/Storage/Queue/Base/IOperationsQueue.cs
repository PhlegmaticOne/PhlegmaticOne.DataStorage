using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Storage.Queue.Observer;

namespace PhlegmaticOne.DataStorage.Storage.Queue.Base
{
    public interface IOperationsQueue : IOperationsQueueObserver
    {
        void EnqueueOperation(IQueueOperation queueOperation, CancellationToken cancellationToken = default);
        Task ExecuteOperationsAsync(CancellationToken cancellationToken = default);
    }
}