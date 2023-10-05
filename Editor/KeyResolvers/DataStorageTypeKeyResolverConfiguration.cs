using PhlegmaticOne.DataStorage.KeyResolvers;
using PhlegmaticOne.DataStorage.KeyResolvers.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.KeyResolvers {
    [CreateAssetMenu(
        menuName = "Data Storage/Type Name Key Resolver Configuration",
        fileName = "TypeNameKeyResolverConfiguration")]
    public sealed class DataStorageTypeKeyResolverConfiguration : DataStorageKeyResolverConfigurationBase {
        public override IKeyResolver CreateKeyResolver() => new TypeNameKeyResolver();
    }
}