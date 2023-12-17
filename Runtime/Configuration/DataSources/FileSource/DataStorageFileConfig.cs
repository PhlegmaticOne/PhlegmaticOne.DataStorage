using PhlegmaticOne.DataStorage.Configuration.DataSources.FileSource.Serializers;
using PhlegmaticOne.DataStorage.Configuration.Helpers;
using PhlegmaticOne.DataStorage.Configuration.KeyResolvers;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Factory;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Options;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers.Base;
using PhlegmaticOne.DataStorage.Infrastructure.KeyResolvers.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.FileSource
{
    [CreateAssetMenu(menuName = "Data Storage/Storages/Files/Config", fileName = "FileConfigDataStorage")]
    public class DataStorageFileConfig : DataStorageConfig, IDefaultSetupConfig
    {
        [SerializeField] private string _savesDirectoryPath;
        [SerializeField] private DataStorageKeyResolverConfig _keyResolverConfig;
        [SerializeField] private DataStorageFileSerializerConfig _serializerConfig;

        public string SavesDirectoryPath => _savesDirectoryPath;

        public void SetupDefault() => _savesDirectoryPath = Constants.DefaultFileSavesDirectoryName;

        public override IDataSourceFactory GetSourceFactory() =>
            new DataSourceFactoryFile(CreateSerializer(), CreateOptions(), CreateKeyResolver());

        public void Setup(DataStorageKeyResolverConfig keyResolverConfig, DataStorageFileSerializerConfig serializerConfig)
        {
            _serializerConfig = serializerConfig;
            _keyResolverConfig = keyResolverConfig;
        }

        private IFileSerializer CreateSerializer() => _serializerConfig.CreateSerializer();
        private IKeyResolver CreateKeyResolver() => _keyResolverConfig.GetKeyResolver();
        private IFileOptions CreateOptions() => new FileOptionsAppPersistentPath(_savesDirectoryPath);
    }
}