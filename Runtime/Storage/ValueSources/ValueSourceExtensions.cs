using PhlegmaticOne.DataStorage.Contracts;

namespace PhlegmaticOne.DataStorage.Storage.ValueSources
{
    public static class ValueSourceExtensions
    {
        public static bool HasChanges(this IValueSource valueSource) => valueSource.TrackedChanges > 0;

        public static bool HasNoValue<T>(this IValueSource<T> valueSource) where T : class, IModel =>
            valueSource?.Value is null;

        public static bool HasValue<T>(this IValueSource<T> valueSource) where T : class, IModel =>
            valueSource?.Value != null;
    }
}