using PhlegmaticOne.DataStorage.Storage.Queue.Observer;
using SimpleUsage.Common;
using TMPro;
using UnityEngine;

namespace SimpleUsage.Controllers {
    public class QueueTextLoggingController : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _operationStateText;
        [SerializeField] private TextMeshProUGUI _queueStateText;
        [SerializeField] private TextMeshProUGUI _errorText;
        
        private IOperationsQueueObserver _queueObserver;
        private MainThreadDispatcherTest _mainThreadDispatcherTest;

        public void Construct(IOperationsQueueObserver queueObserver, MainThreadDispatcherTest mainThreadDispatcherTest) {
            _mainThreadDispatcherTest = mainThreadDispatcherTest;
            _queueObserver = queueObserver;
            _queueObserver.OperationChanged += QueueObserverOnOperationChanged;
        }

        private void QueueObserverOnOperationChanged(QueueOperationState queueOperationState) {
            _mainThreadDispatcherTest.Enqueue(() => {
                if (queueOperationState.IsError) {
                    _errorText.text = queueOperationState.ErrorMessage;
                }
                else {
                    _queueStateText.text = queueOperationState.ToLogMessage();
                }

                _operationStateText.text = FormatQueueState();
            });
        }

        public void OnReset() {
            _queueObserver.OperationChanged -= QueueObserverOnOperationChanged;
        }

        private string FormatQueueState() {
            return $"Enqueued operations count: {_queueObserver.EnqueuedOperationsCount}\n" +
                   $"Capacity: {_queueObserver.OperationsCapacity}";
        }
    }
}