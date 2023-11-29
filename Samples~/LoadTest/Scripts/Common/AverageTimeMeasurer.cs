using System;

namespace LoadTest.Common {
    public class AverageTimeMeasurer {
        private DateTime _previousMeasurementTime;
        private TimeSpan _previousAverageTime;
        private int _measurementsCount;

        public AverageTimeMeasurer() => _previousMeasurementTime = DateTime.Now;

        public TimeSpan RemeasureFromNow() {
            _measurementsCount++;
            
            var now = DateTime.Now;
            var difference = now - _previousMeasurementTime;
            var newAverageTime = _previousAverageTime + (difference - _previousAverageTime) / _measurementsCount;

            _previousMeasurementTime = now;
            _previousAverageTime = newAverageTime;
            return newAverageTime;
        } 
    }
}