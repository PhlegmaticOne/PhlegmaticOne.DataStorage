﻿using System.IO;
using Newtonsoft.Json;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers.Base;

namespace PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers
{
    public class JsonFileSerializerDefault : IFileSerializer
    {
        public string FileExtension => ".json";

        public void Serialize<T>(Stream stream, T value)
        {
            using var writer = new StreamWriter(stream);
            using var jsonWriter = new JsonTextWriter(writer);
            var serializer = new JsonSerializer();
            serializer.Serialize(jsonWriter, value);
            jsonWriter.Flush();
        }

        public T Deserialize<T>(Stream stream)
        {
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);
            var deserializer = new JsonSerializer();
            return deserializer.Deserialize<T>(jsonReader);
        }
    }
}