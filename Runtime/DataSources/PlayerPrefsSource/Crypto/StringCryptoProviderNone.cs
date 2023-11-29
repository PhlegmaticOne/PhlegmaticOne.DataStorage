namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Crypto {
    public class StringCryptoProviderNone : IStringCryptoProvider {
        public string Encrypt(string plainText) => plainText;
        public string Decrypt(string encryptedText) => encryptedText;
    }
}