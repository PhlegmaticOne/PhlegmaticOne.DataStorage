﻿using System;
using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Infrastructure.Cancellation;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;
using PhlegmaticOne.DataStorage.Storage.Base;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker.Base;

namespace PhlegmaticOne.DataStorage.Storage.ChangeTracker
{
    public class ChangeTrackerTimeInterval : IChangeTracker
    {
        private const string CancellationSource = "ChangeTracker";
        private readonly IDataStorageCancellationProvider _cancellationProvider;
        private readonly ChangeTrackerConfiguration _configuration;

        private readonly IDataStorage _dataStorage;
        private readonly IDataStorageLogger _logger;

        private int _hasBeganTracking;
        private int _isTracking;

        public ChangeTrackerTimeInterval(IDataStorage dataStorage,
            ChangeTrackerConfiguration changeTrackerConfig,
            IDataStorageLogger logger,
            IDataStorageCancellationProvider cancellationProvider)
        {
            _cancellationProvider = ExceptionHelper.EnsureNotNull(cancellationProvider, nameof(cancellationProvider));
            _dataStorage = ExceptionHelper.EnsureNotNull(dataStorage, nameof(dataStorage));
            _logger = ExceptionHelper.EnsureNotNull(logger, nameof(logger));
            _configuration = ExceptionHelper.EnsureNotNull(changeTrackerConfig, nameof(dataStorage));
            _isTracking = 1;
        }

        public void StopTracking()
        {
            Interlocked.Exchange(ref _isTracking, 0);
        }

        public void ContinueTracking()
        {
            Interlocked.Exchange(ref _isTracking, 1);
        }

        public Task TrackAsync(CancellationToken cancellationToken = default)
        {
            if (HasBeganTracking())
            {
                return Task.CompletedTask;
            }

            var tokenSource = _cancellationProvider.LinkWith(cancellationToken);

            return Task.Run(async () =>
            {
                try
                {
                    var token = tokenSource.Token;
                    var interval = TimeSpan.FromSeconds(_configuration.TimeInterval);
                    var delayTime = TimeSpan.FromSeconds(_configuration.TimeDelay);

                    await Task.Delay(delayTime, cancellationToken);

                    while (!token.IsCancellationRequested)
                    {
                        if (IsTracking())
                        {
                            _dataStorage.RequestTrackedChangesSaving();
                        }

                        await Task.Delay(interval, token);
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger.LogCancellation(CancellationSource);
                }
                catch (Exception exception)
                {
                    _logger.LogException(exception);
                }
                finally
                {
                    tokenSource.Dispose();
                }
            }, tokenSource.Token);
        }

        private bool IsTracking() => _isTracking == 1;

        private bool HasBeganTracking() => Interlocked.CompareExchange(ref _hasBeganTracking, 1, 0) == 1;
    }
}