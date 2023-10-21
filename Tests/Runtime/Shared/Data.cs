using System.Collections.Generic;
using Phlegmaticone.DataStorage.Tests.Runtime.Shared;

namespace PhlegmaticOne.DataStorage.Tests.Runtime.Shared {
    internal class Data {
        private static List<string> ListValues => new List<string>() {
            "Test1", "Test2", "Test3"
        };

        public static PagedList TestList => PagedList.OnePage(ListValues);
    }
}