using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using PhlegmaticOne.DataStorage.Contracts;

namespace PhlegmaticOne.DataStorage.Storage.ValueSources
{
    internal sealed class ValueSourceCollection : IEnumerable<KeyValuePair<string, IValueSource>>
    {
        private readonly ConcurrentDictionary<string, IValueSource> _valueSources;
        public ValueSourceCollection()
        {
            _valueSources = new ConcurrentDictionary<string, IValueSource>();
        }

        public void Add<T>(string key, IValueSource<T> valueSource) where T : IModel, new()
        {
            _valueSources.TryAdd(key, valueSource);
        }

        public bool TryGet<T>(string key, out IValueSource<T> valueSource) where T : IModel, new()
        {
            if (_valueSources.TryGetValue(key, out var result))
            {
                valueSource = (IValueSource<T>) result;
                return true;
            }

            valueSource = default;
            return false;
        }
        
        public IEnumerator<KeyValuePair<string, IValueSource>> GetEnumerator()
        {
            return _valueSources.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_valueSources).GetEnumerator();
        }
    }
}