using PhlegmaticOne.DataStorage.Configuration.Storage;
using PhlegmaticOne.DataStorage.Migrations;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.Migrations {
    [CreateAssetMenu(menuName = "Data Storage/Data Storage Migration Configuration", fileName = "DataStorageMigrationConfiguration")]
    public class DataStorageMigrationConfiguration : ScriptableObject, IDataStorageMigrationData {
        [Header("Configuration")]
        [SerializeField] private DataStorageConfiguration _previousStorage;
        [SerializeField] private DataStorageConfiguration _currentStorage;
        [SerializeField] private int _migrationVersion;

        public IDataStorageConfiguration PreviousStorage => _previousStorage;
        public IDataStorageConfiguration CurrentStorage => _currentStorage;
        public int MigrationVersion => _migrationVersion;

        public void CloneCurrentStorageToPrevious() => _previousStorage.CopyFrom(_currentStorage);
        
        public void IncreaseMigrationVersion() => ++_migrationVersion;

        public void ResetVersionPlayerPrefs() => new MigrationVersionProviderPlayerPrefs().ResetVersion();
    }
}