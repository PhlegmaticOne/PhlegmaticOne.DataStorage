using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers {
    public sealed class BinaryFileSerializer : IFileSerializer {
        public void Serialize<T>(Stream stream, T value) {
            var serializer = new BinaryFormatter();
            serializer.Serialize(stream, value);
        }

        public T Deserialize<T>(Stream stream) {
            var deserializer = new BinaryFormatter();
            return (T)deserializer.Deserialize(stream);
        }

        public string FileExtension => ".dat";
    }
}