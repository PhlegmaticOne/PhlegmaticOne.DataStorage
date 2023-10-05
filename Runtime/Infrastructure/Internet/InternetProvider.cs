using PhlegmaticOne.DataStorage.Infrastructure.Internet.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Infrastructure.Internet {
    public class InternetProvider : IInternetProvider {
        public bool IsActive() {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }
}