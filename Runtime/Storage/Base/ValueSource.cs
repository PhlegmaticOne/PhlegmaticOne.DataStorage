using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;

namespace PhlegmaticOne.DataStorage.Storage.Base {
    public class ValueSource<T> : IValueSource<T> where T: class, IModel {
        private readonly IDataStorage _dataStorage;
        private T _value;

        public ValueSource(IDataStorage dataStorage, T value) {
            _dataStorage = dataStorage;
            _value = value;
        }
        
        public int TrackedChanges { get; private set; }
        public string DisplayName => typeof(T).Name;

        public T AsTrackable() {
            TrackedChanges++;
            return _value;
        }

        public T AsNoTrackable() => _value;
        
        public void SetRaw(T value) {
            TrackedChanges++;
            _value = value;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default) {
            await _dataStorage.SaveAsync(_value, cancellationToken);
            TrackedChanges = 0;
        }
    }
}