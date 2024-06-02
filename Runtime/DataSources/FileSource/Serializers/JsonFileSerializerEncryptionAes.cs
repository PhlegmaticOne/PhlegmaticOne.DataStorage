using System.IO;
using System.Security.Cryptography;
using JetBrains.Annotations;
using Newtonsoft.Json;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers.Base;
using PhlegmaticOne.DataStorage.Infrastructure.Crypto;

namespace PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers
{
    public class JsonFileSerializerEncryptionAes : IFileSerializer
    {
        private readonly AesCryptoProviderWrapper _aesCryptoProviderWrapper;

        public JsonFileSerializerEncryptionAes([CanBeNull] string privateKey = null)
        {
            _aesCryptoProviderWrapper = new AesCryptoProviderWrapper(privateKey);
        }

        public string FileExtension => ".txt";

        public void Serialize<T>(Stream stream, T value)
        {
            using var provider = _aesCryptoProviderWrapper.CreateEncryptionProvider(stream);
            using var encryptor = provider.CreateEncryptor();
            using var cryptoStream = new CryptoStream(stream, encryptor, CryptoStreamMode.Write);
            using var writer = new StreamWriter(cryptoStream);
            using var jsonWriter = new JsonTextWriter(writer);

            var serializer = new JsonSerializer();
            serializer.Serialize(jsonWriter, value);
            cryptoStream.Flush();
        }

        public T Deserialize<T>(Stream stream)
        {
            using var provider = _aesCryptoProviderWrapper.CreateDecryptionProvider(stream);
            using var decryptor = provider.CreateDecryptor();
            using var cryptoStream = new CryptoStream(stream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);
            using var jsonReader = new JsonTextReader(streamReader);

            var deserializer = new JsonSerializer();
            return deserializer.Deserialize<T>(jsonReader);
        }
    }
}