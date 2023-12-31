﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LoadTest.Processors;
using PhlegmaticOne.DataStorage.Storage.Base;
using PhlegmaticOne.DataStorage.Storage.ChangeTracker.Base;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LoadTest.Controllers
{
    public enum TrackActionType
    {
        TrackEveryAction,
        TrackByChangeTracker
    }

    public class ProcessorsDataController : MonoBehaviour
    {
        [SerializeField] private List<ProcessorDataBase> _processorsData;
        [SerializeField] [Range(0.001f, 5f)] private float _minActionTime;
        [SerializeField] [Range(0.001f, 5f)] private float _maxActionTime;
        [SerializeField] private TrackActionType _trackActionType;
        private IChangeTracker _changeTracker;

        private IDataStorage _dataStorage;

        private bool _isStarted;

        private void Update()
        {
            if (!_isStarted)
            {
                return;
            }

            WorkChangeTracker();
            UpdateDataProcessors();
        }

        public void Construct(IDataStorage dataStorage, IChangeTracker changeTracker)
        {
            _changeTracker = changeTracker;
            _dataStorage = dataStorage;
        }

        public async Task InitializeAsync()
        {
            foreach (var processorDataBase in _processorsData) await processorDataBase.OnInitialize(_dataStorage);

            _isStarted = true;
        }

        private void UpdateDataProcessors()
        {
            var deltaTime = Time.deltaTime;

            foreach (var processorDataBase in _processorsData)
            {
                processorDataBase.TimeToNextAction -= deltaTime;

                if (processorDataBase.TimeToNextAction > 0)
                {
                    continue;
                }

                var time = Random.Range(_minActionTime, _maxActionTime);
                processorDataBase.TimeToNextAction = time;

                switch (_trackActionType)
                {
                    case TrackActionType.TrackEveryAction:
                        processorDataBase.OnActionTrackInstant(_dataStorage);
                        break;
                    case TrackActionType.TrackByChangeTracker:
                        processorDataBase.OnAction(_dataStorage);
                        break;
                }
            }
        }

        private void WorkChangeTracker()
        {
            if (_trackActionType == TrackActionType.TrackByChangeTracker)
            {
                _changeTracker.ContinueTracking();
            }
            else
            {
                _changeTracker.StopTracking();
            }
        }
    }
}