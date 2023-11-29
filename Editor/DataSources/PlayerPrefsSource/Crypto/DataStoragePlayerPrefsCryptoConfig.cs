using PhlegmaticOne.DataStorage.DataSources.PlayerPrefsSource.Crypto;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.DataSources.PlayerPrefsSource.Crypto {
    public abstract class DataStoragePlayerPrefsCryptoConfig : ScriptableObject {
        public abstract IStringCryptoProvider CreateStringCryptoProvider();
    }
}