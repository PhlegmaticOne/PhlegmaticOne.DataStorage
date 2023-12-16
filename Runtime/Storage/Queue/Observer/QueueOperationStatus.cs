namespace PhlegmaticOne.DataStorage.Storage.Queue.Events
{
    public enum QueueOperationStatus
    {
        None = 0,
        PendingForExecution = 1,
        Running = 2,
        Completed = 3,
        CancelledInternal = 4,
        CancelledExternal = 5,
        Faulted = 6
    }
}