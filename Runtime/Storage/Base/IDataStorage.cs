using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.Storage.Queue.Observer;
using PhlegmaticOne.DataStorage.Storage.ValueSources;

namespace PhlegmaticOne.DataStorage.Storage.Base
{
    public interface IDataStorage
    {
        Task<T> ReadAsync<T>(CancellationToken ct = default) where T : class, IModel;
        Task SaveAsync<T>(T value, CancellationToken ct = default) where T : class, IModel;
        Task DeleteAsync<T>(CancellationToken ct = default) where T : class, IModel;
        IValueSource<T> GetOrCreateValueSource<T>() where T : class, IModel;
        void EnqueueForSaving<T>(IValueSource<T> value, CancellationToken ct = default) where T : class, IModel;
        void EnqueueForDeleting<T>(CancellationToken ct = default) where T : class, IModel;
        void RequestTrackedChangesSaving();
        IOperationsQueueObserver GetQueueObserver();
    }
}