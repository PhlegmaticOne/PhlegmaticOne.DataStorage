using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker.Base;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker {
    public abstract class ChangeTrackerDataStorage : IChangeTracker {
        private readonly DataStorage _dataStorage;
        protected readonly ChangeTrackerConfiguration Configuration;
        protected readonly IChangeTrackerLogger Logger;

        protected ChangeTrackerDataStorage(DataStorage dataStorage, 
            ChangeTrackerConfiguration configuration,
            IChangeTrackerLogger logger) {
            _dataStorage = ExceptionHelper.EnsureNotNull(dataStorage);
            Logger = ExceptionHelper.EnsureNotNull(logger);
            Configuration = ExceptionHelper.EnsureNotNull(configuration);
        }

        public abstract Task TrackAsync(CancellationToken cancellationToken = default);

        protected async Task SaveChanges(CancellationToken cancellationToken) {
            foreach (var tracker in _dataStorage.ValueSources.ToArray()) {
                if (tracker.TrackedChanges <= 0) {
                    continue;
                }

                Logger.LogTrackedChanges(tracker);
                await tracker.SaveChangesAsync(cancellationToken);
            }
        }
    }
}