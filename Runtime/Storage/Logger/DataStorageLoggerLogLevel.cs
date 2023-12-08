using System;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker
{
    [Flags]
    public enum DataStorageLoggerLogLevel
    {
        None = 0,
        Info = 1,
        Warnings = 2,
        Errors = 4
    }
}