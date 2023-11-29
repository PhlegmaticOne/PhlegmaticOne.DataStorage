using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;

namespace PhlegmaticOne.DataStorage.DataSources.Base {
    public interface IDataSource { }
    
    public interface IDataSource<T> : IDataSource where T: class, IModel {
        Task<T> ReadAsync(CancellationToken cancellationToken = default);
        Task WriteAsync(T value, CancellationToken cancellationToken = default);
        Task DeleteAsync(CancellationToken cancellationToken = default);
    }
}