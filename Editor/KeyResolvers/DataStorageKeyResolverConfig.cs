using PhlegmaticOne.DataStorage.Infrastructure.KeyResolvers.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.KeyResolvers
{
    public abstract class DataStorageKeyResolverConfig : ScriptableObject
    {
        public abstract IKeyResolver GetKeyResolver();
    }
}