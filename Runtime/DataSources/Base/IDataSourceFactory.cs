namespace PhlegmaticOne.DataStorage.DataSources.Base {
    public interface IDataSourceFactory {
        DataSourceBase<T> CreateDataSource<T>();
    }
}