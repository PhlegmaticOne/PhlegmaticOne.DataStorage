using PhlegmaticOne.DataStorage.Contracts;

namespace PhlegmaticOne.DataStorage.DataSources.Base
{
    public interface IDataSourceFactory
    {
        IDataSource<T> CreateDataSource<T>(DataSourceFactoryContext context) where T : class, IModel;
    }
}