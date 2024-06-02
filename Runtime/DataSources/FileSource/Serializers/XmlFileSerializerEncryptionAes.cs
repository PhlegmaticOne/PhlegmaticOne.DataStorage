using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using JetBrains.Annotations;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers.Base;
using PhlegmaticOne.DataStorage.Infrastructure.Crypto;

namespace PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers
{
    public class XmlFileSerializerEncryptionAes : IFileSerializer
    {
        private readonly AesCryptoProviderWrapper _aesCryptoProviderWrapper;

        public XmlFileSerializerEncryptionAes([CanBeNull] string privateKey = null)
        {
            _aesCryptoProviderWrapper = new AesCryptoProviderWrapper(privateKey);
        }

        public string FileExtension => ".txt";

        public void Serialize<T>(Stream stream, T value)
        {
            using var provider = _aesCryptoProviderWrapper.CreateEncryptionProvider(stream);
            using var encryptor = provider.CreateEncryptor();
            using var cryptoStream = new CryptoStream(stream, encryptor, CryptoStreamMode.Write);
            var serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(cryptoStream, value);
            cryptoStream.Flush();
        }

        public T Deserialize<T>(Stream stream)
        {
            using var provider = _aesCryptoProviderWrapper.CreateDecryptionProvider(stream);
            using var decryptor = provider.CreateDecryptor();
            using var cryptoStream = new CryptoStream(stream, decryptor, CryptoStreamMode.Read);
            var deserializer = new DataContractSerializer(typeof(T));
            return (T) deserializer.ReadObject(cryptoStream);
        }
    }
}