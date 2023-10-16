using Newtonsoft.Json;

namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Serializers {
    public sealed class JsonEntitySerializer : IEntitySerializer {
        public string Serialize<T>(T value) {
            return JsonConvert.SerializeObject(value);
        }

        public T Deserialize<T>(string value) {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}