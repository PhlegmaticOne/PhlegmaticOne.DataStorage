using System;
using System.Threading;

namespace PhlegmaticOne.DataStorage.Infrastructure.Cancellation
{
    public class CancellationLinkEntry : IDisposable
    {
        private readonly CancellationTokenSource _linked;
        private readonly CancellationTokenSource _original;

        public CancellationLinkEntry(CancellationTokenSource original, CancellationTokenSource linked)
        {
            _original = original;
            _linked = linked;
        }

        public CancellationToken Token => _linked.Token;

        public void Dispose()
        {
            if (ReferenceEquals(_original, _linked))
            {
                return;
            }

            _linked.Dispose();
        }
    }
}