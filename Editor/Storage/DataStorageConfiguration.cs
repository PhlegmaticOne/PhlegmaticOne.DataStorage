﻿using PhlegmaticOne.DataStorage.Infrastructure;
using PhlegmaticOne.DataStorage.Infrastructure.Internet;
using PhlegmaticOne.DataStorage.Infrastructure.Internet.Base;
using PhlegmaticOne.DataStorage.Storage.Base;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.Storage {
    [CreateAssetMenu(menuName = "Data Storage/Data Storage Configuration", fileName = "DataStorageConfiguration")]
    public class DataStorageConfiguration : ScriptableObject {
        [Header("Data storage")] 
        [SerializeField] private StorageOperationType _defaultOperationType;
        [SerializeField] private DataStorageConfigurationStorage _localStorage;
        [SerializeField] private DataStorageConfigurationStorage _onlineStorage;

        [Header("Change Tracker")] 
        [SerializeField] private ChangeTrackerConfiguration _changeTrackerConfiguration;

        public DataStorageSourcesContainer GetSourceContainer() {
            return new DataStorageSourcesContainer(
                _localStorage.CreateSourceFactory(), _onlineStorage.CreateSourceFactory(),
                GetInternetProvider(), _defaultOperationType);
        }

        public ChangeTrackerConfiguration GetChangeTrackerConfiguration() {
            return _changeTrackerConfiguration;
        }

        public void CopyFrom(DataStorageConfiguration copyFrom) {
            _defaultOperationType = copyFrom._defaultOperationType;
            _localStorage = copyFrom._localStorage.Clone();
            _onlineStorage = copyFrom._onlineStorage.Clone();
        }

        private static IInternetProvider GetInternetProvider() => new InternetProvider();
    }
}