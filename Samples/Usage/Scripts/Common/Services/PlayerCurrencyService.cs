using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Models;
using PhlegmaticOne.DataStorage.Storage;
using PhlegmaticOne.DataStorage.Storage.Base;

namespace Common.Services {
    public class PlayerCurrencyService : IPlayerCurrencyService {
        private readonly IDataStorage _dataStorage;
        private IValueSource<PlayerState> _playerState;

        public PlayerCurrencyService(IDataStorage dataStorage) {
            _dataStorage = dataStorage;
        }
        
        public async Task InitializeAsync(CancellationToken cancellationToken) {
            _playerState = await _dataStorage.ReadAsync<PlayerState>(cancellationToken);
            
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