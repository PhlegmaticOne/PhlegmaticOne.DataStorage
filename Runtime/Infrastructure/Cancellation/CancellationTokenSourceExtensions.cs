using System.Threading;

namespace PhlegmaticOne.DataStorage.Infrastructure.Cancellation {
    public static class CancellationTokenSourceExtensions {
        public static CancellationLinkEntry LinkWith(this CancellationTokenSource cts, in CancellationToken external) {
            if (external == default) {
                return new CancellationLinkEntry(cts, cts);
            }
            
            var linked = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, external);
            return new CancellationLinkEntry(cts, linked);
        }
    }
}