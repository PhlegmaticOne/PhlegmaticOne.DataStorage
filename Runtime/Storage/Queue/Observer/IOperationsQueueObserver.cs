using System;

namespace PhlegmaticOne.DataStorage.Storage.Queue.Observer
{
    public interface IOperationsQueueObserver
    {
        int EnqueuedOperationsCount { get; }
        int OperationsCapacity { get; }
        event Action<QueueOperationState> OperationChanged;
    }
}