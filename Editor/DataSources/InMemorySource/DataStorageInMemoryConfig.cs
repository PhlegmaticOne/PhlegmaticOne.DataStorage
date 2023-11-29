using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.InMemorySource.Factory;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.InMemorySource {
    [CreateAssetMenu(menuName = "Data Storage/Storages/In Memory", fileName = "InMemoryConfigDataStorage")]
    public class DataStorageInMemoryConfig : DataStorageConfig {
        public override IDataSourceFactory GetSourceFactory() => new DataSourceFactoryInMemory();
    }
}