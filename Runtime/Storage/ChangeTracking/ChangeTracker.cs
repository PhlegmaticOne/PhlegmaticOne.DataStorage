using System;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Infrastructure.Cancellation;
using PhlegmaticOne.DataStorage.Storage.Base;
using PhlegmaticOne.DataStorage.Storage.Logger;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracking
{
    internal static class ChangeTracker
    {
        private const string CancellationSource = "ChangeTracker";
        
        private static bool IsTracking;
        
        internal static void Run(IDataStorage dataStorage,
            ChangeTrackerConfig changeTrackerConfig,
            IDataStorageLogger logger,
            IDataStorageCancellationProvider cancellationProvider)
        {
            if (IsTracking)
            {
                return;
            }

            IsTracking = true;
            _ = TrackPrivate(dataStorage, changeTrackerConfig, logger, cancellationProvider);
        }

        private static async Task TrackPrivate(
            IDataStorage dataStorage,
            ChangeTrackerConfig config,
            IDataStorageLogger logger,
            IDataStorageCancellationProvider cancellationProvider)
        {
            try
            {
                var token = cancellationProvider.Token;
                var interval = TimeSpan.FromSeconds(config.TrackInterval);
                var delayTime = TimeSpan.FromSeconds(config.TrackStartDelay);

                await Task.Delay(delayTime, token);

                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        dataStorage.RequestSaveChanges();
                        await Task.Delay(interval, token);
                    }
                    catch (OperationCanceledException)
                    {
                        logger.LogCancellation(CancellationSource);
                    }
                    catch (Exception exception)
                    {
                        logger.LogException(exception);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogCancellation(CancellationSource);
            }
        }
    }
}