using PhlegmaticOne.DataStorage.Contracts;

namespace PhlegmaticOne.DataStorage.DataSources.Base {
    public interface IDataSourceFactory {
        DataSourceBase<T> CreateDataSource<T>() where T: class, IModel;
    }
}