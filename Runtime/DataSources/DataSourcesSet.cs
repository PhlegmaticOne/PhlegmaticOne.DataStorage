using System;
using System.Collections.Generic;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;

namespace PhlegmaticOne.DataStorage.DataSources {
    public sealed class DataSourcesSet {
        private readonly IDataSourceFactory _dataSourceFactory;
        private readonly Dictionary<Type, IDataSource> _sources;

        public DataSourcesSet(IDataSourceFactory dataSourceFactory) {
            _dataSourceFactory = ExceptionHelper.EnsureNotNull(dataSourceFactory, nameof(dataSourceFactory));
            _sources = new Dictionary<Type, IDataSource>();
        }

        public DataSourceBase<T> Source<T>() {
            if (_sources.TryGetValue(typeof(T), out var dataNode)) {
                return (DataSourceBase<T>)dataNode;
            }

            return CreateNewSource<T>();
        }

        private DataSourceBase<T> CreateNewSource<T>() {
            var node = _dataSourceFactory.CreateDataSource<T>();
            _sources.Add(typeof(T), node);
            return node;
        }
    }
}