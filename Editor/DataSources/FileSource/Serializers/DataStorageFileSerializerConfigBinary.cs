using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.FileSource.Serializers {
    [CreateAssetMenu(
        menuName = "Data Storage/Storages/Files/Serializers/Binary", 
        fileName = "BinaryFileSerializer")]
    public class DataStorageFileSerializerConfigBinary : DataStorageFileSerializerConfig {
        public override IFileSerializer CreateSerializer() => new BinaryFileSerializer();
    }
}