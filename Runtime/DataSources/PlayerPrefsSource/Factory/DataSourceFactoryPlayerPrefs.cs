using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Crypto;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Serializers;

namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Factory
{
    internal sealed class DataSourceFactoryPlayerPrefs : IDataSourceFactory
    {
        private readonly IEntitySerializer _entitySerializer;
        private readonly IStringCryptoProvider _stringCryptoProvider;

        public DataSourceFactoryPlayerPrefs(
            IEntitySerializer entitySerializer,
            IStringCryptoProvider stringCryptoProvider)
        {
            _entitySerializer = entitySerializer;
            _stringCryptoProvider = stringCryptoProvider;
        }

        public IDataSource<T> CreateDataSource<T>(DataSourceFactoryContext context) where T : class, IModel
        {
            return new PlayerPrefsDataSource<T>(_entitySerializer, _stringCryptoProvider, context.MainThreadDispatcher);
        }
    }
}