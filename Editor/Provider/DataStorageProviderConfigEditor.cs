using UnityEditor;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.Provider {
    [CustomEditor(typeof(DataStorageProviderConfig))]
    public class DataStorageProviderConfigEditor : Editor {
        private const float ButtonHeight = 25;
        
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            var script = (DataStorageProviderConfig)target;

            if (GUILayout.Button("Create and setup default configs", GUILayout.Height(ButtonHeight))) {
                script.CreateAndSetupDefaultConfigs();
            }
        }
    }
}