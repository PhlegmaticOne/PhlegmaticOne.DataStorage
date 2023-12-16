using PhlegmaticOne.DataStorage.Configuration.Helpers;
using PhlegmaticOne.DataStorage.Infrastructure.KeyResolvers;
using PhlegmaticOne.DataStorage.Infrastructure.KeyResolvers.Base;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.Configuration.KeyResolvers
{
    [CreateAssetMenu(
        menuName = "Data Storage/Infrastructure/Type Name Key Resolver Configuration",
        fileName = "TypeNameKeyResolverConfiguration")]
    public class DataStorageTypeKeyResolverConfig : DataStorageKeyResolverConfig, IDefaultSetupConfig
    {
        [SerializeField] private string _format;
        public void SetupDefault() => _format = "{0}";
        public override IKeyResolver GetKeyResolver() => new TypeNameKeyResolver(_format);
    }
}