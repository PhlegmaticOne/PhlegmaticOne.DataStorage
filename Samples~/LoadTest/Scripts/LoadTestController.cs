using LoadTest.Common;
using LoadTest.Controllers;
using PhlegmaticOne.DataStorage.Configuration.Provider;
using PhlegmaticOne.DataStorage.Provider.Base;
using UnityEngine;

namespace LoadTest {
    public class LoadTestController : MonoBehaviour {
        [SerializeField] private DataStorageProviderConfig _dataStorageProviderConfig;
        [SerializeField] private QueueTextLoggingController _queueTextLoggingController;
        [SerializeField] private ProcessorsDataController _processorsDataController;
        [SerializeField] private MainThreadDispatcherTest _mainThreadDispatcher;
        
        private DataStorageCreationResult _creationResult;

        private void Awake() {
            _creationResult = _dataStorageProviderConfig.CreateDataStorageFromThisConfig();
            var dataStorage = _creationResult.DataStorage;
            var queueObserver = dataStorage.GetQueueObserver();

            _queueTextLoggingController.Construct(queueObserver, _mainThreadDispatcher);
            _processorsDataController.Construct(dataStorage, _creationResult.ChangeTracker);
        }

        private async void Start() {
            _ = _creationResult.ChangeTracker.TrackAsync();
            await _processorsDataController.InitializeAsync();
        }

        private void OnApplicationQuit() {
            _queueTextLoggingController.OnReset();
            _creationResult.CancellationProvider.Cancel();
        }        
    }
}