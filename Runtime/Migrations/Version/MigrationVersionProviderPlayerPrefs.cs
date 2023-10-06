using UnityEngine;

namespace PhlegmaticOne.DataStorage.Migrations {
    public class MigrationVersionProviderPlayerPrefs : IMigrationVersionProvider {
        private const string MigrationVersionKey = "_DataStorage_MigrationVersion";
        
        public int GetVersion() => PlayerPrefs.GetInt(MigrationVersionKey, 0);

        public void SetVersion(int version) => PlayerPrefs.SetInt(MigrationVersionKey, version);

        public void ResetVersion() => PlayerPrefs.DeleteKey(MigrationVersionKey);
    }
}