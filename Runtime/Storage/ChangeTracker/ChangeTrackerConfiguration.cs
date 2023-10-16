using System;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker {
    [Serializable]
    public class ChangeTrackerConfiguration {
        [SerializeField] private float _timeInterval;
        [SerializeField] private float _timeDelay;
        [SerializeField] private bool _isChangeTrackerVerbose;

        public ChangeTrackerConfiguration(float timeInterval, float timeDelay, bool isChangeTrackerVerbose) {
            _timeInterval = timeInterval;
            _timeDelay = timeDelay;
            _isChangeTrackerVerbose = isChangeTrackerVerbose;
        }
        
        public float TimeInterval => _timeInterval;
        public float TimeDelay => _timeDelay;
        public bool IsChangeTrackerVerbose => _isChangeTrackerVerbose;
    }
}