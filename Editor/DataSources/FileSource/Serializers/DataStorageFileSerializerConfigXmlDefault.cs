using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.FileSource.Serializers {
    [CreateAssetMenu(
        menuName = "Data Storage/Storages/Files/Serializers/Xml Default", 
        fileName = "XmlFileSerializerDefault")]
    public class DataStorageFileSerializerConfigXmlDefault : DataStorageFileSerializerConfig {
        public override IFileSerializer CreateSerializer() => new XmlFileSerializerDefault();
    }
}