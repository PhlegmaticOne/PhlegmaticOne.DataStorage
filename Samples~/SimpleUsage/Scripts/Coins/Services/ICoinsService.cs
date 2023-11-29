using System.Threading.Tasks;

namespace SimpleUsage.Coins.Services {
    public interface ICoinsService {
        Task InitializeAsync();
        int Coins { get; }
        void ChangeCoins(int delta);
    }
}