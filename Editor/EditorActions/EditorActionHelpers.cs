using System.IO;
using System.Linq;
using PhlegmaticOne.DataStorage.Configuration.DataSources.FileSource;
using UnityEditor;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.EditorActions
{
    public static class EditorActionHelpers
    {
        [MenuItem("Tools/Data Storage/Clear Player Prefs Storage")]
        public static void ClearPlayerPrefsStorage()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        [MenuItem("Tools/Data Storage/Clear File Storage")]
        public static void ClearFileStorage()
        {
            var config = FindScriptableObjectOfType<DataStorageFileConfig>();

            if (config == null)
            {
                return;
            }

            var directoryPath = Path.Combine(Application.persistentDataPath, config.SavesDirectoryPath);

            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }

        [MenuItem("Tools/Data Storage/Clear All Storages")]
        public static void ClearAllStorages()
        {
            ClearFileStorage();
            ClearPlayerPrefsStorage();
        }
        
        private static T FindScriptableObjectOfType<T>(string folder = "Assets") where T : ScriptableObject
        {
            try
            {
                var filter = $"t:{typeof(T).Name}";
                var first = AssetDatabase.FindAssets(filter, new[] {folder}).First();
                var asset = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(first));
                return asset;
            }
            catch
            {
                return null;
            }
        }
    }
}