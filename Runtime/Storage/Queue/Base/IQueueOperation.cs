using System.Threading.Tasks;

namespace PhlegmaticOne.DataStorage.Storage.Queue.Base
{
    public interface IQueueOperation
    {
        Task ExecuteAsync();
    }
}