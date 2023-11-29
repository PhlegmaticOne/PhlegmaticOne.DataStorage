using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.FileSource.Serializers {
    public abstract class DataStorageFileSerializerConfig : ScriptableObject {
        public abstract IFileSerializer CreateSerializer();
    }
}