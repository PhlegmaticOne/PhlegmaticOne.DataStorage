﻿using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Converters {
    public sealed class XmlEntityConverter : IEntityConverter {
        public string Convert<T>(T value) {
            var xmlSerializer = new DataContractSerializer(typeof(T));
            var memoryStream = new MemoryStream();
            xmlSerializer.WriteObject(memoryStream, value);
            var serialized = Encoding.ASCII.GetString(memoryStream.ToArray());
            return serialized;
        }

        public T ConvertBack<T>(string value) {
            var xmlSerializer = new DataContractSerializer(typeof(T));
            var bytes = Encoding.ASCII.GetBytes(value);
            using var memoryStream = new MemoryStream(bytes);
            return (T)xmlSerializer.ReadObject(memoryStream);
        }
    }
}