using System;

namespace PhlegmaticOne.DataStorage.Storage.Queue.Observer {
    public interface IOperationsQueueObserver {
        event Action<QueueOperationState> OperationChanged;
        int EnqueuedOperationsCount { get; }
        int OperationsCapacity { get; }
    }
}