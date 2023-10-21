using System.Collections.Generic;
using PhlegmaticOne.DataStorage.Configuration.ChangeTracker;
using PhlegmaticOne.DataStorage.Configuration.DataSources;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Provider {
    [CreateAssetMenu(
        menuName = "Data Storage/Data Storage Provider Config",
        fileName = "DataStorageProviderConfig")]
    public class DataStorageProviderConfig : ScriptableObject {
        [SerializeField] private ChangeTrackerConfig _changeTrackerConfig;
        [SerializeField] private List<DataStorageConfigBase> _dataStorageConfigs;

        public ChangeTrackerConfig ChangeTrackerConfig => _changeTrackerConfig;
        public DataStorageConfigBase DataStorageConfig => _dataStorageConfigs[0];
    }
}