using System;
using PhlegmaticOne.DataStorage.Storage.ValueSources;

namespace PhlegmaticOne.DataStorage.Storage.Logger
{
    internal sealed class DataStorageLoggerEmpty : IDataStorageLogger
    {
        public void LogException(Exception exception) { }
        public void LogCancellation(string cancellationSource) { }
        public void LogTrackedChanges(IValueSource valueSource) { }
        public void LogSaving(string key) { }
    }
}