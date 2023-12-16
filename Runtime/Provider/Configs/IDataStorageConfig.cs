using PhlegmaticOne.DataStorage.DataSources.Base;

namespace PhlegmaticOne.DataStorage.Provider.Configs
{
    public interface IDataStorageConfig
    {
        IDataSourceFactory GetSourceFactory();
    }
}