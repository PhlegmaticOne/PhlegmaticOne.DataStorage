using System;
using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources;
using PhlegmaticOne.DataStorage.Infrastructure.Cancellation;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;
using PhlegmaticOne.DataStorage.Storage.Base;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker;
using PhlegmaticOne.DataStorage.Storage.Queue.Base;
using PhlegmaticOne.DataStorage.Storage.Queue.Observer;
using PhlegmaticOne.DataStorage.Storage.Queue.Operations;
using PhlegmaticOne.DataStorage.Storage.ValueSources;

namespace PhlegmaticOne.DataStorage.Storage {
    public class DataStorage : IDataStorage {
        private static readonly object Sync = new object();
        
        private readonly IDataStorageLogger _logger;
        private readonly DataSourcesSet _dataSourcesSet;
        private readonly IOperationsQueue _operationsQueue;
        private readonly ValueSourceCollection _valueSourceCollection;
        private readonly CancellationTokenSource _cancellationTokenSource;
        
        public DataStorage(
            IDataStorageLogger logger,
            DataSourcesSet dataSourcesSet,
            IOperationsQueue operationsQueue,
            CancellationTokenSource cts) {
            _valueSourceCollection = new ValueSourceCollection();
            _logger = ExceptionHelper.EnsureNotNull(logger, nameof(logger));
            _cancellationTokenSource = ExceptionHelper.EnsureNotNull(cts, nameof(cts));
            _dataSourcesSet = ExceptionHelper.EnsureNotNull(dataSourcesSet, nameof(dataSourcesSet));
            _operationsQueue = ExceptionHelper.EnsureNotNull(operationsQueue, nameof(operationsQueue));
        }

        public Task<T> ReadAsync<T>(CancellationToken ct = default) where T: class, IModel {
            try {
                using var tokenSource = _cancellationTokenSource.LinkWith(ct);
                var source = _dataSourcesSet.Source<T>();
                return source.ReadAsync(tokenSource.Token);
            }
            catch (Exception exception) {
                _logger.LogException(exception);
                return Task.FromException<T>(exception);
            }
        }

        public Task SaveAsync<T>(T value, CancellationToken ct = default) where T : class, IModel {
            try {
                using var tokenSource = _cancellationTokenSource.LinkWith(ct);
                var source = _dataSourcesSet.Source<T>();
                return source.WriteAsync(value, tokenSource.Token);
            }
            catch (Exception exception) {
                _logger.LogException(exception);
                return Task.FromException(exception);
            }
        }

        public Task DeleteAsync<T>(CancellationToken ct = default) where T: class, IModel {
            try {
                using var tokenSource = _cancellationTokenSource.LinkWith(ct);
                var source = _dataSourcesSet.Source<T>();
                return source.DeleteAsync(tokenSource.Token);
            }
            catch (Exception exception) {
                _logger.LogException(exception);
                return Task.FromException(exception);
            }
        }

        public IValueSource<T> GetOrCreateValueSource<T>() where T: class, IModel {
            if (_valueSourceCollection.TryGet<T>(out var existing)) {
                return existing;
            }
            
            var valueSource = new ValueSource<T>(this);
            _valueSourceCollection.Add(valueSource);
            return valueSource;
        }

        public IOperationsQueueObserver GetQueueObserver() => _operationsQueue;

        public void EnqueueForSaving<T>(IValueSource<T> value, CancellationToken ct = default) where T : class, IModel {
            var operation = new QueueOperationSaveState<T>(value, _logger, this);
            _operationsQueue.EnqueueOperation(operation, ct);
        }

        public void EnqueueForDeleting<T>(CancellationToken ct = default) where T : class, IModel {
            var operation = new QueueOperationDeleteState<T>(this);
            _operationsQueue.EnqueueOperation(operation, ct);
        }

        public void RequestTrackedChangesSaving() {
            lock (Sync) {
                foreach (var source in _valueSourceCollection) {
                    var valueSource = source.Value;

                    if (valueSource.HasChanges() == false) {
                        continue;
                    }
                
                    _logger.LogTrackedChanges(valueSource);
                    valueSource.EnqueueForSaving();
                }
            }
        }
    }
}