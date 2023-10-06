using System;
using PhlegmaticOne.DataStorage.Configuration.KeyResolvers;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.FirebaseSource.Factory;
using PhlegmaticOne.DataStorage.DataSources.FirebaseSource.Options;
using PhlegmaticOne.DataStorage.KeyResolvers.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.FirebaseSource {
    [Serializable]
    public sealed class DataStorageFirebaseConfiguration {
        [Header("Key names")] 
        [SerializeField] private DataStorageKeyResolverConfigurationBase _keyResolverConfiguration;

        [Header("Options")] 
        [SerializeField] private string _testUserId;

        public DataStorageFirebaseConfiguration(DataStorageFirebaseConfiguration from) {
            _keyResolverConfiguration = from._keyResolverConfiguration;
            _testUserId = new string(from._testUserId);
        }
        
        public IDataSourceFactory CreateSourceFactory() {
            return new DataSourceFactoryFirebase(CreateKeyResolver(), CreateOptions());
        }

        private IKeyResolver CreateKeyResolver() => _keyResolverConfiguration.CreateKeyResolver();
        private FirebaseSourceOptions CreateOptions() => new(Application.isEditor, _testUserId);
    }
}