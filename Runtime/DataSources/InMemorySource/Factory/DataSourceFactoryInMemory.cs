using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources.Base;

namespace PhlegmaticOne.DataStorage.DataSources.InMemorySource.Factory
{
    internal sealed class DataSourceFactoryInMemory : IDataSourceFactory
    {
        public IDataSource<T> CreateDataSource<T>(DataSourceFactoryContext context) where T : class, IModel
        {
            return new InMemoryDataSource<T>();
        }
    }
}