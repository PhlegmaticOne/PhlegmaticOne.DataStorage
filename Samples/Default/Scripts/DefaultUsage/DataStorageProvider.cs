using System.Threading;
using PhlegmaticOne.DataStorage.Configuration.ChangeTracker;
using PhlegmaticOne.DataStorage.Configuration.DataSources;
using PhlegmaticOne.DataStorage.DataSources;
using PhlegmaticOne.DataStorage.Storage;
using PhlegmaticOne.DataStorage.Storage.Base;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker.Base;
using UnityEngine;

namespace DefaultUsage {
    public class DataStorageProvider : MonoBehaviour {
        [SerializeField] private DataStorageConfigBase _dataStorageConfig;
        [SerializeField] private ChangeTrackerConfig _changeTrackerConfig;

        private IChangeTracker _changeTracker;
        public IDataStorage DataStorage { get; private set; }
        public CancellationTokenSource TokenSource { get; private set; }
        
        public void Initialize() {
            var set = new DataSourcesSet(_dataStorageConfig.GetSourceFactory());
            var changeTrackerConfig = _changeTrackerConfig.GetChangeTrackerConfig();
            
            var dataStorage = new DataStorage(set);
            var logger = new ChangeTrackerLoggerDebug(changeTrackerConfig);
            
            DataStorage = dataStorage;
            TokenSource = new CancellationTokenSource();
            _changeTracker = new ChangeTrackerTimeInterval(dataStorage, changeTrackerConfig, logger);
        }

        private void Start() {
            _changeTracker.TrackAsync(TokenSource.Token);
        }

        private void OnDestroy() {
            TokenSource.Cancel();
        }
    }
}