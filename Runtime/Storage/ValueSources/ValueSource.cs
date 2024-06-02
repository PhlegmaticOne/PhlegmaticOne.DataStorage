using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.Storage.Base;

namespace PhlegmaticOne.DataStorage.Storage.ValueSources
{
    internal sealed class ValueSource<T> : IValueSource<T> where T : class, IModel, new()
    {
        private readonly IDataStorage _dataStorage;

        private int _trackedChanges;

        public ValueSource(IDataStorage dataStorage, string key)
        {
            Key = key;
            _dataStorage = dataStorage;
        }

        public string Key { get; }
        public int TrackedChanges => _trackedChanges;
        public T TrackableValue
        {
            get
            {
                IncrementTrackedChanges();
                return Value;
            }
        }

        public T Value { get; private set; }

        public void SetRawValue(T value)
        {
            Value = value;
            IncrementTrackedChanges();
        }

        public async Task InitializeAsync()
        {
            Value ??= await _dataStorage.ReadAsync<T>(Key);
        }

        public void EnqueueSave()
        {
            _dataStorage.EnqueueSave(Key, Value);
            ResetTrackedChanges();
        }

        public void EnqueueDelete()
        {
            _dataStorage.EnqueueDelete<T>(Key);
            ResetTrackedChanges();
        }

        private void IncrementTrackedChanges()
        {
            Interlocked.Increment(ref _trackedChanges);
        }

        private void ResetTrackedChanges()
        {
            Interlocked.Exchange(ref _trackedChanges, 0);
        }

        public override string ToString()
        {
            return typeof(T).Name;
        }
    }
}