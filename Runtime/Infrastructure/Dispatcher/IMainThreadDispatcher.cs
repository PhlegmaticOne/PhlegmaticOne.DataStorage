using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Infrastructure.Dispatcher.Actions;

namespace PhlegmaticOne.DataStorage.Infrastructure.Dispatcher
{
    public interface IMainThreadDispatcher
    {
        void EnqueueForExecution(IMainThreadAction action);
        Task AwaitExecution(IMainThreadAction action);
        Task<T> AwaitExecution<T>(IMainThreadFunc<T> func);
    }
}