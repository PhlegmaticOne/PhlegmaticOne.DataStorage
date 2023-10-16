﻿using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;

namespace PhlegmaticOne.DataStorage.DataSources.Base {
    public abstract class DataSourceBase<T> : IDataSource where T: class, IModel {
        public Task WriteAsync(object value, CancellationToken cancellationToken = default) {
            if (value is T generic) {
                return WriteAsync(generic, cancellationToken);
            }
            
            return Task.CompletedTask;
        }

        public abstract Task DeleteAsync(CancellationToken cancellationToken = default);
        public abstract Task<T> ReadAsync(CancellationToken cancellationToken = default);
        protected abstract Task WriteAsync(T value, CancellationToken cancellationToken = default);
    }
}