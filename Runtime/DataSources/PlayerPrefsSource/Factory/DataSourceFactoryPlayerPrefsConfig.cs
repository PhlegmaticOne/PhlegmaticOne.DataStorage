using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Crypto;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Serializers;

namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Factory
{
    public class DataSourceFactoryPlayerPrefsConfig
    {
        public DataSourceFactoryPlayerPrefsConfig()
        {
            EntitySerializer = new JsonEntitySerializer();
            StringCryptoProvider = new StringCryptoProviderNone();
        }
        
        public IEntitySerializer EntitySerializer { get; set; }
        public IStringCryptoProvider StringCryptoProvider { get; set; }
    }
}