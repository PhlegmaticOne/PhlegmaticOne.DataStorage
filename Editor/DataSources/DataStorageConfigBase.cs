using PhlegmaticOne.DataStorage.DataSources.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources {
    public abstract class DataStorageConfigBase : ScriptableObject {
        public abstract IDataSourceFactory GetSourceFactory();
    }
}