using System;
using PhlegmaticOne.DataStorage.Storage.ValueSources;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker
{
    public class DataStorageLoggerDebug : IDataStorageLogger
    {
        private readonly bool _isDebug;
        private readonly DataStorageLoggerLogLevel _logLevel;

        public DataStorageLoggerDebug(DataStorageLoggerLogLevel logLevel)
        {
            _logLevel = logLevel;
            _isDebug = Application.isEditor || Debug.isDebugBuild;
        }

        public void LogException(Exception exception)
        {
            Debug.LogException(exception);

            if (ShouldLog(DataStorageLoggerLogLevel.Errors))
            {
                LogMessage($"<color=#cc3300>[DataStorage]: </color>Error: {exception.Message}");
            }
        }

        public void LogCancellation(string cancellationSource)
        {
            if (ShouldLog(DataStorageLoggerLogLevel.Warnings))
            {
                LogMessage($"<color=#ffcc00>[DataStorage]: </color>{cancellationSource} - cancelled");
            }
        }

        public void LogTrackedChanges(IValueSource valueSource)
        {
            if (ShouldLog(DataStorageLoggerLogLevel.Info))
            {
                LogMessage(
                    $"<color=#99cc33>[DataStorage]: </color>Tracked {valueSource.TrackedChanges} changes in {valueSource.DisplayName}");
            }
        }

        public void LogSaving(IValueSource valueSource)
        {
            if (ShouldLog(DataStorageLoggerLogLevel.Info))
            {
                LogMessage($"<color=#339900>[DataStorage]: </color>Saving changes in {valueSource.DisplayName}");
            }
        }

        private bool ShouldLog(DataStorageLoggerLogLevel logLevel)
        {
            return _logLevel != DataStorageLoggerLogLevel.None && _logLevel.HasFlag(logLevel) && _isDebug;
        }

        private static void LogMessage(string message)
        {
            Debug.Log(message);
        }
    }
}