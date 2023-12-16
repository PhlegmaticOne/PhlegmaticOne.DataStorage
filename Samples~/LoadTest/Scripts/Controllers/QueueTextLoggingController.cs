using LoadTest.Common;
using PhlegmaticOne.DataStorage.Storage.Queue.Observer;
using TMPro;
using UnityEngine;

namespace LoadTest.Controllers
{
    public class QueueTextLoggingController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _operationStateText;
        [SerializeField] private TextMeshProUGUI _queueStateText;
        [SerializeField] private TextMeshProUGUI _errorText;
        private MainThreadDispatcherTest _mainThreadDispatcherTest;

        private IOperationsQueueObserver _queueObserver;

        public void Construct(IOperationsQueueObserver queueObserver, MainThreadDispatcherTest mainThreadDispatcherTest)
        {
            _mainThreadDispatcherTest = mainThreadDispatcherTest;
            _queueObserver = queueObserver;
            _queueObserver.OperationChanged += QueueObserverOnOperationChanged;
        }

        private void QueueObserverOnOperationChanged(QueueOperationState queueOperationState)
        {
            var count = _queueObserver.EnqueuedOperationsCount;

            _mainThreadDispatcherTest.Enqueue(() =>
            {
                if (queueOperationState.IsError)
                {
                    _errorText.text = queueOperationState.ErrorMessage;
                }
                else
                {
                    _queueStateText.text = queueOperationState.ToLogMessage();
                }

                _operationStateText.text = FormatQueueState(count);
            });
        }

        public void OnReset()
        {
            _queueObserver.OperationChanged -= QueueObserverOnOperationChanged;
        }

        private string FormatQueueState(int count) =>
            $"Enqueued operations count: {count}\n" +
            $"Capacity: {_queueObserver.OperationsCapacity}";
    }
}