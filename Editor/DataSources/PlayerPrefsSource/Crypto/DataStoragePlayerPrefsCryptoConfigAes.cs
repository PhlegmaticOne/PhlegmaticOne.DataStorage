using PhlegmaticOne.DataStorage.Configuration.Helpers;
using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Crypto;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.PlayerPrefsSource.Crypto
{
    [CreateAssetMenu(
        menuName = "Data Storage/Storages/Player Prefs/Crypto/Aes",
        fileName = "PlayerPrefsCryptoConfigAes")]
    public class DataStoragePlayerPrefsCryptoConfigAes : DataStoragePlayerPrefsCryptoConfig, IDefaultSetupConfig
    {
        [SerializeField] private string _privateKey;
        public void SetupDefault() => _privateKey = Constants.DefaultPrivateKey;
        public override IStringCryptoProvider CreateStringCryptoProvider() => new StringCryptoProviderAes(_privateKey);
    }
}