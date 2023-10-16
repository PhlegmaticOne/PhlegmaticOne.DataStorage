using System;
using PhlegmaticOne.DataStorage.Configuration.KeyResolvers;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Factory;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Serializers;
using PhlegmaticOne.DataStorage.KeyResolvers.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.PlayerPrefsSource {
    [CreateAssetMenu(menuName = "Data Storage/Storages/Player Prefs", fileName = "PlayerPrefsConfigDataStorage")]
    public sealed class DataStoragePlayerPrefsConfig : DataStorageConfigBase {
        [Header("Serialization")]
        [SerializeField] private DataStoragePlayerPrefsSerializerType _serializerType;

        [Header("Key names")] 
        [SerializeField] private DataStorageKeyResolverConfigurationBase _keyResolverConfiguration;

        public override IDataSourceFactory GetSourceFactory() {
            return new DataSourceFactoryPlayerPrefs(CreateConverter(), CreateKeyResolver());
        }

        private IEntitySerializer CreateConverter() {
            return _serializerType switch {
                DataStoragePlayerPrefsSerializerType.Json => new JsonEntitySerializer(),
                DataStoragePlayerPrefsSerializerType.Xml => new XmlEntitySerializer(),
                _ => throw new ArgumentException($"Unknown entity converter type: {_serializerType}", nameof(_serializerType))
            };
        }

        private IKeyResolver CreateKeyResolver() => _keyResolverConfiguration.GetKeyResolver();
    }
}