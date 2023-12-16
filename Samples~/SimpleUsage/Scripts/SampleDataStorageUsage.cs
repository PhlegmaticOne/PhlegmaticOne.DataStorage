using PhlegmaticOne.DataStorage.Configuration.Provider;
using PhlegmaticOne.DataStorage.Provider.Base;
using SimpleUsage.Coins.Models;
using SimpleUsage.Coins.Services;
using SimpleUsage.Common;
using SimpleUsage.Controllers;
using UnityEngine;

namespace SimpleUsage
{
    public class SampleDataStorageUsage : MonoBehaviour
    {
        [SerializeField] private DataStorageProviderConfig _dataStorageProviderConfig;
        [SerializeField] private ChangePlayerCoinsController _changePlayerCoinsController;
        [SerializeField] private QueueTextLoggingController _queueTextLoggingController;
        [SerializeField] private MainThreadDispatcherTest _mainThreadDispatcher;

        private DataStorageCreationResult _creationResult;

        private void Awake()
        {
            _creationResult = _dataStorageProviderConfig.CreateDataStorageFromThisConfig();
            var dataStorage = _creationResult.DataStorage;
            var valueSource = dataStorage.GetOrCreateValueSource<CoinsState>();
            var coinsService = new CoinsService(valueSource);
            var queueObserver = dataStorage.GetQueueObserver();

            _changePlayerCoinsController.Construct(coinsService);
            _queueTextLoggingController.Construct(queueObserver, _mainThreadDispatcher);
        }

        private async void Start()
        {
            _ = _creationResult.ChangeTracker.TrackAsync();
            await _changePlayerCoinsController.InitializeAsync();
        }

        private void OnApplicationQuit()
        {
            _changePlayerCoinsController.OnReset();
            _queueTextLoggingController.OnReset();
            _creationResult.CancellationProvider.Cancel();
        }
    }
}