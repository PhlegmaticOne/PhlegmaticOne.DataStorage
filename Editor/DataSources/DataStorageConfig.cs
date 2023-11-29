using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.Provider.Configs;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources {
    public abstract class DataStorageConfig : ScriptableObject, IDataStorageConfig {
        public abstract IDataSourceFactory GetSourceFactory();
    }
}