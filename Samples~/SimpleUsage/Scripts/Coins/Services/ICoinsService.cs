using System.Threading.Tasks;

namespace SimpleUsage.Coins.Services
{
    public interface ICoinsService
    {
        int Coins { get; }
        Task InitializeAsync();
        void ChangeCoins(int delta);
    }
}