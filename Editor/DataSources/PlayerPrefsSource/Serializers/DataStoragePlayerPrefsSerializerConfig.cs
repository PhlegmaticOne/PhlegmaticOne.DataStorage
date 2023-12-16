using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Serializers;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.PlayerPrefsSource.Serializers
{
    public abstract class DataStoragePlayerPrefsSerializerConfig : ScriptableObject
    {
        public abstract IEntitySerializer CreateSerializer();
    }
}