using PhlegmaticOne.DataStorage.Storage.ChangeTracker;

namespace PhlegmaticOne.DataStorage.Provider.Configs
{
    public interface IChangeTrackerConfig
    {
        ChangeTrackerConfiguration GetChangeTrackerConfig();
    }
}