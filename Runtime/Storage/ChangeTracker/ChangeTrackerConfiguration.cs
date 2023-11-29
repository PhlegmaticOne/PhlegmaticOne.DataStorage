using System;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker {
    [Serializable]
    public class ChangeTrackerConfiguration {
        [SerializeField] private float _timeInterval;
        [SerializeField] private float _timeDelay;

        public ChangeTrackerConfiguration(float timeInterval, float timeDelay) {
            _timeInterval = timeInterval;
            _timeDelay = timeDelay;
        }

        public float TimeInterval => _timeInterval;
        public float TimeDelay => _timeDelay;
    }
}