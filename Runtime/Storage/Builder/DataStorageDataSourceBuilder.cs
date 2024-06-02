using System;
using JetBrains.Annotations;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Factory;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Options;
using PhlegmaticOne.DataStorage.DataSources.InMemorySource.Factory;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Factory;

namespace PhlegmaticOne.DataStorage.Storage.Builder
{
    public class DataStorageDataSourceBuilder
    {
        public IDataSourceFactory InMemory()
        {
            return new DataSourceFactoryInMemory();
        }

        public IDataSourceFactory PlayerPrefs([CanBeNull] Action<DataSourceFactoryPlayerPrefsConfig> configAction = null)
        {
            var config = new DataSourceFactoryPlayerPrefsConfig();
            configAction?.Invoke(config);
            return new DataSourceFactoryPlayerPrefs(config.EntitySerializer, config.StringCryptoProvider);
        }
        
        public IDataSourceFactory File([CanBeNull] Action<DataSourceFactoryFileConfig> configAction = null)
        {
            var config = new DataSourceFactoryFileConfig();
            configAction?.Invoke(config);
            var options = new FileOptionsAppPersistentPath(config.DirectoryName);
            return new DataSourceFactoryFile(config.Serializer, options);
        }
    }
}