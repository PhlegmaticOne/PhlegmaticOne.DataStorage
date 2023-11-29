using PhlegmaticOne.DataStorage.Configuration.Helpers;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.FileSource.Serializers {
    [CreateAssetMenu(
        menuName = "Data Storage/Storages/Files/Serializers/Json Default", 
        fileName = "JsonFileSerializerDefault")]
    [DefaultImplementation]
    public class DataStorageFileSerializerConfigJsonDefault : DataStorageFileSerializerConfig {
        public override IFileSerializer CreateSerializer() => new JsonFileSerializerDefault();
    }
}