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

namespace PhlegmaticOne.DataStorage.Storage
{
    public class DataStorage : IDataStorage
    {
        private static readonly object Sync = new object();
        
        private readonly IDataStorageCancellationProvider _cancellationProvider;
        private readonly ValueSourceCollection _valueSourceCollection;
        private readonly IOperationsQueue _operationsQueue;
        private readonly DataSourcesSet _dataSourcesSet;
        private readonly IDataStorageLogger _logger;

        public DataStorage(
            IDataStorageLogger logger,
            DataSourcesSet dataSourcesSet,
            IOperationsQueue operationsQueue,
            IDataStorageCancellationProvider cancellationProvider)
        {
            _valueSourceCollection = new ValueSourceCollection();
            _cancellationProvider = ExceptionHelper.EnsureNotNull(cancellationProvider, nameof(cancellationProvider));
            _logger = ExceptionHelper.EnsureNotNull(logger, nameof(logger));
            _dataSourcesSet = ExceptionHelper.EnsureNotNull(dataSourcesSet, nameof(dataSourcesSet));
            _operationsQueue = ExceptionHelper.EnsureNotNull(operationsQueue, nameof(operationsQueue));
        }

        public async Task<T> ReadAsync<T>(CancellationToken ct = default) where T : class, IModel
        {
            var tokenSource = _cancellationProvider.LinkWith(ct);

            try
            {
                var source = _dataSourcesSet.Source<T>();
                return await source.ReadAsync(tokenSource.Token);
            }
            catch (Exception exception)
            {
                _logger.LogException(exception);
                return await Task.FromException<T>(exception);
            }
            finally
            {
                tokenSource.Dispose();
            }
        }

        public async Task SaveAsync<T>(T value, CancellationToken ct = default) where T : class, IModel
        {
            var tokenSource = _cancellationProvider.LinkWith(ct);

            try
            {
                var source = _dataSourcesSet.Source<T>();
                await source.WriteAsync(value, tokenSource.Token);
            }
            catch (Exception exception)
            {
                _logger.LogException(exception);
                await Task.FromException(exception);
            }
            finally
            {
                tokenSource.Dispose();
            }
        }

        public async Task DeleteAsync<T>(CancellationToken ct = default) where T : class, IModel
        {
            var tokenSource = _cancellationProvider.LinkWith(ct);

            try
            {
                var source = _dataSourcesSet.Source<T>();
                await source.DeleteAsync(tokenSource.Token);
            }
            catch (Exception exception)
            {
                _logger.LogException(exception);
                await Task.FromException(exception);
            }
            finally
            {
                tokenSource.Dispose();
            }
        }

        public IValueSource<T> GetOrCreateValueSource<T>() where T : class, IModel
        {
            if (_valueSourceCollection.TryGet<T>(out var existing))
            {
                return existing;
            }

            var valueSource = new ValueSource<T>(this);
            _valueSourceCollection.Add(valueSource);
            return valueSource;
        }

        public IOperationsQueueObserver GetQueueObserver() => _operationsQueue;

        public void EnqueueForSaving<T>(IValueSource<T> value, CancellationToken ct = default) where T : class, IModel
        {
            var operation = new QueueOperationSaveState<T>(value, _logger, this);
            _operationsQueue.EnqueueOperation(operation, ct);
        }

        public void EnqueueForDeleting<T>(CancellationToken ct = default) where T : class, IModel
        {
            var operation = new QueueOperationDeleteState<T>(this);
            _operationsQueue.EnqueueOperation(operation, ct);
        }

        public void RequestTrackedChangesSaving()
        {
            lock (Sync)
            {
                foreach (var source in _valueSourceCollection)
                {
                    var valueSource = source.Value;

                    if (valueSource.HasChanges() == false)
                    {
                        continue;
                    }

                    _logger.LogTrackedChanges(valueSource);
                    valueSource.EnqueueForSaving();
                }
            }
        }
    }
}