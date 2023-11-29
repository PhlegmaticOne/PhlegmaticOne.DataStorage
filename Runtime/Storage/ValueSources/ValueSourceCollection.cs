using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace PhlegmaticOne.DataStorage.Storage.ValueSources {
    public class ValueSourceCollection : IEnumerable<KeyValuePair<string, IValueSource>> {
        private readonly ConcurrentDictionary<string, IValueSource> _valueSources;
        public ValueSourceCollection() => _valueSources = new ConcurrentDictionary<string, IValueSource>();
        public void Add<T>(IValueSource<T> valueSource) => _valueSources.TryAdd(Key<T>(), valueSource);

        public bool TryGet<T>(out IValueSource<T> valueSource) {
            if (_valueSources.TryGetValue(Key<T>(), out var result)) {
                valueSource = (IValueSource<T>)result;
                return true;
            }

            valueSource = default;
            return false;
        }

        public IEnumerator<KeyValuePair<string, IValueSource>> GetEnumerator() => _valueSources.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_valueSources).GetEnumerator();
        private static string Key<T>() => typeof(T).Name;
    }
}