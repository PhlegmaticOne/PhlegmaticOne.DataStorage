using System;
using System.Threading.Tasks;
using Common.Models;
using PhlegmaticOne.DataStorage.Storage;
using PhlegmaticOne.DataStorage.Storage.Base;

namespace Common.Services {
    public class PlayerCurrencyService : IPlayerCurrencyService {
        private readonly IValueSource<PlayerState> _playerState;

        public PlayerCurrencyService(IValueSource<PlayerState> playerState) {
            _playerState = playerState;
        }
        
        public async Task InitializeAsync() {
            await _playerState.InitializeAsync();
            
            if (_playerState.NoValue()) {
                _playerState.SetRaw(PlayerState.Initial);  
            }
        }

        public int GetCurrency(CurrencyType currencyType) {
            return currencyType switch {
                CurrencyType.Coins => _playerState.AsNoTrackable().Coins,
                CurrencyType.Gems => _playerState.AsNoTrackable().Gems,
                _ => throw new ArgumentOutOfRangeException(nameof(currencyType), currencyType, null)
            };
        }

        public void ChangeCurrency(int delta, CurrencyType currencyType) {
            switch (currencyType) {
                case CurrencyType.Coins:
                    _playerState.AsTrackable().ChangeCoins(delta);
                    break;
                case CurrencyType.Gems:
                    _playerState.AsTrackable().ChangeGems(delta);
                    break;
            }
        }
    }
}