using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker {
    public abstract class ChangeTrackerDataStorage : IChangeTracker {
        protected readonly ChangeTrackerConfiguration Configuration;
        private readonly DataStorage _dataStorage;

        protected ChangeTrackerDataStorage(DataStorage dataStorage, ChangeTrackerConfiguration configuration) {
            _dataStorage = dataStorage;
            Configuration = ExceptionHelper.EnsureNotNull(configuration);
        }

        public abstract Task TrackAsync(CancellationToken cancellationToken = default);

        protected async Task SaveChanges(CancellationToken cancellationToken) {
            foreach (var tracker in _dataStorage.ValueSources) {
                if (tracker.TrackedChanges <= 0) {
                    continue;
                }
                
                await tracker.SaveChangesAsync(cancellationToken);
            }
        }
    }
}