using System;
using System.Diagnostics.Tracing;
using PhlegmaticOne.DataStorage.Storage.ValueSources;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker
{
    public class DataStorageLoggerDebug : IDataStorageLogger
    {
        private readonly DataStorageLoggerLogLevel _logLevel;
        private readonly bool _isDebug;

        public DataStorageLoggerDebug(DataStorageLoggerLogLevel logLevel)
        {
            _logLevel = logLevel;
            _isDebug = Application.isEditor || Debug.isDebugBuild;
        }

        public void LogException(Exception exception)
        {
            Debug.LogException(exception);

            if (_logLevel.HasFlag(EventLevel.Error))
            {
                LogMessage($"<color=#cc3300>[DataStorage]: </color>Error: {exception.Message}");
            }
        }

        public void LogCancellation(string cancellationSource)
        {
            if (_logLevel.HasFlag(EventLevel.Warning))
            {
                LogMessage($"<color=#ffcc00>[DataStorage]: </color>{cancellationSource} - cancelled");
            }
        }

        public void LogTrackedChanges(IValueSource valueSource)
        {
            if (_logLevel.HasFlag(DataStorageLoggerLogLevel.Info))
            {
                LogMessage($"<color=#99cc33>[DataStorage]: </color>Tracked {valueSource.TrackedChanges} changes in {valueSource.DisplayName}");
            }
        }

        public void LogSaving(IValueSource valueSource)
        {
            if (_logLevel.HasFlag(DataStorageLoggerLogLevel.Info))
            {
                LogMessage($"<color=#339900>[DataStorage]: </color>Saving changes in {valueSource.DisplayName}");
            }
        }

        private void LogMessage(string message)
        {
            if (ShouldLog())
            {
                Debug.Log(message);
            }
        }

        private bool ShouldLog() => _logLevel != DataStorageLoggerLogLevel.None && _isDebug;
    }
}