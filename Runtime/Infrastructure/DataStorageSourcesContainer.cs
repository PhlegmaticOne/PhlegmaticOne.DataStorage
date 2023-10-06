using System;
using System.Collections.Generic;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.Infrastructure.Exceptions;
using PhlegmaticOne.DataStorage.Infrastructure.Internet.Base;
using PhlegmaticOne.DataStorage.Storage.Base;

namespace PhlegmaticOne.DataStorage.Infrastructure {
    public class DataStorageSourcesContainer {
        private readonly DataSourcesSet _localSources;
        private readonly DataSourcesSet _onlineSources;
        private readonly IInternetProvider _internetProvider;
        private readonly StorageOperationType _defaultOperationType;

        public DataStorageSourcesContainer(
            IDataSourceFactory localSourceFactory,
            IDataSourceFactory onlineSourceFactory,
            IInternetProvider internetProvider,
            StorageOperationType defaultOperationType) {
            Validate(defaultOperationType);
            _localSources = new DataSourcesSet(localSourceFactory);
            _onlineSources = new DataSourcesSet(onlineSourceFactory);
            _internetProvider = internetProvider;
            _defaultOperationType = defaultOperationType;
        }
        
        public DataSourceBase<T> GetSource<T>(StorageOperationType operationType) where T: class, IModel {
            var type = operationType == StorageOperationType.Auto ? _defaultOperationType : operationType;
            EnsureStorageAvailable(type);
            var nodesSet = GetSourceSet(type);
            return nodesSet.Source<T>();
        }

        private DataSourcesSet GetSourceSet(StorageOperationType operationType) {
            return operationType switch {
                StorageOperationType.Local => _localSources,
                StorageOperationType.Online => _onlineSources,
                _ => throw new ArgumentOutOfRangeException(nameof(operationType), "Unknown StorageOperationType", null)
            };
        }

        private void EnsureStorageAvailable(StorageOperationType operationType) {
            if (operationType == StorageOperationType.Online && _internetProvider.IsActive() == false) {
                throw new DataStorageNoInternetException();
            }
        }

        private static void Validate(StorageOperationType operationType) {
            if (operationType == StorageOperationType.Auto) {
                throw new ArgumentException("Default OperationType cannot be Auto", nameof(operationType));
            }
        }
    }
}