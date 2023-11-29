using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources.Base;

namespace PhlegmaticOne.DataStorage.DataSources.InMemorySource.Factory {
    public class DataSourceFactoryInMemory : IDataSourceFactory {
        public IDataSource<T> CreateDataSource<T>(DataSourceFactoryContext context) where T: class, IModel => 
            new InMemoryDataSource<T>();
    }
}