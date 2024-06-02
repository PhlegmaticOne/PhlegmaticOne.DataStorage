using System;
using PhlegmaticOne.DataStorage.Storage.ValueSources;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Storage.Logger
{
    internal sealed class DataStorageLoggerDebug : IDataStorageLogger
    {
        private readonly DataStorageLoggerConfig _loggerConfig;

        private bool _isLog;

        public DataStorageLoggerDebug(DataStorageLoggerConfig loggerConfig)
        {
            _loggerConfig = loggerConfig;
            InitializeIsLog();
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
                    $"<color=#99cc33>[DataStorage]: </color>Tracked {valueSource.TrackedChanges} changes in {valueSource}");
            }
        }

        public void LogSaving(string key)
        {
            if (ShouldLog(DataStorageLoggerLogLevel.Info))
            {
                LogMessage($"<color=#339900>[DataStorage]: </color>Saving changes in {key}");
            }
        }
        
        private bool ShouldLog(DataStorageLoggerLogLevel logLevel)
        {
            var configLogLevel = _loggerConfig.LogLevel;
            return configLogLevel != DataStorageLoggerLogLevel.None && configLogLevel.HasFlag(logLevel) && _isLog;
        }

        private static void LogMessage(string message)
        {
            Debug.Log(message);
        }

        private void InitializeIsLog()
        {
            var logType = _loggerConfig.LogType;
            
#if DEVELOPMENT_BUILD
            if (logType.HasFlag(DataStorageLoggerLogType.DevelopmentBuild))
            {
                _isLog = true;
            }
#endif
#if UNITY_EDITOR
            if (logType.HasFlag(DataStorageLoggerLogType.Editor))
            {
                _isLog = true;
            }
#endif
#if UNITY_ANDROID
            if (logType.HasFlag(DataStorageLoggerLogType.Android))
            {
                _isLog = true;
            }
#endif
        }
    }
}