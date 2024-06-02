using PhlegmaticOne.DataStorage.Storage.Base;
using PhlegmaticOne.DataStorage.Storage.Builder;
using SimpleUsage.Coins.Models;
using SimpleUsage.Coins.Services;
using SimpleUsage.Controllers;
using UnityEngine;

namespace SimpleUsage
{
    public class SampleDataStorageUsage : MonoBehaviour
    {
        [SerializeField] private ChangePlayerCoinsController _changePlayerCoinsController;
        
        private IDataStorage _dataStorage;

        private void Awake()
        {
            _dataStorage = DataStorageBuilder.Create()
                .UseDataSource(x => x.PlayerPrefs())
                .UseLogger()
                .UseChangeTracker()
                .Build();
            
            var valueSource = _dataStorage.GetValueSource<CoinsState>("coins");
            var coinsService = new CoinsService(valueSource);
            _changePlayerCoinsController.Construct(coinsService);
        }

        private async void Start()
        {
            await _changePlayerCoinsController.InitializeAsync();
        }

        private void OnApplicationQuit()
        {
            _dataStorage.Cancel();
            _changePlayerCoinsController.OnReset();
        }
    }
}