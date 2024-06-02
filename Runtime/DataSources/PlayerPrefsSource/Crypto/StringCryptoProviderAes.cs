using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using JetBrains.Annotations;
using PhlegmaticOne.DataStorage.Infrastructure.Crypto;

namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Crypto
{
    public class StringCryptoProviderAes : IStringCryptoProvider
    {
        private readonly AesCryptoProviderWrapper _aesCryptoProviderWrapper;

        public StringCryptoProviderAes([CanBeNull] string privateKey = null)
        {
            _aesCryptoProviderWrapper = new AesCryptoProviderWrapper(privateKey);
        }

        public string Encrypt(string plainText)
        {
            using var memoryStream = new MemoryStream();
            using var provider = _aesCryptoProviderWrapper.CreateEncryptionProvider(memoryStream);
            using var encryptor = provider.CreateEncryptor(provider.Key, provider.IV);
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

            var encryptionBytes = Encoding.UTF8.GetBytes(plainText);

            cryptoStream.Write(encryptionBytes, 0, encryptionBytes.Length);
            cryptoStream.FlushFinalBlock();
            
            var resultBytes = memoryStream.ToArray();
            return Convert.ToBase64String(resultBytes);
        }

        public string Decrypt(string encryptedText)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            using var memoryStream = new MemoryStream(encryptedBytes);
            using var provider = _aesCryptoProviderWrapper.CreateDecryptionProvider(memoryStream);
            using var decryptor = provider.CreateDecryptor();
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }
    }
}