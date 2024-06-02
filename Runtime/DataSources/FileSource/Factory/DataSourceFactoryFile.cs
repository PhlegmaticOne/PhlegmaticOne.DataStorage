using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Options;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers.Base;

namespace PhlegmaticOne.DataStorage.DataSources.FileSource.Factory
{
    internal sealed class DataSourceFactoryFile : IDataSourceFactory
    {
        private readonly IFileOptions _fileOptions;
        private readonly IFileSerializer _fileSerializer;

        public DataSourceFactoryFile(IFileSerializer fileSerializer, IFileOptions fileOptions)
        {
            _fileSerializer = fileSerializer;
            _fileOptions = fileOptions;
        }

        public IDataSource<T> CreateDataSource<T>(DataSourceFactoryContext context) where T : class, IModel
        {
            return new FileDataSource<T>(_fileSerializer, _fileOptions);
        }
    }
}