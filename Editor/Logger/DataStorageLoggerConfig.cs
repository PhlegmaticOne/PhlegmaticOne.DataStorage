using PhlegmaticOne.DataStorage.Configuration.Helpers;
using PhlegmaticOne.DataStorage.Provider.Configs;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.Logger {
    [CreateAssetMenu(menuName = "Data Storage/Infrastructure/Logger Config", fileName = "LoggerConfig")]
    public class DataStorageLoggerConfig : ScriptableObject, IDataStorageLoggerConfig, IDefaultSetupConfig {
        [SerializeField] private bool _verbose;
        public IDataStorageLogger GetLogger() => new DataStorageLoggerDebug(_verbose);
        public void SetupDefault() => _verbose = true;
    }
}