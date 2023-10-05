using PhlegmaticOne.DataStorage.DataSources.Base;

namespace PhlegmaticOne.DataStorage.DataSources.InMemorySource.Factory {
    public sealed class DataSourceFactoryInMemory : IDataSourceFactory {
        public DataSourceBase<T> CreateDataSource<T>() => new InMemoryDataSource<T>();
    }
}