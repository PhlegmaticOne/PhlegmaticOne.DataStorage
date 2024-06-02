using System.Collections.Concurrent;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Infrastructure.Cancellation;
using PhlegmaticOne.DataStorage.Infrastructure.Dispatcher.Actions;

namespace PhlegmaticOne.DataStorage.Infrastructure.Dispatcher
{
    internal sealed class MainThreadDispatcher : IMainThreadDispatcher
    {
        private readonly IDataStorageCancellationProvider _cancellationProvider;
        private readonly ConcurrentQueue<IMainThreadAction> _executionQueue;

        public MainThreadDispatcher(IDataStorageCancellationProvider cancellationProvider)
        {
            _cancellationProvider = cancellationProvider;
            _executionQueue = new ConcurrentQueue<IMainThreadAction>();
        }

        public void Run()
        {
            _ = RunPrivate();
        }

        private async Task RunPrivate()
        {
            var token = _cancellationProvider.Token;
            
            while (!token.IsCancellationRequested)
            {
                await Task.Yield();
                Update();
            }
        } 
        
        private void Update()
        {
            if (_executionQueue.Count <= 0)
            {
                return;
            }

            while (_executionQueue.TryDequeue(out var action))
            {
                action.Execute();
            }
        }

        public void EnqueueForExecution(IMainThreadAction action)
        {
            _executionQueue.Enqueue(action);
        }

        public Task Await(IMainThreadAction action)
        {
            var tcs = new TaskCompletionSource<bool>();
            var enqueueAction = new MainThreadExecuteAction(action, tcs);
            EnqueueForExecution(enqueueAction);
            return tcs.Task;
        }

        public Task<T> Await<T>(IMainThreadFunc<T> func)
        {
            var tcs = new TaskCompletionSource<T>();
            var action = new MainThreadExecuteFunc<T>(func, tcs);
            EnqueueForExecution(action);
            return tcs.Task;
        }
    }
}