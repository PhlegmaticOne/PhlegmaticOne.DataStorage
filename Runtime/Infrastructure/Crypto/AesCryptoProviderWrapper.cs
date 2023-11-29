﻿using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PhlegmaticOne.DataStorage.Infrastructure.Crypto {
    public class AesCryptoProviderWrapper {
        private const int AesKeyLength = 16;
        private const string DefaultPrivateKey = "super-secret-key";
        
        private readonly byte[] _privateKeyData;

        public AesCryptoProviderWrapper(string privateKey) {
            _privateKeyData = Encoding.UTF8.GetBytes(ProcessPrivateKey(privateKey));
        }

        public AesCryptoServiceProvider CreateProviderForDecryption(Stream stream) {
            var aes = new AesCryptoServiceProvider {
                Key = _privateKeyData, 
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7, 
                IV = ReadIV(stream)
            };

            return aes;
        }
        
        public AesCryptoServiceProvider CreateProviderForEncryption(Stream stream) {
            var aes = new AesCryptoServiceProvider {
                Key = _privateKeyData, 
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7, 
            };

            aes.GenerateIV();
            stream.Write(aes.IV, 0, AesKeyLength);
            return aes;
        }

        private static byte[] ReadIV(Stream stream) {
            var iv = new byte[AesKeyLength];
            stream.Read(iv, 0, AesKeyLength);
            stream.Position = iv.Length;
            return iv;
        }

        private static string ProcessPrivateKey(string privateKey) {
            if (string.IsNullOrWhiteSpace(privateKey)) {
                privateKey = DefaultPrivateKey;
            }
            
            return privateKey.Length > AesKeyLength ? privateKey.Substring(0, AesKeyLength) : privateKey;
        }
    }
}