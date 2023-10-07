﻿using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker {
    public abstract class ChangeTrackerDataStorage : IChangeTracker {
        protected readonly ChangeTrackerConfiguration Configuration;
        private readonly DataStorage _dataStorage;
        private readonly bool _isDebug;

        protected ChangeTrackerDataStorage(DataStorage dataStorage, ChangeTrackerConfiguration configuration) {
            _dataStorage = dataStorage;
            _isDebug = Application.isEditor || Debug.isDebugBuild;
            Configuration = ExceptionHelper.EnsureNotNull(configuration);
        }

        public abstract Task TrackAsync(CancellationToken cancellationToken = default);

        protected async Task SaveChanges(CancellationToken cancellationToken) {
            foreach (var tracker in _dataStorage.ValueSources) {
                if (tracker.TrackedChanges <= 0) {
                    continue;
                }

                if (Configuration.IsLogTrackedChangesInDebugMode && _isDebug) {
                    Debug.Log($"ChangeTracker tracked {tracker.TrackedChanges} changes in {tracker.DisplayName} and now saving them!");    
                }
                
                await tracker.SaveChangesAsync(cancellationToken);
            }
        }
    }
}