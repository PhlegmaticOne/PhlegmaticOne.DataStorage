using PhlegmaticOne.DataStorage.Configuration.Helpers;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.FileSource.Serializers {
    [CreateAssetMenu(
        menuName = "Data Storage/Storages/Files/Serializers/Xml Encryption Aes", 
        fileName = "XmlFileSerializerEncryptionAes")]
    public class DataStorageFileSerializerConfigXmlEncryptionAes : DataStorageFileSerializerConfig, IDefaultSetupConfig {
        [SerializeField] private string _privateKey;
        public override IFileSerializer CreateSerializer() => new XmlFileSerializerEncryptionAes(_privateKey);
        public void SetupDefault() => _privateKey = Constants.DefaultPrivateKey;
    }
}