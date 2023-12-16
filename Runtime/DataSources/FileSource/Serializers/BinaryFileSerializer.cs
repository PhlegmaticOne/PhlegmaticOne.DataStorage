using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers.Base;

namespace PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers
{
    public class BinaryFileSerializer : IFileSerializer
    {
        public string FileExtension => ".dat";

        public void Serialize<T>(Stream stream, T value)
        {
            var serializer = new BinaryFormatter();
            serializer.Serialize(stream, value);
        }

        public T Deserialize<T>(Stream stream)
        {
            var deserializer = new BinaryFormatter();
            return (T) deserializer.Deserialize(stream);
        }
    }
}