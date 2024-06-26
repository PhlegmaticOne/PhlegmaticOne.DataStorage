﻿using System;
using PhlegmaticOne.DataStorage.Storage.ValueSources;

namespace PhlegmaticOne.DataStorage.Storage.Logger
{
    public interface IDataStorageLogger
    {
        void LogException(Exception exception);
        void LogCancellation(string cancellationSource);
        void LogTrackedChanges(IValueSource valueSource);
        void LogSaving(string key);
    }
}