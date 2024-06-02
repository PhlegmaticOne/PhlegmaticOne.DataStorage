using System;
using JetBrains.Annotations;
using PhlegmaticOne.DataStorage.Storage.Base;
using PhlegmaticOne.DataStorage.Storage.ChangeTracking;
using PhlegmaticOne.DataStorage.Storage.Logger;

namespace PhlegmaticOne.DataStorage.Storage.Builder.Base
{
    public interface IDataStorageBuilder
    {
        IDataStorageBuilder UseLogger(IDataStorageLogger logger);
        IDataStorageBuilder UseLogger([CanBeNull] Action<DataStorageLoggerConfig> configAction = null);
        IDataStorageBuilder UseChangeTracker([CanBeNull] Action<ChangeTrackerConfig> configAction = null);
        IDataStorage Build();
    }
}