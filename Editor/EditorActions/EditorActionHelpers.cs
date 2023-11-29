using System.IO;
using PhlegmaticOne.DataStorage.Configuration.DataSources.FileSource;
using PhlegmaticOne.DataStorage.Configuration.Helpers;
using UnityEditor;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.EditorActions {
    public static class EditorActionHelpers {
        [MenuItem("Tools/Data Storage/Clear Player Prefs Storage")]
        public static void ClearPlayerPrefsStorage() {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        [MenuItem("Tools/Data Storage/Clear File Storage")]
        public static void ClearFileStorage() {
            var config = AssetUtils.FindScriptableObjectOfType<DataStorageFileConfig>();

            if (config == null) {
                return;
            }
            
            var directoryPath = Path.Combine(Application.persistentDataPath, config.SavesDirectoryPath);

            if (Directory.Exists(directoryPath)) {
                Directory.Delete(directoryPath, true);
            }
        }
        
        [MenuItem("Tools/Data Storage/Clear All Storages")]
        public static void ClearAllStorages() {
            ClearFileStorage();
            ClearPlayerPrefsStorage();
        }
    }
}