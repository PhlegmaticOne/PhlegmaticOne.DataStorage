using System.Threading;
using System.Threading.Tasks;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker {
    public interface IChangeTracker {
        Task TrackAsync(CancellationToken cancellationToken = default);
    }
}