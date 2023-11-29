using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PhlegmaticOne.DataStorage.Configuration.Helpers {
    internal static class AssetUtils {
        public static string GetAssetDirectory(Object asset) {
            return Path.GetDirectoryName(AssetDatabase.GetAssetPath(asset));
        }
        
        public static T Create<T>(string directory) where T : ScriptableObject {
            return (T)Create(typeof(T), directory);
        }

        public static string CreateDirectory(string rootPath, string folderName) {
            var resultGuid = AssetDatabase.CreateFolder(rootPath, folderName);
            return AssetDatabase.GUIDToAssetPath(resultGuid);
        }
        
        public static List<T> CreateAllInheritors<T>(string directory) where T : ScriptableObject {
            return Assembly.GetAssembly(typeof(T))
                .GetTypes()
                .Where(x => x.IsInheritorOf<T>())
                .Select(x => (T)Create(x, directory))
                .ToList();
        }

        public static T FindDefaultImplementation<T>(IList<T> assets) where T : ScriptableObject {
            var result = assets.FirstOrDefault(x => x.GetType().HasAttribute<DefaultImplementationAttribute>());
            return result == null ? assets.First() : result;
        }

        private static ScriptableObject Create(Type type, string directory) {
            var config = ScriptableObject.CreateInstance(type);
            
            if (config is IDefaultSetupConfig defaultSetupConfig) {
                defaultSetupConfig.SetupDefault();
            }

            var name = type.GetAssetFileName();
            AssetDatabase.CreateAsset(config, Path.Combine(directory, name + ".asset"));
            EditorUtility.SetDirty(config);
            return config;
        }

        public static void DeleteAllExcept(Object asset, string directory) {
            var assets = AssetDatabase.FindAssets("", new [] { directory });
            var thisPath = AssetDatabase.GetAssetPath(asset);

            foreach (var assetToDelete in assets) {
                var path = AssetDatabase.GUIDToAssetPath(assetToDelete);
                
                if (path != thisPath) {
                    AssetDatabase.DeleteAsset(path);
                }
            }
        }
        
        public static T FindScriptableObjectOfType<T>(string folder = "Assets") where T : ScriptableObject {
            try {
                var filter = $"t:{typeof(T).Name}";
                var first = AssetDatabase.FindAssets(filter, new[] {folder}).First();
                var asset = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(first));
                return asset;
            }
            catch {
                return null;
            }
        }

        public static void CommitChanges() {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}