using PhlegmaticOne.DataStorage.Configuration.Helpers;
using PhlegmaticOne.DataStorage.Provider.Configs;
using PhlegmaticOne.DataStorage.Storage.Queue;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.OperationsQueue {
    [CreateAssetMenu(fileName = "OperationsQueueConfig", menuName = "Data Storage/Infrastructure/Operations Queue Config")]
    public class DataStorageOperationsQueueConfig : ScriptableObject, IOperationsQueueConfig, IDefaultSetupConfig {
        [SerializeField] private OperationsQueueConfiguration _configuration;
        
        public OperationsQueueConfiguration GetOperationsQueueConfig() => _configuration;

        public void SetUnlimitedCapacity() => _configuration.SetUnlimitedCapacity();
        
        public void SetupDefault() {
            _configuration = new OperationsQueueConfiguration();
            SetUnlimitedCapacity();
        }
    }
}