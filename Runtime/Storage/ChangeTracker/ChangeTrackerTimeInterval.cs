using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker {
    public class ChangeTrackerTimeInterval : ChangeTrackerDataStorage {
        public ChangeTrackerTimeInterval(DataStorage dataStorage, ChangeTrackerConfiguration configuration) 
            : base(dataStorage, configuration) { }

        public override async Task TrackAsync(CancellationToken cancellationToken = default) {
            var interval = TimeSpan.FromSeconds(Configuration.TimeInterval);

            await UniTask.Delay(TimeSpan.FromSeconds(Configuration.DelayTime), cancellationToken: cancellationToken);
            await foreach (var _ in UniTaskAsyncEnumerable
                               .Timer(interval, PlayerLoopTiming.Update, true)
                               .WithCancellation(cancellationToken)) {
                await SaveChanges(cancellationToken);
            }
        }
    }
}