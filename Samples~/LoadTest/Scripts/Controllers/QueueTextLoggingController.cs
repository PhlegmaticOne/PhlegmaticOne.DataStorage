using System;
using LoadTest.Common;
using PhlegmaticOne.DataStorage.Storage.Queue.Observer;
using TMPro;
using UnityEngine;

namespace LoadTest.Controllers {
    public class QueueTextLoggingController : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _operationStateText;
        [SerializeField] private TextMeshProUGUI _queueStateText;
        [SerializeField] private TextMeshProUGUI _errorText;
        [SerializeField] private TextMeshProUGUI _averageTime;
        
        private IOperationsQueueObserver _queueObserver;
        private MainThreadDispatcherTest _mainThreadDispatcherTest;
        private AverageTimeMeasurer _averageTimeMeasurer;

        public void Construct(IOperationsQueueObserver queueObserver, MainThreadDispatcherTest mainThreadDispatcherTest) {
            _mainThreadDispatcherTest = mainThreadDispatcherTest;
            _queueObserver = queueObserver;
            _averageTimeMeasurer = new AverageTimeMeasurer();
            _queueObserver.OperationChanged += QueueObserverOnOperationChanged;
        }

        private void QueueObserverOnOperationChanged(QueueOperationState queueOperationState) {
            var count = _queueObserver.EnqueuedOperationsCount;
            var newAverageTime = _averageTimeMeasurer.RemeasureFromNow();
            
            _mainThreadDispatcherTest.Enqueue(() => {
                if (queueOperationState.IsError) {
                    _errorText.text = queueOperationState.ErrorMessage;
                }
                else {
                    _queueStateText.text = queueOperationState.ToLogMessage();
                }

                _operationStateText.text = FormatQueueState(count);
                _averageTime.text = FormatAverageTime(newAverageTime);
            });
        }

        public void OnReset() {
            _queueObserver.OperationChanged -= QueueObserverOnOperationChanged;
        }

        private string FormatQueueState(int count) {
            return $"Enqueued operations count: {count}\n" +
                   $"Capacity: {_queueObserver.OperationsCapacity}";
        }

        private static string FormatAverageTime(TimeSpan averageTime) {
            return $"Average time: {averageTime:G}";
        }
    }
}