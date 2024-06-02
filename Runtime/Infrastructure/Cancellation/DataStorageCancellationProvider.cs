using System.Threading;

namespace PhlegmaticOne.DataStorage.Infrastructure.Cancellation
{
    internal sealed class DataStorageCancellationProvider : IDataStorageCancellationProvider
    {
        private readonly CancellationTokenSource _cancellationTokenSource;

        public DataStorageCancellationProvider()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public CancellationToken Token => _cancellationTokenSource.Token;

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}