using System;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker {
    [Serializable]
    public class ChangeTrackerConfiguration {
        [SerializeField] private int _framesInterval;
        [SerializeField] private int _delayFrames;

        [SerializeField] private float _timeInterval;
        [SerializeField] private float _timeDelay;
        [SerializeField] private bool _isChangeTrackerVerbose;

        public ChangeTrackerConfiguration(int framesInterval, int delayFrames, bool isChangeTrackerVerbose) {
            _framesInterval = framesInterval;
            _delayFrames = delayFrames;
            _isChangeTrackerVerbose = isChangeTrackerVerbose;
        }
        
        public int FramesInterval => _framesInterval;
        public int DelayFrames => _delayFrames;
        public float TimeInterval => _timeInterval;
        public float TimeDelay => _timeDelay;
        public bool IsChangeTrackerVerbose => _isChangeTrackerVerbose;
    }
}