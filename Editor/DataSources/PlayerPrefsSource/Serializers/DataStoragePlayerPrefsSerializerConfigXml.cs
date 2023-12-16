using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Serializers;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.PlayerPrefsSource.Serializers
{
    [CreateAssetMenu(menuName = "Data Storage/Storages/Player Prefs/Serializers/Xml",
        fileName = "XmlPlayerPrefsSerializer")]
    public class DataStoragePlayerPrefsSerializerConfigXml : DataStoragePlayerPrefsSerializerConfig
    {
        public override IEntitySerializer CreateSerializer() => new XmlEntitySerializer();
    }
}