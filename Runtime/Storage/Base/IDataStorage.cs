using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.Storage.ValueSources;

namespace PhlegmaticOne.DataStorage.Storage.Base
{
    public interface IDataStorage
    {
        Task<T> ReadAsync<T>(string key) where T : class, IModel;
        Task SaveAsync<T>(string key, T value) where T : class, IModel;
        Task DeleteAsync<T>(string key) where T : class, IModel;
        void EnqueueSave<T>(string key, T value) where T : class, IModel;
        void EnqueueDelete<T>(string key) where T : class, IModel;
        void RequestSaveChanges();
        IValueSource<T> GetValueSource<T>(string key) where T : class, IModel, new();
        void Cancel();
    }
}