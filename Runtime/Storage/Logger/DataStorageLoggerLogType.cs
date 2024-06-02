using System;

namespace PhlegmaticOne.DataStorage.Storage.Logger
{
    [Flags]
    public enum DataStorageLoggerLogType
    {
        None = 0,
        Editor = 1,
        DevelopmentBuild = 2,
        Android = 4
    }
}