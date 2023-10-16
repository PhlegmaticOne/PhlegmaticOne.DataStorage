using System.Threading;
using System.Threading.Tasks;
using Common.Models;

namespace Common.Services {
    public interface IPlayerCurrencyService {
        Task InitializeAsync(CancellationToken cancellationToken);
        void ChangeCurrency(int delta, CurrencyType currencyType);
        int GetCurrency(CurrencyType currencyType);
    }
}