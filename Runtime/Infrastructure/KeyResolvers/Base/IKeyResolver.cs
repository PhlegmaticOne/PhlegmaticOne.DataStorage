namespace PhlegmaticOne.DataStorage.Infrastructure.KeyResolvers.Base {
    public interface IKeyResolver {
        string ResolveKey<T>();
    }
}