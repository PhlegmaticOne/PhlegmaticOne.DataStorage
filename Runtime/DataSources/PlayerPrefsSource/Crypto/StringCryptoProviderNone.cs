namespace PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Crypto
{
    public class StringCryptoProviderNone : IStringCryptoProvider
    {
        public string Encrypt(string plainText)
        {
            return plainText;
        }

        public string Decrypt(string encryptedText)
        {
            return encryptedText;
        }
    }
}