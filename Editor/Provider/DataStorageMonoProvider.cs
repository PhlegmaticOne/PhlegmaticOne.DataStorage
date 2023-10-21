using System.Threading;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.Storage.Base;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Provider {
    public class DataStorageMonoProvider : MonoBehaviour {
        [SerializeField] private DataStorageProviderConfig _dataStorageProviderConfig;
        
        private bool _isCancelOnDestroy;

        public CancellationTokenSource TokenSource { get; private set; }
        public Storage.DataStorage DataStorage { get; private set; }
        public IChangeTracker ChangeTracker { get; private set; }

        public void Initialize() {
            var dataStorageConfig = _dataStorageProviderConfig.DataStorageConfig;
            var changeTrackerConfig = _dataStorageProviderConfig.ChangeTrackerConfig;
            
            TokenSource = new CancellationTokenSource();
            var token = TokenSource.Token;
            var dataStorage = Storage.DataStorage.FromConfig(dataStorageConfig).WithCancellation(token);
            var changeTracker = new ChangeTrackerTimeInterval(dataStorage, changeTrackerConfig);

            DataStorage = dataStorage;
            ChangeTracker = changeTracker;
        }

        public void StartChangeTracker(bool isCancelOnDestroy = true) {
            _isCancelOnDestroy = isCancelOnDestroy;
            ChangeTracker.TrackAsync(TokenSource.Token);
        }

        public IValueSource<T> NewValueSource<T>() where T : class, IModel {
            return new ValueSource<T>(DataStorage);
        }
        
        private void OnDestroy() => TryCancel();

        private void OnApplicationQuit() => TryCancel();
        
        private void TryCancel() {
            if (_isCancelOnDestroy && !TokenSource.IsCancellationRequested) {
                TokenSource.Cancel();
            }
        }
    }
}