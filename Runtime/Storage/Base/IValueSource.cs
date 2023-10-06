using System.Threading;
using System.Threading.Tasks;

namespace PhlegmaticOne.DataStorage.Storage.Base {
    public interface IValueSource {
        int TrackedChanges { get; }
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public interface IValueSource<T> : IValueSource {
        T AsTrackable();
        T AsNoTrackable();
        void SetRaw(T value);
    }
}