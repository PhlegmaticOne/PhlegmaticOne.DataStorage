using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Infrastructure.Dispatcher.Actions;

namespace PhlegmaticOne.DataStorage.Infrastructure.Dispatcher
{
    public interface IMainThreadDispatcher
    {
        void Run();
        void EnqueueForExecution(IMainThreadAction action);
        Task Await(IMainThreadAction action);
        Task<T> Await<T>(IMainThreadFunc<T> func);
    }
}