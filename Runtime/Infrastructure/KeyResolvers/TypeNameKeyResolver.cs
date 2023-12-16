using PhlegmaticOne.DataStorage.Infrastructure.KeyResolvers.Base;

namespace PhlegmaticOne.DataStorage.Infrastructure.KeyResolvers
{
    public class TypeNameKeyResolver : IKeyResolver
    {
        private readonly string _format;

        public TypeNameKeyResolver(string format = "{0}") => _format = format;

        public string ResolveKey<T>()
        {
            var key = typeof(T).Name;
            return string.Format(_format, key);
        }
    }
}