using System;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources;
using PhlegmaticOne.DataStorage.Infrastructure.Cancellation;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;
using PhlegmaticOne.DataStorage.Storage.Base;
using PhlegmaticOne.DataStorage.Storage.Logger;
using PhlegmaticOne.DataStorage.Storage.Queue.Base;
using PhlegmaticOne.DataStorage.Storage.Queue.Operations;
using PhlegmaticOne.DataStorage.Storage.ValueSources;

namespace PhlegmaticOne.DataStorage.Storage
{
    internal sealed class DataStorage : IDataStorage
    {
        private readonly IDataStorageCancellationProvider _cancellationProvider;
        private readonly ValueSourceCollection _valueSourceCollection;
        private readonly IOperationsQueue _operationsQueue;
        private readonly DataSourcesSet _dataSourcesSet;
        private readonly IDataStorageLogger _logger;

        internal DataStorage(
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

        public Task<T> ReadAsync<T>(string key) where T : class, IModel
        {
            try
            {
                var source = _dataSourcesSet.Source<T>();
                return source.ReadAsync(key, _cancellationProvider.Token);
            }
            catch (Exception exception)
            {
                _logger.LogException(exception);
                return Task.FromResult<T>(default);
            }
        }

        public Task SaveAsync<T>(string key, T value) where T : class, IModel
        {
            try
            {
                var source = _dataSourcesSet.Source<T>();
                return source.WriteAsync(key, value, _cancellationProvider.Token);
            }
            catch (Exception exception)
            {
                _logger.LogException(exception);
                return Task.CompletedTask;
            }
        }

        public Task DeleteAsync<T>(string key) where T : class, IModel
        {
            try
            {
                var source = _dataSourcesSet.Source<T>();
                return source.DeleteAsync(key, _cancellationProvider.Token);
            }
            catch (Exception exception)
            {
                _logger.LogException(exception);
                return Task.CompletedTask;
            }
        }

        public IValueSource<T> GetValueSource<T>(string key) where T : class, IModel, new()
        {
            if (_valueSourceCollection.TryGet<T>(key, out var existing))
            {
                return existing;
            }

            var valueSource = new ValueSource<T>(this, key);
            _valueSourceCollection.Add(key, valueSource);
            return valueSource;
        }

        public void EnqueueSave<T>(string key, T value) where T : class, IModel
        {
            var operation = new QueueOperationSaveState<T>(key, value, _logger, this);
            _operationsQueue.EnqueueOperation(operation);
        }

        public void EnqueueDelete<T>(string key) where T : class, IModel
        {
            var operation = new QueueOperationDeleteState<T>(this, key);
            _operationsQueue.EnqueueOperation(operation);
        }

        public void RequestSaveChanges()
        {
            lock (_valueSourceCollection)
            {
                foreach (var source in _valueSourceCollection)
                {
                    var valueSource = source.Value;

                    if (!valueSource.HasChanges())
                    {
                        continue;
                    }

                    _logger.LogTrackedChanges(valueSource);
                    valueSource.EnqueueSave();
                }
            }
        }

        public void Cancel()
        {
            _cancellationProvider.Cancel();
        }
    }
}