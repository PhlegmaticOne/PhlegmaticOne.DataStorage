using PhlegmaticOne.DataStorage.Configuration.DataSources.PlayerPrefsSource.Crypto;
using PhlegmaticOne.DataStorage.Configuration.DataSources.PlayerPrefsSource.Serializers;
using PhlegmaticOne.DataStorage.Configuration.Helpers;
using PhlegmaticOne.DataStorage.Configuration.KeyResolvers;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Crypto;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Factory;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Serializers;
using PhlegmaticOne.DataStorage.Infrastructure.KeyResolvers.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.PlayerPrefsSource
{
    [CreateAssetMenu(menuName = "Data Storage/Storages/Player Prefs/Config", fileName = "PlayerPrefsConfigDataStorage")]
    [DefaultImplementation]
    public class DataStoragePlayerPrefsConfig : DataStorageConfig
    {
        [SerializeField] private DataStoragePlayerPrefsSerializerConfig _serializerConfig;
        [SerializeField] private DataStorageKeyResolverConfig _keyResolverConfig;
        [SerializeField] private DataStoragePlayerPrefsCryptoConfig _cryptoConfig;

        public override IDataSourceFactory GetSourceFactory() =>
            new DataSourceFactoryPlayerPrefs(CreateSerializer(), CreateCryptoProvider(), CreateKeyResolver());

        public void Setup(DataStorageKeyResolverConfig keyResolverConfig,
            DataStoragePlayerPrefsSerializerConfig serializerConfig,
            DataStoragePlayerPrefsCryptoConfig cryptoConfig)
        {
            _keyResolverConfig = keyResolverConfig;
            _serializerConfig = serializerConfig;
            _cryptoConfig = cryptoConfig;
        }

        private IEntitySerializer CreateSerializer() => _serializerConfig.CreateSerializer();
        private IStringCryptoProvider CreateCryptoProvider() => _cryptoConfig.CreateStringCryptoProvider();
        private IKeyResolver CreateKeyResolver() => _keyResolverConfig.GetKeyResolver();
    }
}