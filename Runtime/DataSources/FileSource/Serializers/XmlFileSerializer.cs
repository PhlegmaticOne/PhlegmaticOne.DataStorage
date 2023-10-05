using System.IO;
using System.Runtime.Serialization;

namespace PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers {
    public sealed class XmlFileSerializer : IFileSerializer {
        public void Serialize<T>(Stream stream, T value) {
            var serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(stream, value);
        }

        public T Deserialize<T>(Stream stream) {
            var deserializer = new DataContractSerializer(typeof(T));
            return (T)deserializer.ReadObject(stream);
        }

        public string FileExtension => ".xml";
    }
}