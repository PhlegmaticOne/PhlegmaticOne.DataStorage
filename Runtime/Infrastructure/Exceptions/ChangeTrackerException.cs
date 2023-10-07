using System;

namespace PhlegmaticOne.DataStorage.Infrastructure.Exceptions {
    public class ChangeTrackerException : Exception {
        public ChangeTrackerException(Exception innerException) :
            base("Change tracker stopped working because Exception was thrown", innerException) { }
    }
}