using System;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;

namespace PhlegmaticOne.DataStorage.Storage.ValueSources
{
    public static class ValueSourceExtensions
    {
        public static async Task EnsureInitializedAsync<T>(this IValueSource<T> valueSource)
            where T : class, IModel, new()
        {
            await valueSource.InitializeAsync();

            if (valueSource.HasNoValue())
            {
                valueSource.SetRawValue(new T());
            }
        }
        
        public static async Task EnsureInitializedAsync<T>(this IValueSource<T> valueSource, Func<T> stateFactory) 
            where T : class, IModel, new()
        {
            await valueSource.InitializeAsync();

            if (valueSource.HasNoValue())
            {
                var state = stateFactory();
                valueSource.SetRawValue(state);
            }
        }
        
        public static bool HasChanges(this IValueSource valueSource)
        {
            return valueSource.TrackedChanges > 0;
        }

        public static bool HasNoValue<T>(this IValueSource<T> valueSource) where T : class, IModel, new()
        {
            return valueSource?.Value is null;
        }

        public static bool HasValue<T>(this IValueSource<T> valueSource) where T : class, IModel, new()
        {
            return valueSource?.Value != null;
        }
    }
}