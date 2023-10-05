using System;

namespace PhlegmaticOne.DataStorage.Infrastructure.Exceptions {
    public class DataStorageNoInternetException : Exception {
        public DataStorageNoInternetException() : base("Requested Online OperationType when Internet not available") { }
    }
}