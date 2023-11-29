using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Serializers {
    public class XmlEntitySerializer : IEntitySerializer {
        public string Serialize<T>(T value) {
            var xmlSerializer = new DataContractSerializer(typeof(T));
            using var memoryStream = new MemoryStream();
            xmlSerializer.WriteObject(memoryStream, value);
            var serialized = Encoding.UTF8.GetString(memoryStream.ToArray());
            return serialized;
        }

        public T Deserialize<T>(string value) {
            var xmlSerializer = new DataContractSerializer(typeof(T));
            var bytes = Encoding.UTF8.GetBytes(value);
            using var memoryStream = new MemoryStream(bytes);
            return (T)xmlSerializer.ReadObject(memoryStream);
        }
    }
}