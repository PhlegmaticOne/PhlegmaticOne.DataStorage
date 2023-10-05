using System;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.InMemorySource.Factory;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.InMemorySource {
    [Serializable]
    public sealed class DataStorageInMemoryConfiguration {
        public IDataSourceFactory CreateSourceFactory() => new DataSourceFactoryInMemory();
    }
}