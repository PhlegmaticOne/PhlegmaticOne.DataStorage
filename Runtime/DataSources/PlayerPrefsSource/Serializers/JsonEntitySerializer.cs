using Newtonsoft.Json;

namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Serializers
{
    public class JsonEntitySerializer : IEntitySerializer
    {
        public string Serialize<T>(T value) => JsonConvert.SerializeObject(value);
        public T Deserialize<T>(string value) => JsonConvert.DeserializeObject<T>(value);
    }
}