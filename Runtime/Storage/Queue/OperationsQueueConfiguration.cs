using System;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Storage.Queue {
    [Serializable]
    public class OperationsQueueConfiguration {
        [SerializeField] private int _maxOperationsCapacity;
        public bool IsUnlimitedCapacity => _maxOperationsCapacity == -1;
        public int MaxOperationsCapacity => _maxOperationsCapacity;
        public void SetUnlimitedCapacity() => _maxOperationsCapacity = -1;
    }
}