using PhlegmaticOne.DataStorage.Storage.ChangeTracker;

namespace PhlegmaticOne.DataStorage.Configs {
    public interface IChangeTrackerConfig {
        ChangeTrackerConfiguration GetChangeTrackerConfig();
        IChangeTrackerLogger GetLogger();
    }
}