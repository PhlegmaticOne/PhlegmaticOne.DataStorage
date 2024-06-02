using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources.Base;

namespace PhlegmaticOne.DataStorage.DataSources.InMemorySource
{
    internal sealed class InMemoryDataSource<T> : IDataSource<T> where T : class, IModel
    {
        private readonly Dictionary<string, T> _values;

        public InMemoryDataSource()
        {
            _values = new Dictionary<string, T>();
        }

        public Task WriteAsync(string key, T value, CancellationToken cancellationToken = default)
        {
            _values[key] = value;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            _values.Remove(key);
            return Task.CompletedTask;
        }

        public Task<T> ReadAsync(string key, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_values.GetValueOrDefault(key));
        }
    }
}