﻿using System;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker
{
    [Flags]
    public enum DataStorageLoggerLogType
    {
        None = 0,
        Editor = 1,
        DevelopmentBuild = 2
    }
}