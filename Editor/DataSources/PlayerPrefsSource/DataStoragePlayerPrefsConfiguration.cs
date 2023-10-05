using System;
using PhlegmaticOne.DataStorage.Configuration.KeyResolvers;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Converters;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Factory;
using PhlegmaticOne.DataStorage.KeyResolvers.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.PlayerPrefsSource {
    [Serializable]
    public sealed class DataStoragePlayerPrefsConfiguration {
        [Header("Serialization")]
        [SerializeField] private DataStoragePlayerPrefsConverterType _converterType;

        [Header("Key names")] 
        [SerializeField] private DataStorageKeyResolverConfigurationBase _keyResolverConfiguration;

        public DataStoragePlayerPrefsConfiguration(DataStoragePlayerPrefsConfiguration from) {
            _converterType = from._converterType;
            _keyResolverConfiguration = from._keyResolverConfiguration;
        }
        
        public IDataSourceFactory CreateSourceFactory() {
            return new DataSourceFactoryPlayerPrefs(CreateConverter(), CreateKeyResolver());
        }

        private IEntityConverter CreateConverter() {
            return _converterType switch {
                DataStoragePlayerPrefsConverterType.Json => new JsonEntityConverter(),
                DataStoragePlayerPrefsConverterType.Xml => new XmlEntityConverter(),
                _ => throw new ArgumentException($"Unknown entity converter type: {_converterType}", nameof(_converterType))
            };
        }

        private IKeyResolver CreateKeyResolver() => _keyResolverConfiguration.CreateKeyResolver();
    }
}