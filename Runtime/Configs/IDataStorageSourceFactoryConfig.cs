using PhlegmaticOne.DataStorage.DataSources.Base;

namespace PhlegmaticOne.DataStorage.Configs {
    public interface IDataStorageSourceFactoryConfig {
        IDataSourceFactory GetSourceFactory();
    }
}