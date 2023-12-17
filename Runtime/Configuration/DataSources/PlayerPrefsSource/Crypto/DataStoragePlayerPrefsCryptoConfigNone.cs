using PhlegmaticOne.DataStorage.Configuration.Helpers;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Crypto;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.PlayerPrefsSource.Crypto
{
    [CreateAssetMenu(
        menuName = "Data Storage/Storages/Player Prefs/Crypto/None",
        fileName = "PlayerPrefsCryptoConfigNone")]
    [DefaultImplementation]
    public class DataStoragePlayerPrefsCryptoConfigNone : DataStoragePlayerPrefsCryptoConfig
    {
        public override IStringCryptoProvider CreateStringCryptoProvider() => new StringCryptoProviderNone();
    }
}