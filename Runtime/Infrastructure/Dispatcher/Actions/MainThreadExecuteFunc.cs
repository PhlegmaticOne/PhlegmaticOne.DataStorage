using System;
using System.Threading.Tasks;

namespace PhlegmaticOne.DataStorage.Infrastructure.Dispatcher.Actions
{
    public class MainThreadExecuteFunc<T> : IMainThreadAction
    {
        private readonly IMainThreadFunc<T> _func;
        private readonly TaskCompletionSource<T> _taskCompletionSource;

        public MainThreadExecuteFunc(IMainThreadFunc<T> func, TaskCompletionSource<T> taskCompletionSource)
        {
            _func = func;
            _taskCompletionSource = taskCompletionSource;
        }

        public void Execute()
        {
            try
            {
                var result = _func.Execute();
                _taskCompletionSource.TrySetResult(result);
            }
            catch (Exception ex)
            {
                _taskCompletionSource.TrySetException(ex);
            }
        }
    }
}