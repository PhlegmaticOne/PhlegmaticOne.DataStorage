using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Crypto;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Serializers;
using PhlegmaticOne.DataStorage.Infrastructure.KeyResolvers.Base;

namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Factory
{
    public class DataSourceFactoryPlayerPrefs : IDataSourceFactory
    {
        private readonly IEntitySerializer _entitySerializer;
        private readonly IKeyResolver _keyResolver;
        private readonly IStringCryptoProvider _stringCryptoProvider;

        public DataSourceFactoryPlayerPrefs(
            IEntitySerializer entitySerializer,
            IStringCryptoProvider stringCryptoProvider,
            IKeyResolver keyResolver)
        {
            _entitySerializer = entitySerializer;
            _stringCryptoProvider = stringCryptoProvider;
            _keyResolver = keyResolver;
        }

        public IDataSource<T> CreateDataSource<T>(DataSourceFactoryContext context) where T : class, IModel =>
            new PlayerPrefsDataSource<T>(_entitySerializer, _stringCryptoProvider, context.MainThreadDispatcher, _keyResolver);
    }
}