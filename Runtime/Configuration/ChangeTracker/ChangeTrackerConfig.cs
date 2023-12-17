using PhlegmaticOne.DataStorage.Configuration.Helpers;
using PhlegmaticOne.DataStorage.Provider.Configs;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.ChangeTracker
{
    [CreateAssetMenu(menuName = "Data Storage/Infrastructure/Change Tracker Config", fileName = "ChangeTrackerConfig")]
    public class ChangeTrackerConfig : ScriptableObject, IChangeTrackerConfig, IDefaultSetupConfig
    {
        [SerializeField] private ChangeTrackerConfiguration _changeTrackerConfig;
        public ChangeTrackerConfiguration GetChangeTrackerConfig() => _changeTrackerConfig;
        public void SetupDefault() => _changeTrackerConfig = new ChangeTrackerConfiguration(2, 2.5f);
    }
}