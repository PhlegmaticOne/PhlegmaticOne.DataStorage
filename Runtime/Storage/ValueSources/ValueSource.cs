using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.Storage.Base;

namespace PhlegmaticOne.DataStorage.Storage.ValueSources {
    public class ValueSource<T> : IValueSource<T> where T: class, IModel {
        private readonly IDataStorage _dataStorage;

        private int _trackedChanges;
        public ValueSource(IDataStorage dataStorage) => _dataStorage = dataStorage;
        public int TrackedChanges => _trackedChanges;
        public string DisplayName => typeof(T).Name;

        public T TrackableValue {
            get {
                IncrementTrackedChanges();
                return Value;
            }
        }

        public T Value { get; private set; }

        public void SetRawValue(T value) {
            Value = value;
            IncrementTrackedChanges();
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default) {
            Value = await _dataStorage.ReadAsync<T>(cancellationToken);
        }

        public void EnqueueForSaving(CancellationToken cancellationToken = default) {
            _dataStorage.EnqueueForSaving(this, cancellationToken);
            ResetTrackedChanges();
        }

        public void EnqueueForDeleting(CancellationToken cancellationToken = default) {
            _dataStorage.EnqueueForDeleting<T>(cancellationToken);
            ResetTrackedChanges();
        }

        private void IncrementTrackedChanges() => Interlocked.Increment(ref _trackedChanges);
        private void ResetTrackedChanges() => Interlocked.Exchange(ref _trackedChanges, 0);
    }
}