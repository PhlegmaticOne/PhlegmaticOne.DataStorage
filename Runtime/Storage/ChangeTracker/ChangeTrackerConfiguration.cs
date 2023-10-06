using System;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker {
    [Serializable]
    public class ChangeTrackerConfiguration {
        [SerializeField] private int _framesInterval;
        [SerializeField] private float _timeInterval;
        [SerializeField] private int _delayFrames;
        [SerializeField] private float _delayTime;
        public int FramesInterval => _framesInterval;
        public float TimeInterval => _timeInterval;
        public int DelayFrames => _delayFrames;
        public float DelayTime => _delayTime;
    }
}