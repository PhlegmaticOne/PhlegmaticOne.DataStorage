using PhlegmaticOne.DataStorage.Configs;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.ChangeTracker {
    [CreateAssetMenu(menuName = "Data Storage/Change Tracker Config", fileName = "ChangeTrackerConfig")]
    public class ChangeTrackerConfig : ScriptableObject, IChangeTrackerConfig {
        [SerializeField] private ChangeTrackerConfiguration _changeTrackerConfig;
        public ChangeTrackerConfiguration GetChangeTrackerConfig() => _changeTrackerConfig;
        public IChangeTrackerLogger GetLogger() => new ChangeTrackerLoggerDebug(_changeTrackerConfig);
    }
}