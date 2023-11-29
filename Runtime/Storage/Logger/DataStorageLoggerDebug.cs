using System;
using PhlegmaticOne.DataStorage.Storage.ValueSources;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker {
    public class DataStorageLoggerDebug : IDataStorageLogger {
        private readonly bool _verbose;
        private readonly bool _isDebug;

        public DataStorageLoggerDebug(bool verbose) {
            _verbose = verbose;
            _isDebug = Application.isEditor || Debug.isDebugBuild;
        }
        
        public void LogException(Exception exception) {
            Debug.LogException(exception);
            LogMessagePrivate($"<color=#cc3300>[DataStorage]: </color>Error: {exception.Message}");
        }

        public void LogCancellation(string cancellationSource) {
            LogMessagePrivate($"<color=#ffcc00>[DataStorage]: </color>{cancellationSource} - cancelled");
        }

        public void LogTrackedChanges(IValueSource valueSource) {
            LogMessagePrivate($"<color=#99cc33>[DataStorage]: </color>Tracked {valueSource.TrackedChanges} changes in {valueSource.DisplayName}");
        }

        public void LogSaving(IValueSource valueSource) {
            LogMessagePrivate($"<color=#339900>[DataStorage]: </color>Saving changes in {valueSource.DisplayName}");
        }

        private void LogMessagePrivate(string message) {
            if (ShouldLog()) {
                Debug.Log(message);
            }
        }

        private bool ShouldLog() => _verbose && _isDebug;
    }
}