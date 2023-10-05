﻿using System;
using PhlegmaticOne.DataStorage.Configuration.DataSources.FileSource;
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
        [SerializeField] private DataStorageInMemoryConfiguration _inMemoryConfiguration;
        [SerializeField] private DataStorageFileConfiguration _fileConfiguration;
        [SerializeField] private DataStoragePlayerPrefsConfiguration _playerPrefsConfiguration;

        public IDataSourceFactory CreateSourceFactory() {
            return _storageType switch {
                DataStorageType.InMemory => _inMemoryConfiguration.CreateSourceFactory(),
                DataStorageType.PlayerPrefs => _playerPrefsConfiguration.CreateSourceFactory(),
                DataStorageType.File => _fileConfiguration.CreateSourceFactory(),
                _ => throw new ArgumentException($"Unknown data storage type: {_storageType}", nameof(_storageType))
            };
        }

        public DataStorageConfigurationStorage Clone() {
            return new DataStorageConfigurationStorage {
                _storageType = _storageType,
                _inMemoryConfiguration = new DataStorageInMemoryConfiguration(),
                _fileConfiguration = new DataStorageFileConfiguration(_fileConfiguration),
                _playerPrefsConfiguration = new DataStoragePlayerPrefsConfiguration(_playerPrefsConfiguration)
            };
        }
    }
}