using Common.Models;
using Common.Services;
using PhlegmaticOne.DataStorage.Provider;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultUsage {
    public class SampleDataStorageUsage : MonoBehaviour {
        [SerializeField] private DataStorageMonoProvider _dataStorageProvider;
        [SerializeField] private Button _addCoinsButton;
        [SerializeField] private Button _subtractCoinsButton;
        [SerializeField] private Text _infoText;
        [SerializeField] private int _testCoins;

        private IPlayerCurrencyService _playerCurrencyService;

        private void Awake() {
            _dataStorageProvider.Initialize();
            _addCoinsButton.onClick.AddListener(AddCoins);
            _subtractCoinsButton.onClick.AddListener(SubtractCoins);

            var playerState = _dataStorageProvider.NewValueSource<PlayerState>();
            _playerCurrencyService = new PlayerCurrencyService(playerState);
        }

        private async void Start() {
            _dataStorageProvider.StartChangeTracker();
            await _playerCurrencyService.InitializeAsync();
            UpdateInfoText();
        }
        
        private void SubtractCoins() {
            _playerCurrencyService.ChangeCurrency(-_testCoins, CurrencyType.Coins);
            UpdateInfoText();
        }

        private void AddCoins() {
            _playerCurrencyService.ChangeCurrency(_testCoins, CurrencyType.Coins);
            UpdateInfoText();
        }

        private void UpdateInfoText() {
            var coins = _playerCurrencyService.GetCurrency(CurrencyType.Coins);
            var text = $"Player has {coins} coins now!";
            _infoText.text = text;
        }
    }
}