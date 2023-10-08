using System;
using PhlegmaticOne.DataStorage.Configuration.DataSources.FileSource;
using PhlegmaticOne.DataStorage.Configuration.DataSources.FirebaseSource;
using PhlegmaticOne.DataStorage.Configuration.DataSources.InMemorySource;
using PhlegmaticOne.DataStorage.Configuration.DataSources.PlayerPrefsSource;
using PhlegmaticOne.DataStorage.DataSources.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.Storage {
    [Serializable]
    public class DataStorageConfigurationStorage {
        [Header("Data storage")]
        [SerializeField] private DataStorageType _storageType;
        
        [Header("Configurations")]
        [SerializeField] private DataStorageFileConfiguration _fileConfiguration;
        [SerializeField] private DataStoragePlayerPrefsConfiguration _playerPrefsConfiguration;
        [SerializeField] private DataStorageFirebaseConfiguration _firebaseConfiguration;

        public IDataSourceFactory CreateSourceFactory() {
            return _storageType switch {
                DataStorageType.InMemory => new DataStorageInMemoryConfiguration().CreateSourceFactory(),
                DataStorageType.PlayerPrefs => _playerPrefsConfiguration.CreateSourceFactory(),
                DataStorageType.File => _fileConfiguration.CreateSourceFactory(),
                DataStorageType.Firebase => _firebaseConfiguration.CreateSourceFactory(),
                _ => throw new ArgumentException($"Unknown data storage type: {_storageType}", nameof(_storageType))
            };
        }

        public DataStorageConfigurationStorage Clone() {
            return new DataStorageConfigurationStorage {
                _storageType = _storageType,
                _fileConfiguration = new DataStorageFileConfiguration(_fileConfiguration),
                _playerPrefsConfiguration = new DataStoragePlayerPrefsConfiguration(_playerPrefsConfiguration),
                _firebaseConfiguration = new DataStorageFirebaseConfiguration(_firebaseConfiguration)
            };
        }
    }
}