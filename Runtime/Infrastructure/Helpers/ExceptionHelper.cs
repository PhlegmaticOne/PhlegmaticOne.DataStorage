using System;

namespace PhlegmaticOne.DataStorage.Infrastructure.Helpers
{
    internal static class ExceptionHelper
    {
        internal static T EnsureNotNull<T>(T value, string paramName = null)
        {
            if (value is null)
            {
                throw new ArgumentNullException(paramName);
            }

            return value;
        }
    }
}