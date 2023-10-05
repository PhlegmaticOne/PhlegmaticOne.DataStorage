using PhlegmaticOne.DataStorage.KeyResolvers.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.KeyResolvers {
    public abstract class DataStorageKeyResolverConfigurationBase : ScriptableObject {
        public abstract IKeyResolver CreateKeyResolver();
    }
}