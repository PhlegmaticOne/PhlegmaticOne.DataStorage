using System.Threading;

namespace PhlegmaticOne.DataStorage.Infrastructure.Cancellation {
    public interface IDataStorageCancellationProvider {
        CancellationToken InternalToken { get; }
        CancellationLinkEntry LinkWith(CancellationToken internalToken = default);
        void Cancel();
    }
}