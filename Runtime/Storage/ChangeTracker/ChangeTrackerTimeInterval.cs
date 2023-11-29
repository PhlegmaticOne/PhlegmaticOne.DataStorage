using System;
using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Infrastructure.Cancellation;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;
using PhlegmaticOne.DataStorage.Storage.Base;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker.Base;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker {
    public class ChangeTrackerTimeInterval : IChangeTracker {
        private const string CancellationSource = "ChangeTracker";

        private readonly IDataStorage _dataStorage;
        private readonly IDataStorageLogger _logger;
        private readonly ChangeTrackerConfiguration _configuration;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private int _hasBeganTracking;
        private int _isTracking;

        public ChangeTrackerTimeInterval(IDataStorage dataStorage,
            ChangeTrackerConfiguration changeTrackerConfig, 
            IDataStorageLogger logger,
            CancellationTokenSource cancellationTokenSource) {
            _cancellationTokenSource = cancellationTokenSource;
            _dataStorage = ExceptionHelper.EnsureNotNull(dataStorage);
            _logger = ExceptionHelper.EnsureNotNull(logger, nameof(logger));
            _configuration = ExceptionHelper.EnsureNotNull(changeTrackerConfig);
            _isTracking = 1;
        }

        public void StopTracking() {
            Interlocked.Exchange(ref _isTracking, 0);
        }

        public void ContinueTracking() {
            Interlocked.Exchange(ref _isTracking, 1);
        }

        public async Task TrackAsync(CancellationToken cancellationToken = default) {
            try {
                if (HasBeganTracking()) {
                    return;
                }
                
                await TrackChanges(cancellationToken);
            }
            catch (OperationCanceledException) {
                _logger.LogCancellation(CancellationSource);
            }
            catch (Exception exception) {
                _logger.LogException(exception);
            }
        }

        private async Task TrackChanges(CancellationToken cancellationToken) {
            using var tokenSource = _cancellationTokenSource.LinkWith(cancellationToken);
            var token = tokenSource.Token;
            var interval = TimeSpan.FromSeconds(_configuration.TimeInterval);
            var delayTime = TimeSpan.FromSeconds(_configuration.TimeDelay);
            
            await Task.Delay(delayTime, cancellationToken);

            while (!token.IsCancellationRequested) {
                if (IsTracking()) {
                    _dataStorage.RequestTrackedChangesSaving();
                }
                await Task.Delay(interval, token);
            }
        }

        private bool IsTracking() {
            return _isTracking == 1;
        }

        private bool HasBeganTracking() {
            return Interlocked.CompareExchange(ref _hasBeganTracking, 1, 0) == 1;
        }
    }
}