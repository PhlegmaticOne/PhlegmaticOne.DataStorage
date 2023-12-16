using System.Threading.Tasks;
using SimpleUsage.Coins.Services;
using SimpleUsage.Coins.Views;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleUsage.Controllers
{
    public class ChangePlayerCoinsController : MonoBehaviour
    {
        [SerializeField] private CoinsView _coinsView;
        [SerializeField] private Button _addCoinsButton;
        [SerializeField] private Button _subtractCoinsButton;
        [SerializeField] private int _testCoins = 42;

        private ICoinsService _coinsService;

        public void Construct(ICoinsService coinsService)
        {
            _coinsService = coinsService;

            _addCoinsButton.onClick.AddListener(AddCoins);
            _subtractCoinsButton.onClick.AddListener(SubtractCoins);
        }

        public async Task InitializeAsync()
        {
            await _coinsService.InitializeAsync();
            UpdateInfoText();
        }

        public void OnReset()
        {
            _addCoinsButton.onClick.RemoveAllListeners();
            _subtractCoinsButton.onClick.RemoveAllListeners();
        }

        private void SubtractCoins()
        {
            _coinsService.ChangeCoins(-_testCoins);
            UpdateInfoText();
        }

        private void AddCoins()
        {
            _coinsService.ChangeCoins(_testCoins);
            UpdateInfoText();
        }

        private void UpdateInfoText()
        {
            var coins = _coinsService.Coins;
            _coinsView.UpdateCoins(coins);
        }
    }
}