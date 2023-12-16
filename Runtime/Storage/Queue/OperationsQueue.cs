using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Infrastructure.Cancellation;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker;
using PhlegmaticOne.DataStorage.Storage.Queue.Base;
using PhlegmaticOne.DataStorage.Storage.Queue.Events;
using PhlegmaticOne.DataStorage.Storage.Queue.Observer;

namespace PhlegmaticOne.DataStorage.Storage.Queue
{
    public class OperationsQueue : IOperationsQueue
    {
        private const string CancellationSource = "OperationsQueue";

        private readonly IDataStorageCancellationProvider _cancellationProvider;
        private readonly BlockingCollection<IQueueOperation> _queueOperations;
        private readonly OperationsQueueConfiguration _configuration;
        private readonly IDataStorageLogger _logger;

        private int _isRunning;

        public OperationsQueue(IDataStorageLogger logger,
            OperationsQueueConfiguration configuration,
            IDataStorageCancellationProvider cancellationProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _cancellationProvider = cancellationProvider;
            _queueOperations = CreateOperationsQueue();
        }

        public event Action<QueueOperationState> OperationChanged;
        public int EnqueuedOperationsCount => _queueOperations.Count;
        public int OperationsCapacity => _queueOperations.BoundedCapacity;

        public void EnqueueOperation(IQueueOperation queueOperation, CancellationToken cancellationToken = default)
        {
            var tokenSource = _cancellationProvider.LinkWith(cancellationToken);

            try
            {
                _queueOperations.Add(queueOperation, tokenSource.Token);
                RaiseOperationChanged(queueOperation, QueueOperationStatus.PendingForExecution);
            }
            catch (OperationCanceledException)
            {
                _logger.LogCancellation(CancellationSource);
                RaiseOperationChanged(queueOperation, GetCancellationStatus());
            }
            catch (Exception exception)
            {
                _logger.LogException(exception);
                RaiseOperationChanged(queueOperation, QueueOperationStatus.Faulted, exception.Message);
            }
            finally
            {
                tokenSource.Dispose();
            }
        }

        public Task ExecuteOperationsAsync(CancellationToken cancellationToken = default)
        {
            if (IsRunning())
            {
                return Task.CompletedTask;
            }

            var tokenSource = _cancellationProvider.LinkWith(cancellationToken);

            return Task.Run(async () =>
            {
                var token = tokenSource.Token;

                while (token.IsCancellationRequested == false)
                {
                    IQueueOperation queueOperation = default;

                    try
                    {
                        queueOperation = _queueOperations.Take(tokenSource.Token);
                        RaiseOperationChanged(queueOperation, QueueOperationStatus.Running);
                        await queueOperation.ExecuteAsync(tokenSource.Token);
                        RaiseOperationChanged(queueOperation, QueueOperationStatus.Completed);
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogCancellation(CancellationSource);
                        RaiseOperationChanged(queueOperation, GetCancellationStatus());
                    }
                    catch (Exception exception)
                    {
                        _logger.LogException(exception);
                        RaiseOperationChanged(queueOperation, QueueOperationStatus.Faulted, exception.Message);
                    }
                    finally
                    {
                        tokenSource.Dispose();
                    }
                }
            }, tokenSource.Token);
        }

        private void RaiseOperationChanged(IQueueOperation operation, QueueOperationStatus status, string errorMessage = "")
        {
            if (OperationChanged == null)
            {
                return;
            }

            var operationMessage = operation == null ? string.Empty : operation.OperationMessage;
            var occuredAt = DateTime.UtcNow;
            var queueOperationState = new QueueOperationState(operationMessage, status, occuredAt, errorMessage);
            OperationChanged.Invoke(queueOperationState);
        }

        private QueueOperationStatus GetCancellationStatus() =>
            _cancellationProvider.InternalToken.IsCancellationRequested
                ? QueueOperationStatus.CancelledInternal
                : QueueOperationStatus.CancelledExternal;

        private BlockingCollection<IQueueOperation> CreateOperationsQueue() =>
            _configuration.IsUnlimitedCapacity
                ? new BlockingCollection<IQueueOperation>()
                : new BlockingCollection<IQueueOperation>(_configuration.MaxOperationsCapacity);

        private bool IsRunning() => Interlocked.CompareExchange(ref _isRunning, 1, 0) == 1;
    }
}