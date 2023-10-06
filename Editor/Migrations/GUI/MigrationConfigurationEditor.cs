using UnityEditor;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.Migrations.GUI {
    [CustomEditor(typeof(DataStorageMigrationConfiguration))]
    public class MigrationConfigurationEditor : Editor {
        private const float Height = 25;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var configuration = (DataStorageMigrationConfiguration)target;

            if(GUILayout.Button("Clone current storage to previous", GUILayout.Height(Height))) {
                configuration.CloneCurrentStorageToPrevious();
            }
        
            if(GUILayout.Button("Increase migration version", GUILayout.Height(Height))) {
                configuration.IncreaseMigrationVersion();
            }
            
            if(GUILayout.Button("Reset version PlayerPrefs", GUILayout.Height(Height))) {
                configuration.ResetVersionPlayerPrefs();
            }
        }
    }
}