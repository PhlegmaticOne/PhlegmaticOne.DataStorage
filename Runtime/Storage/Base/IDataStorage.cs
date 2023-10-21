using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;

namespace PhlegmaticOne.DataStorage.Storage.Base {
    public interface IDataStorage {
        Task SaveAsync<T>(T value, CancellationToken ct = default) where T: class, IModel;
        Task<IValueSource<T>> ReadAsync<T>(CancellationToken ct = default) where T: class, IModel;
        Task DeleteAsync<T>(CancellationToken ct = default) where T: class, IModel;
    }
}