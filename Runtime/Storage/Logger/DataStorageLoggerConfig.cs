namespace PhlegmaticOne.DataStorage.Storage.Logger
{
    public class DataStorageLoggerConfig
    {
        public DataStorageLoggerConfig()
        {
            LogLevel = DataStorageLoggerLogLevel.Info |
                       DataStorageLoggerLogLevel.Warnings |
                       DataStorageLoggerLogLevel.Errors;

            LogType = DataStorageLoggerLogType.Editor |
                      DataStorageLoggerLogType.DevelopmentBuild;
        }
        
        public DataStorageLoggerLogLevel LogLevel { get; set; }
        public DataStorageLoggerLogType LogType { get; set; }
    }
}