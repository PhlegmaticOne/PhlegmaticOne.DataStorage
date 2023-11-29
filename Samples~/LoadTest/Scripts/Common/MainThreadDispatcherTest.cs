using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoadTest.Common {
    public class MainThreadDispatcherTest : MonoBehaviour {
        private static readonly Queue<Action> ExecutionQueue = new Queue<Action>();
        private static MainThreadDispatcherTest _instance;

        private void Awake() {
	        if (_instance == null) {
		        _instance = this;
		        DontDestroyOnLoad(gameObject);
	        }
        }

        public void Update() {
	        lock(ExecutionQueue) {
		        while (ExecutionQueue.Count > 0) {
			        ExecutionQueue.Dequeue().Invoke();
		        }
	        }
        }
        
        private void OnDestroy() {
	        _instance = null;
        }

        public void Enqueue(Action action) {
			Enqueue(ActionWrapper(action));
		}

		private void Enqueue(IEnumerator action) {
			lock (ExecutionQueue) {
				ExecutionQueue.Enqueue (() => {
					StartCoroutine (action);
				});
			}
		}

		private static IEnumerator ActionWrapper(Action a) {
			a();
			yield return null;
		}
    }
}