using System.Threading;

namespace PhlegmaticOne.DataStorage.Infrastructure.Cancellation
{
    public class DataStorageCancellationProvider : IDataStorageCancellationProvider
    {
        private readonly CancellationTokenSource _cancellationTokenSource;

        public DataStorageCancellationProvider() => _cancellationTokenSource = new CancellationTokenSource();

        public CancellationToken InternalToken => _cancellationTokenSource.Token;

        public CancellationLinkEntry LinkWith(CancellationToken externalToken = default)
        {
            var cts = _cancellationTokenSource;

            if (externalToken == default)
            {
                return new CancellationLinkEntry(cts, cts);
            }

            var linked = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, externalToken);
            return new CancellationLinkEntry(cts, linked);
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}