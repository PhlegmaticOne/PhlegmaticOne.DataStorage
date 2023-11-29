using TMPro;
using UnityEngine;

namespace SimpleUsage.Coins.Views {
    public class CoinsView : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _infoText;

        public void UpdateCoins(int coins) {
            var text = $"Player has {coins} coins now!";
            _infoText.text = text;
        }
    }
}