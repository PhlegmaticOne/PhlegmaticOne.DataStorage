using PhlegmaticOne.DataStorage.Configs;
using PhlegmaticOne.DataStorage.DataSources.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources {
    public abstract class DataStorageConfigBase : ScriptableObject, IDataStorageSourceFactoryConfig {
        public abstract IDataSourceFactory GetSourceFactory();
    }
}