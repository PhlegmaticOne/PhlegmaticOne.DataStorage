namespace PhlegmaticOne.DataStorage.Storage.Queue.Base
{
    public interface IOperationsQueue
    {
        int Count { get; }
        void Run();
        void EnqueueOperation(IQueueOperation queueOperation);
    }
}