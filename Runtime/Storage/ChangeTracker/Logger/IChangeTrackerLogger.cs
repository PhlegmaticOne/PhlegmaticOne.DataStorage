using PhlegmaticOne.DataStorage.Storage.Base;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker {
    public interface IChangeTrackerLogger {
        void LogError(string message);
        void LogCancellation();
        void LogTrackedChanges(IValueSource tracker);
    }
}