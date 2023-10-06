using PhlegmaticOne.DataStorage.Infrastructure;

namespace PhlegmaticOne.DataStorage.Migrations {
    public interface IDataStorageConfiguration {
        DataStorageSourcesContainer GetSourceContainer();
    }
}