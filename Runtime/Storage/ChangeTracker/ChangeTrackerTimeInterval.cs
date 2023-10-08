using System;
using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Infrastructure.Exceptions;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker {
    public class ChangeTrackerTimeInterval : ChangeTrackerDataStorage {
        public ChangeTrackerTimeInterval(DataStorage dataStorage, ChangeTrackerConfiguration configuration) : 
            base(dataStorage, configuration) { }

        public override async Task TrackAsync(CancellationToken cancellationToken = default) {
            try {
                await TrackChanges(cancellationToken);
            }
            catch (OperationCanceledException) {
                Logger.LogCancellation();
            }
            catch (Exception e) {
                Logger.LogError(e.Message);
                Debug.LogException(new ChangeTrackerException(e));
            }
        }

        private async Task TrackChanges(CancellationToken cancellationToken) {
            var interval = TimeSpan.FromSeconds(Configuration.TimeInterval);
            var delayTime = TimeSpan.FromSeconds(Configuration.TimeDelay);
            
            await Task.Delay(delayTime, cancellationToken);

            while (!cancellationToken.IsCancellationRequested) {
                await SaveChanges(cancellationToken);
                await Task.Delay(interval, cancellationToken);
            }
        }
    }
}