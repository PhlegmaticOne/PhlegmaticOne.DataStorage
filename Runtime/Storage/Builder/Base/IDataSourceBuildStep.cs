using System;
using PhlegmaticOne.DataStorage.DataSources.Base;

namespace PhlegmaticOne.DataStorage.Storage.Builder.Base
{
    public interface IDataSourceBuildStep
    {
        IDataStorageBuilder UseDataSource(Func<DataStorageDataSourceBuilder, IDataSourceFactory> sourceBuilderAction);
        IDataStorageBuilder UseDataSource(IDataSourceFactory dataSourceFactory);
    }
}