using System;
using Newtonsoft.Json;
using PhlegmaticOne.DataStorage.Storage.Queue.Events;

namespace PhlegmaticOne.DataStorage.Storage.Queue.Observer
{
    [Serializable]
    public class QueueOperationState
    {
        [JsonConstructor]
        public QueueOperationState(string operationMessage, QueueOperationStatus status, DateTime occuredAt,
            string errorMessage = "")
        {
            OperationMessage = operationMessage;
            Status = status;
            OccuredAt = occuredAt;
            ErrorMessage = errorMessage;
        }

        public string OperationMessage { get; }
        public QueueOperationStatus Status { get; }
        public DateTime OccuredAt { get; }
        public string ErrorMessage { get; }

        [JsonIgnore] public bool IsError => !string.IsNullOrEmpty(ErrorMessage);

        public string ToLogMessage() => $"{OccuredAt.ToShortTimeString()}\n{OperationMessage} - {Status}";
    }
}