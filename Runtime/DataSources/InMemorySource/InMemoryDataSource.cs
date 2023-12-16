using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources.Base;

namespace PhlegmaticOne.DataStorage.DataSources.InMemorySource
{
    public class InMemoryDataSource<T> : IDataSource<T> where T : class, IModel
    {
        private T _inMemoryValue;

        public Task WriteAsync(T value, CancellationToken cancellationToken = default)
        {
            _inMemoryValue = value;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(CancellationToken cancellationToken = default)
        {
            _inMemoryValue = default;
            return Task.CompletedTask;
        }

        public Task<T> ReadAsync(CancellationToken cancellationToken = default) => Task.FromResult(_inMemoryValue);
    }
}