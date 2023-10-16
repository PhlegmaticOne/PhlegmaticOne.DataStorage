using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Serializers;
using PhlegmaticOne.DataStorage.KeyResolvers.Base;

namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Factory {
    public sealed class DataSourceFactoryPlayerPrefs : IDataSourceFactory {
        private readonly IEntitySerializer _entitySerializer;
        private readonly IKeyResolver _keyResolver;

        public DataSourceFactoryPlayerPrefs(IEntitySerializer entitySerializer, IKeyResolver keyResolver) {
            _entitySerializer = entitySerializer;
            _keyResolver = keyResolver;
        }
        
        public DataSourceBase<T> CreateDataSource<T>() where T: class, IModel {
            return new PlayerPrefsDataSource<T>(_entitySerializer, _keyResolver);
        }
    }
}