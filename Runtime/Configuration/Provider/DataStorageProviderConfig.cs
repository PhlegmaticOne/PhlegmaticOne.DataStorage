using System.Collections.Generic;
using PhlegmaticOne.DataStorage.Configuration.ChangeTracker;
using PhlegmaticOne.DataStorage.Configuration.DataSources;
using PhlegmaticOne.DataStorage.Configuration.DataSources.FileSource;
using PhlegmaticOne.DataStorage.Configuration.DataSources.FileSource.Serializers;
using PhlegmaticOne.DataStorage.Configuration.DataSources.InMemorySource;
using PhlegmaticOne.DataStorage.Configuration.DataSources.PlayerPrefsSource;
using PhlegmaticOne.DataStorage.Configuration.DataSources.PlayerPrefsSource.Crypto;
using PhlegmaticOne.DataStorage.Configuration.DataSources.PlayerPrefsSource.Serializers;
using PhlegmaticOne.DataStorage.Configuration.Helpers;
using PhlegmaticOne.DataStorage.Configuration.KeyResolvers;
using PhlegmaticOne.DataStorage.Configuration.Logger;
using PhlegmaticOne.DataStorage.Configuration.OperationsQueue;
using PhlegmaticOne.DataStorage.Provider;
using PhlegmaticOne.DataStorage.Provider.Base;
using PhlegmaticOne.DataStorage.Provider.Configs;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.Provider
{
    [CreateAssetMenu(
        menuName = "Data Storage/Data Storage Provider Config",
        fileName = "DataStorageProviderConfig")]
    public class DataStorageProviderConfig : ScriptableObject, IDataStorageProviderConfig
    {
        [SerializeField] private DataStorageLoggerConfig _loggerConfig;
        [SerializeField] private ChangeTrackerConfig _changeTrackerConfig;
        [SerializeField] private DataStorageConfig _dataStorageConfig;
        [SerializeField] private DataStorageOperationsQueueConfig _operationsQueueConfig;

        public IChangeTrackerConfig ChangeTrackerConfig => _changeTrackerConfig;
        public IDataStorageLoggerConfig LoggerConfig => _loggerConfig;
        public IDataStorageConfig DataStorageConfig => _dataStorageConfig;
        public IOperationsQueueConfig OperationsQueueConfig => _operationsQueueConfig;

        public DataStorageCreationResult CreateDataStorageFromThisConfig() =>
            DataStorageProvider.CreateDataStorage(this);

        public void CreateAndSetupDefaultConfigs()
        {
            var rootDirectory = AssetUtils.GetAssetDirectory(this);
            ResetState(rootDirectory);

            var infrastructureDirectory = AssetUtils.CreateDirectory(rootDirectory, DirectoryNames.Infrastructure);
            var changeTrackerConfig = AssetUtils.Create<ChangeTrackerConfig>(infrastructureDirectory);
            var loggerConfig = AssetUtils.Create<DataStorageLoggerConfig>(infrastructureDirectory);
            var keyResolverConfig = AssetUtils.Create<DataStorageTypeKeyResolverConfig>(infrastructureDirectory);
            var operationsQueueConfig = AssetUtils.Create<DataStorageOperationsQueueConfig>(infrastructureDirectory);
            var storagesDirectory = AssetUtils.CreateDirectory(rootDirectory, DirectoryNames.Storages);

            var storageConfigs = new List<DataStorageConfig>
            {
                CreateInMemoryConfig(storagesDirectory),
                CreatePlayerPrefsConfig(storagesDirectory, keyResolverConfig),
                CreateFileConfig(storagesDirectory, keyResolverConfig)
            };

            var defaultStorageConfig = AssetUtils.FindDefaultImplementation(storageConfigs);

            _loggerConfig = loggerConfig;
            _changeTrackerConfig = changeTrackerConfig;
            _dataStorageConfig = defaultStorageConfig;
            _operationsQueueConfig = operationsQueueConfig;

            AssetUtils.CommitChanges();
        }

        private void ResetState(string rootDirectory)
        {
            _loggerConfig = null;
            _changeTrackerConfig = null;
            _loggerConfig = null;
            AssetUtils.DeleteAllExcept(this, rootDirectory);
        }

        private static DataStorageConfig CreatePlayerPrefsConfig(string storagesDirectory,
            DataStorageKeyResolverConfig keyResolverConfig)
        {
            var directory = AssetUtils.CreateDirectory(storagesDirectory, DirectoryNames.PlayerPrefs);
            var serializersDirectory = AssetUtils.CreateDirectory(directory, DirectoryNames.Serializers);
            var cryptoDirectory = AssetUtils.CreateDirectory(directory, DirectoryNames.Crypto);
            var config = AssetUtils.Create<DataStoragePlayerPrefsConfig>(directory);
            var serializerConfigs =
                AssetUtils.CreateAllInheritors<DataStoragePlayerPrefsSerializerConfig>(serializersDirectory);
            var defaultSerializer = AssetUtils.FindDefaultImplementation(serializerConfigs);
            var cryptoConfigs = AssetUtils.CreateAllInheritors<DataStoragePlayerPrefsCryptoConfig>(cryptoDirectory);
            var defaultCrypto = AssetUtils.FindDefaultImplementation(cryptoConfigs);
            config.Setup(keyResolverConfig, defaultSerializer, defaultCrypto);
            return config;
        }

        private static DataStorageConfig CreateFileConfig(string storagesDirectory,
            DataStorageKeyResolverConfig keyResolverConfig)
        {
            var directory = AssetUtils.CreateDirectory(storagesDirectory, DirectoryNames.Files);
            var serializersDirectory = AssetUtils.CreateDirectory(directory, DirectoryNames.Serializers);
            var config = AssetUtils.Create<DataStorageFileConfig>(directory);
            var serializerConfigs =
                AssetUtils.CreateAllInheritors<DataStorageFileSerializerConfig>(serializersDirectory);
            var defaultSerializer = AssetUtils.FindDefaultImplementation(serializerConfigs);
            config.Setup(keyResolverConfig, defaultSerializer);
            return config;
        }

        private static DataStorageConfig CreateInMemoryConfig(string storagesDirectory)
        {
            var inMemoryDirectory = AssetUtils.CreateDirectory(storagesDirectory, DirectoryNames.InMemory);
            var inMemoryConfig = AssetUtils.Create<DataStorageInMemoryConfig>(inMemoryDirectory);
            return inMemoryConfig;
        }
    }
}