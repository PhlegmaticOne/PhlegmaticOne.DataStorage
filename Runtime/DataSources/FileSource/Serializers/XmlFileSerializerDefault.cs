using System.IO;
using System.Runtime.Serialization;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers.Base;

namespace PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers {
    public class XmlFileSerializerDefault : IFileSerializer {
        public string FileExtension => ".xml";

        public void Serialize<T>(Stream stream, T value) {
            var serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(stream, value);
        }

        public T Deserialize<T>(Stream stream) {
            var deserializer = new DataContractSerializer(typeof(T));
            return (T)deserializer.ReadObject(stream);
        }
    }
}