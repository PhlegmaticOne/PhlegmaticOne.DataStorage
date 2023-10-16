using System;
using PhlegmaticOne.DataStorage.Configuration.KeyResolvers;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Factory;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Options;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers;
using PhlegmaticOne.DataStorage.KeyResolvers.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.FileSource {
    [CreateAssetMenu(menuName = "Data Storage/Storages/Files", fileName = "FileConfigDataStorage")]
    public sealed class DataStorageFileConfig : DataStorageConfigBase {
        [Header("Saves path")] 
        [SerializeField] private string _savesDirectoryPath;

        [Header("File names")] 
        [SerializeField] private DataStorageKeyResolverConfigurationBase _keyResolverConfiguration;

        [Header("Serialization")]
        [SerializeField] private DataStorageFileSerializerType _serializerType;

        public override IDataSourceFactory GetSourceFactory() {
            return new DataSourceFactoryFile(CreateSerializer(), CreateOptions(), CreateKeyResolver());
        }

        private IFileSerializer CreateSerializer() {
            return _serializerType switch {
                DataStorageFileSerializerType.Binary => new BinaryFileSerializer(),
                DataStorageFileSerializerType.Json => new JsonFileSerializer(),
                DataStorageFileSerializerType.Xml => new XmlFileSerializer(),
                _ => throw new ArgumentException($"Unknown file serializer type: {_serializerType}", nameof(_serializerType))
            };
        }

        private IKeyResolver CreateKeyResolver() => _keyResolverConfiguration.GetKeyResolver();

        private IFileOptions CreateOptions() => new FileOptionsAppPersistentPath(_savesDirectoryPath);
    }
}