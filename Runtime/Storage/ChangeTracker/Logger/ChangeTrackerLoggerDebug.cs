using PhlegmaticOne.DataStorage.Infrastructure.Helpers;
using PhlegmaticOne.DataStorage.Storage.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker {
    public class ChangeTrackerLoggerDebug : IChangeTrackerLogger {
        private readonly ChangeTrackerConfiguration _configuration;
        private readonly bool _isDebug;

        public ChangeTrackerLoggerDebug(ChangeTrackerConfiguration configuration) {
            _isDebug = Application.isEditor || Debug.isDebugBuild;
            _configuration = ExceptionHelper.EnsureNotNull(configuration);
        }
        
        public void LogError(string message) {
            if (ShouldLog()) {
                Debug.Log($"<color=#cc3300>ChangeTracker: </color>Error: {message}");
            }
        }

        public void LogCancellation() {
            if (ShouldLog()) {
                Debug.Log("<color=#ffcc00>ChangeTracker: </color>Tracking was canceled");
            }
        }

        public void LogTrackedChanges(IValueSource tracker) {
            if (ShouldLog()) {
                Debug.Log($"<color=#99cc33>ChangeTracker: </color>Tracked {tracker.TrackedChanges} changes in {tracker.DisplayName} and now saving them!");
            }
        }
        
        private bool ShouldLog() => _configuration.IsChangeTrackerVerbose && _isDebug;
    }
}