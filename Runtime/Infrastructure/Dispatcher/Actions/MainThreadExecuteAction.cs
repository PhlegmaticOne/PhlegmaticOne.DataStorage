using System;
using System.Threading.Tasks;

namespace PhlegmaticOne.DataStorage.Infrastructure.Dispatcher.Actions {
    public class MainThreadExecuteAction : IMainThreadAction {
        private readonly IMainThreadAction _action;
        private readonly TaskCompletionSource<bool> _taskCompletionSource;

        public MainThreadExecuteAction(IMainThreadAction action, TaskCompletionSource<bool> taskCompletionSource) {
            _action = action;
            _taskCompletionSource = taskCompletionSource;
        }
		    
        public void Execute() {
            try {
                _action.Execute();
                _taskCompletionSource.TrySetResult(true);
            } 
            catch (Exception ex) {
                _taskCompletionSource.TrySetException(ex);
            }
        }
    }
}