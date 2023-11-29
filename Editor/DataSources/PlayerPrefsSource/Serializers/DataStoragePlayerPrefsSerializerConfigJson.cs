using PhlegmaticOne.DataStorage.Configuration.Helpers;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Serializers;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.PlayerPrefsSource.Serializers {
    [CreateAssetMenu(
        menuName = "Data Storage/Storages/Player Prefs/Serializers/Json",
        fileName = "JsonPlayerPrefsSerializer")]
    [DefaultImplementation]
    public class DataStoragePlayerPrefsSerializerConfigJson : DataStoragePlayerPrefsSerializerConfig {
        public override IEntitySerializer CreateSerializer() => new JsonEntitySerializer();
    }
}