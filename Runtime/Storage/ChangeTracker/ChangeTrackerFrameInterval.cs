using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker {
    
    public class ChangeTrackerFrameInterval : ChangeTrackerDataStorage {
        public ChangeTrackerFrameInterval(DataStorage dataStorage, ChangeTrackerConfiguration configuration) 
            : base(dataStorage, configuration) { }

        public override async Task TrackAsync(CancellationToken cancellationToken = default) {
            var intervalFrames = Configuration.FramesInterval;
            
            await UniTask.DelayFrame(Configuration.DelayFrames, cancellationToken: cancellationToken);
            await foreach (var _ in UniTaskAsyncEnumerable
                               .IntervalFrame(intervalFrames)
                               .WithCancellation(cancellationToken)) {
                await SaveChanges(cancellationToken);
            }
        }
    }
}