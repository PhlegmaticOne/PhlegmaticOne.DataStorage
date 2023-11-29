namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Crypto {
    public interface IStringCryptoProvider {
        string Encrypt(string plainText);
        string Decrypt(string encryptedText);
    }
}