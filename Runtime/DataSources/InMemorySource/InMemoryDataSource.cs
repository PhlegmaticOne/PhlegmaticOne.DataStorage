using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources.Base;

namespace PhlegmaticOne.DataStorage.DataSources.InMemorySource {
    internal sealed class InMemoryDataSource<T> : DataSourceBase<T> where T: class, IModel {
        private T _inMemoryValue;

        protected override Task WriteAsync(T value, CancellationToken cancellationToken = default) {
            _inMemoryValue = value;
            return Task.CompletedTask;
        }

        public override Task DeleteAsync(CancellationToken cancellationToken = default) {
            _inMemoryValue = default;
            return Task.CompletedTask;
        }

        public override Task<T> ReadAsync(CancellationToken cancellationToken = default) => Task.FromResult(_inMemoryValue);
    }
}