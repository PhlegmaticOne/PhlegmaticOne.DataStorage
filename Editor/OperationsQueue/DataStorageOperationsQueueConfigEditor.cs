using UnityEditor;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.OperationsQueue {
    [CustomEditor(typeof(DataStorageOperationsQueueConfig))]
    public class DataStorageOperationsQueueConfigEditor : Editor {
        private const float ButtonHeight = 25;
        
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            var script = (DataStorageOperationsQueueConfig)target;

            if (GUILayout.Button("Set unlimited capacity", GUILayout.Height(ButtonHeight))) {
                script.SetUnlimitedCapacity();
            }
        }
    }
}