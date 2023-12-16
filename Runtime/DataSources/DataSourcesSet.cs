using System;
using System.Collections.Generic;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;

namespace PhlegmaticOne.DataStorage.DataSources
{
    public class DataSourcesSet
    {
        private readonly IDataSourceFactory _dataSourceFactory;
        private readonly DataSourceFactoryContext _factoryContext;
        private readonly Dictionary<Type, IDataSource> _sources;

        public DataSourcesSet(IDataSourceFactory dataSourceFactory, DataSourceFactoryContext factoryContext)
        {
            _factoryContext = ExceptionHelper.EnsureNotNull(factoryContext, nameof(factoryContext));
            _dataSourceFactory = ExceptionHelper.EnsureNotNull(dataSourceFactory, nameof(dataSourceFactory));
            _sources = new Dictionary<Type, IDataSource>();
        }

        public IDataSource<T> Source<T>() where T : class, IModel
        {
            if (_sources.TryGetValue(typeof(T), out var dataSource))
            {
                return (IDataSource<T>) dataSource;
            }

            return CreateNewSource<T>();
        }

        private IDataSource<T> CreateNewSource<T>() where T : class, IModel
        {
            var dataSource = _dataSourceFactory.CreateDataSource<T>(_factoryContext);
            _sources.Add(typeof(T), dataSource);
            return dataSource;
        }
    }
}