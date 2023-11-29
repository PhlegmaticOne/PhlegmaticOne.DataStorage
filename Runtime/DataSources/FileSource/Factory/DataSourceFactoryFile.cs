using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Options;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers.Base;
using PhlegmaticOne.DataStorage.Infrastructure.KeyResolvers.Base;

namespace PhlegmaticOne.DataStorage.DataSources.FileSource.Factory {
    public class DataSourceFactoryFile : IDataSourceFactory {
        private readonly IFileSerializer _fileSerializer;
        private readonly IFileOptions _fileOptions;
        private readonly IKeyResolver _keyResolver;

        public DataSourceFactoryFile(IFileSerializer fileSerializer, IFileOptions fileOptions, IKeyResolver keyResolver) {
            _fileSerializer = fileSerializer;
            _fileOptions = fileOptions;
            _keyResolver = keyResolver;
        }
        
        public IDataSource<T> CreateDataSource<T>(DataSourceFactoryContext context) where T: class, IModel {
            return new FileDataSource<T>(_fileSerializer, _fileOptions, _keyResolver);
        }
    }
}