using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Infrastructure.Cancellation;
using PhlegmaticOne.DataStorage.Storage.Logger;
using PhlegmaticOne.DataStorage.Storage.Queue.Base;

namespace PhlegmaticOne.DataStorage.Storage.Queue
{
    internal sealed class OperationsQueue : IOperationsQueue
    {
        private const string CancellationSource = "OperationsQueue";

        private readonly IDataStorageCancellationProvider _cancellationProvider;
        private readonly BlockingCollection<IQueueOperation> _queueOperations;
        private readonly IDataStorageLogger _logger;

        private int _isRunning;

        public OperationsQueue(IDataStorageLogger logger,
            IDataStorageCancellationProvider cancellationProvider)
        {
            _logger = logger;
            _cancellationProvider = cancellationProvider;
            _queueOperations = new BlockingCollection<IQueueOperation>();
        }

        public int Count => _queueOperations.Count;

        public void EnqueueOperation(IQueueOperation queueOperation)
        {
            try
            {
                _queueOperations.Add(queueOperation, _cancellationProvider.Token);
            }
            catch (OperationCanceledException)
            {
                _logger.LogCancellation(CancellationSource);
            }
            catch (Exception exception)
            {
                _logger.LogException(exception);
            }
        }

        public void Run()
        {
            if (IsRunning())
            {
                return;
            }

            Task.Factory.StartNew(
                ExecuteActionsPrivate, _cancellationProvider.Token,
                TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private void ExecuteActionsPrivate()
        {
            var token = _cancellationProvider.Token;
                
            while (token.IsCancellationRequested == false)
            {
                try
                {
                    var queueOperation = _queueOperations.Take(token);
                    queueOperation.ExecuteAsync().Wait(token);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogCancellation(CancellationSource);
                }
                catch (Exception exception)
                {
                    _logger.LogException(exception);
                }
            }
        }

        private bool IsRunning()
        {
            return Interlocked.CompareExchange(ref _isRunning, 1, 0) == 1;
        }
    }
}