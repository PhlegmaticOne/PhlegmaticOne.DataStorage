using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;

namespace PhlegmaticOne.DataStorage.Storage.ValueSources
{
    public interface IValueSource
    {
        string Key { get; }
        int TrackedChanges { get; }
        Task InitializeAsync();
        void EnqueueSave();
        void EnqueueDelete();
    }

    public interface IValueSource<T> : IValueSource where T : IModel, new()
    {
        T TrackableValue { get; }
        T Value { get; }
        void SetRawValue(T value);
    }
}