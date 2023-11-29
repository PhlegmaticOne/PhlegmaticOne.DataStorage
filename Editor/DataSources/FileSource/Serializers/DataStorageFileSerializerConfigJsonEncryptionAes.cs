using PhlegmaticOne.DataStorage.Configuration.Helpers;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.FileSource.Serializers {
    [CreateAssetMenu(
        menuName = "Data Storage/Storages/Files/Serializers/Json Encryption Aes",
        fileName = "JsonFileSerializerEncryptionAes")]
    public class DataStorageFileSerializerConfigJsonEncryptionAes : DataStorageFileSerializerConfig, IDefaultSetupConfig {
        [SerializeField] private string _privateKey;
        public override IFileSerializer CreateSerializer() => new JsonFileSerializerEncryptionAes(_privateKey);
        public void SetupDefault() => _privateKey = Constants.DefaultPrivateKey;
    }
}