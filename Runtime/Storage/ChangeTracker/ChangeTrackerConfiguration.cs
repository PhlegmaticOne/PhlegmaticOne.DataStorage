using System;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker {
    [Serializable]
    public class ChangeTrackerConfiguration {
        [SerializeField] private int _framesInterval;
        [SerializeField] private int _delayFrames;
        [SerializeField] private bool _isLogTrackedChangesInDebugMode;

        public ChangeTrackerConfiguration(int framesInterval, int delayFrames, bool isLogTrackedChangesInDebugMode) {
            _framesInterval = framesInterval;
            _delayFrames = delayFrames;
            _isLogTrackedChangesInDebugMode = isLogTrackedChangesInDebugMode;
        }
        
        public int FramesInterval => _framesInterval;
        public int DelayFrames => _delayFrames;
        public bool IsLogTrackedChangesInDebugMode => _isLogTrackedChangesInDebugMode;
    }
}