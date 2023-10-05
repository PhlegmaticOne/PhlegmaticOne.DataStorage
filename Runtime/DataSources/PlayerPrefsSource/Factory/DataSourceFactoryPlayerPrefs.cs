using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Converters;
using PhlegmaticOne.DataStorage.KeyResolvers.Base;

namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Factory {
    public sealed class DataSourceFactoryPlayerPrefs : IDataSourceFactory {
        private readonly IEntityConverter _entityConverter;
        private readonly IKeyResolver _keyResolver;

        public DataSourceFactoryPlayerPrefs(IEntityConverter entityConverter, IKeyResolver keyResolver) {
            _entityConverter = entityConverter;
            _keyResolver = keyResolver;
        }
        
        public DataSourceBase<T> CreateDataSource<T>() {
            return new PlayerPrefsDataSource<T>(_entityConverter, _keyResolver);
        }
    }
}