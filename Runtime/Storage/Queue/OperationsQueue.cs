using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Infrastructure.Cancellation;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker;
using PhlegmaticOne.DataStorage.Storage.Queue.Base;
using PhlegmaticOne.DataStorage.Storage.Queue.Events;
using PhlegmaticOne.DataStorage.Storage.Queue.Observer;

namespace PhlegmaticOne.DataStorage.Storage.Queue {
    public class OperationsQueue : IOperationsQueue {
        private const string CancellationSource = "OperationsQueue";
        
        private readonly IDataStorageLogger _logger;
        private readonly OperationsQueueConfiguration _configuration;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly BlockingCollection<IQueueOperation> _queueOperations;

        public int EnqueuedOperationsCount => _queueOperations.Count;
        public int OperationsCapacity => _queueOperations.BoundedCapacity;

        public event Action<QueueOperationState> OperationChanged;
        
        public OperationsQueue(IDataStorageLogger logger, 
            OperationsQueueConfiguration configuration,
            CancellationTokenSource cancellationTokenSource) {
            _logger = logger;
            _configuration = configuration;
            _cancellationTokenSource = cancellationTokenSource;
            _queueOperations = CreateOperationsQueue();
        }

        public void EnqueueOperation(IQueueOperation queueOperation, CancellationToken cancellationToken = default) {
            using var tokenSource = _cancellationTokenSource.LinkWith(cancellationToken);

            try {
                _queueOperations.Add(queueOperation, tokenSource.Token);
                RaiseOperationChanged(queueOperation, QueueOperationStatus.PendingForExecution);
            }
            catch (OperationCanceledException) {
                _logger.LogCancellation(CancellationSource);
                RaiseOperationChanged(queueOperation, GetCancellationStatus());
            }
            catch (Exception exception) {
                _logger.LogException(exception);
                RaiseOperationChanged(queueOperation, QueueOperationStatus.Faulted, exception.Message);
            }
        }

        public Task ExecuteOperationsAsync(CancellationToken cancellationToken = default) {
            var tokenSource = _cancellationTokenSource.LinkWith(cancellationToken);
            
            return Task.Run(async () => {
                var token = tokenSource.Token;
                
                while (token.IsCancellationRequested == false) {
                    IQueueOperation queueOperation = default;

                    try {
                        queueOperation = _queueOperations.Take(tokenSource.Token);
                        RaiseOperationChanged(queueOperation, QueueOperationStatus.Running);
                        await queueOperation.ExecuteAsync(tokenSource.Token);
                        RaiseOperationChanged(queueOperation, QueueOperationStatus.Completed);
                    }
                    catch (OperationCanceledException) {
                        _logger.LogCancellation(CancellationSource);
                        RaiseOperationChanged(queueOperation, GetCancellationStatus());
                    }
                    catch (Exception exception) {
                        _logger.LogException(exception);
                        RaiseOperationChanged(queueOperation, QueueOperationStatus.Faulted, exception.Message);
                    }
                }
            }, tokenSource.Token);
        }

        private void RaiseOperationChanged(IQueueOperation operation, QueueOperationStatus status, string errorMessage = "") {
            if (OperationChanged == null) {
                return;
            }
            
            var operationMessage = operation == null ? string.Empty : operation.OperationMessage;
            var occuredAt = DateTime.UtcNow;
            var queueOperationState = new QueueOperationState(operationMessage, status, occuredAt, errorMessage);
            OperationChanged.Invoke(queueOperationState);
        }

        private QueueOperationStatus GetCancellationStatus() {
            return _cancellationTokenSource.Token.IsCancellationRequested ?
                QueueOperationStatus.CancelledInternal : 
                QueueOperationStatus.CancelledExternal;
        }

        private BlockingCollection<IQueueOperation> CreateOperationsQueue() {
            return _configuration.IsUnlimitedCapacity ?
                new BlockingCollection<IQueueOperation>() : 
                new BlockingCollection<IQueueOperation>(_configuration.MaxOperationsCapacity);
        }
    }
}