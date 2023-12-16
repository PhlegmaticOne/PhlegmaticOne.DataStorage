using System.Threading;
using System.Threading.Tasks;

namespace PhlegmaticOne.DataStorage.Storage.ValueSources
{
    public interface IValueSource
    {
        int TrackedChanges { get; }
        string DisplayName { get; }
        Task InitializeAsync(CancellationToken cancellationToken = default);
        void EnqueueForSaving(CancellationToken cancellationToken = default);
        void EnqueueForDeleting(CancellationToken cancellationToken = default);
    }

    public interface IValueSource<T> : IValueSource
    {
        T TrackableValue { get; }
        T Value { get; }
        void SetRawValue(T value);
    }
}