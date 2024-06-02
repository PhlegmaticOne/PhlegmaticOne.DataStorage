using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Storage.ValueSources;
using SimpleUsage.Coins.Models;

namespace SimpleUsage.Coins.Services
{
    public class CoinsService : ICoinsService
    {
        private readonly IValueSource<CoinsState> _coinsState;

        public CoinsService(IValueSource<CoinsState> coinsState)
        {
            _coinsState = coinsState;
        }

        public int Coins => _coinsState.Value.Coins;

        public Task InitializeAsync()
        {
            return _coinsState.EnsureInitializedAsync();
        }

        public void ChangeCoins(int delta)
        {
            _coinsState.TrackableValue.Coins += delta;
        }
    }
}