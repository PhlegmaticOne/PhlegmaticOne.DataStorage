using System;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker {
    [Serializable]
    public class ChangeTrackerConfiguration {
        [SerializeField] private int _framesInterval;
        [SerializeField] private int _delayFrames;
        public int FramesInterval => _framesInterval;
        public int DelayFrames => _delayFrames;
    }
}