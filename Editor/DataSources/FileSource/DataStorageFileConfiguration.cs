using System;
using PhlegmaticOne.DataStorage.Configuration.KeyResolvers;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Factory;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Options;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers;
using PhlegmaticOne.DataStorage.KeyResolvers.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.FileSource {
    [Serializable]
    public sealed class DataStorageFileConfiguration {
        [Header("Saves path")] 
        [SerializeField] private string _savesDirectoryPath;

        [Header("File names")] 
        [SerializeField] private DataStorageKeyResolverConfigurationBase _keyResolverConfiguration;

        [Header("Serialization")]
        [SerializeField] private DataStorageFileSerializationType _serializationType;

        public DataStorageFileConfiguration(DataStorageFileConfiguration from) {
            _savesDirectoryPath = string.Copy(from._savesDirectoryPath);
            _keyResolverConfiguration = from._keyResolverConfiguration;
            _serializationType = from._serializationType;
        }

        public IDataSourceFactory CreateSourceFactory() {
            return new DataSourceFactoryFile(CreateSerializer(), CreateOptions(), CreateKeyResolver());
        }

        private IFileSerializer CreateSerializer() {
            return _serializationType switch {
                DataStorageFileSerializationType.Binary => new BinaryFileSerializer(),
                DataStorageFileSerializationType.Json => new JsonFileSerializer(),
                DataStorageFileSerializationType.Xml => new XmlFileSerializer(),
                _ => throw new ArgumentException($"Unknown file serializer type: {_serializationType}", nameof(_serializationType))
            };
        }

        private IKeyResolver CreateKeyResolver() => _keyResolverConfiguration.CreateKeyResolver();

        private IFileOptions CreateOptions() => new FileOptionsAppPersistentPath(_savesDirectoryPath);
    }
}