using System;
using PhlegmaticOne.DataStorage.Storage.ValueSources;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker {
    public class DataStorageLoggerDebug : IDataStorageLogger {
        private readonly DataStorageLoggerLogLevel _logLevel;
        private readonly bool _isDebug;

        public DataStorageLoggerDebug(DataStorageLoggerLogLevel logLevel) {
            _logLevel = logLevel;
            _isDebug = Application.isEditor || Debug.isDebugBuild;
        }
        
        public void LogException(Exception exception) {
            Debug.LogException(exception);
            LogMessageIfLogLevelMatches(
                $"<color=#cc3300>[DataStorage]: </color>Error: {exception.Message}",
                DataStorageLoggerLogLevel.Errors);
        }

        public void LogCancellation(string cancellationSource) {
            LogMessageIfLogLevelMatches(
                $"<color=#ffcc00>[DataStorage]: </color>{cancellationSource} - cancelled",
                DataStorageLoggerLogLevel.Warnings);
        }

        public void LogTrackedChanges(IValueSource valueSource) {
            LogMessageIfLogLevelMatches(
                $"<color=#99cc33>[DataStorage]: </color>Tracked {valueSource.TrackedChanges} changes in {valueSource.DisplayName}",
                DataStorageLoggerLogLevel.Info);
        }

        public void LogSaving(IValueSource valueSource) {
            LogMessageIfLogLevelMatches(
                $"<color=#339900>[DataStorage]: </color>Saving changes in {valueSource.DisplayName}",
                DataStorageLoggerLogLevel.Info);
        }

        private void LogMessageIfLogLevelMatches(string message, DataStorageLoggerLogLevel logLevel) {
            if (ShouldLog() && _logLevel.HasFlag(logLevel)) {
                Debug.Log(message);
            }
        }

        private bool ShouldLog() => _logLevel != DataStorageLoggerLogLevel.None && _isDebug;
    }
}