using System.Threading;
using System.Threading.Tasks;

namespace PhlegmaticOne.DataStorage.Storage.Queue.Base
{
    public interface IQueueOperation
    {
        string OperationMessage { get; }
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}