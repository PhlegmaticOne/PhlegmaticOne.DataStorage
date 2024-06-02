using System.Threading;

namespace PhlegmaticOne.DataStorage.Infrastructure.Cancellation
{
    public interface IDataStorageCancellationProvider
    {
        CancellationToken Token { get; }
        void Cancel();
    }
}