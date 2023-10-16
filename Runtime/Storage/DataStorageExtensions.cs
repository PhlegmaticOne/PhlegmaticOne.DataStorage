using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.Storage.Base;

namespace PhlegmaticOne.DataStorage.Storage {
    public static class DataStorageExtensions {
        public static bool NoValue<T>(this IValueSource<T> valueSource) where T : class, IModel => 
            valueSource?.AsNoTrackable() is null;

        public static bool HasValue<T>(this IValueSource<T> valueSource) where T : class, IModel => 
            valueSource?.AsNoTrackable() != null;
    }
}