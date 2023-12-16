using System.Collections.Concurrent;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Infrastructure.Dispatcher.Actions;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Infrastructure.Dispatcher
{
    public class UnityMainThreadDispatcher : MonoBehaviour, IMainThreadDispatcher
    {
        private static readonly ConcurrentQueue<IMainThreadAction> ExecutionQueue;
        private static UnityMainThreadDispatcher _instance;

        static UnityMainThreadDispatcher() => ExecutionQueue = new ConcurrentQueue<IMainThreadAction>();

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (ExecutionQueue.Count <= 0)
            {
                return;
            }

            while (ExecutionQueue.TryDequeue(out var action)) action.Execute();
        }

        public void EnqueueForExecution(IMainThreadAction action) => ExecutionQueue.Enqueue(action);

        public Task AwaitExecution(IMainThreadAction action)
        {
            var tcs = new TaskCompletionSource<bool>();
            var enqueueAction = new MainThreadExecuteAction(action, tcs);
            EnqueueForExecution(enqueueAction);
            return tcs.Task;
        }

        public Task<T> AwaitExecution<T>(IMainThreadFunc<T> func)
        {
            var tcs = new TaskCompletionSource<T>();
            var action = new MainThreadExecuteFunc<T>(func, tcs);
            EnqueueForExecution(action);
            return tcs.Task;
        }
    }
}