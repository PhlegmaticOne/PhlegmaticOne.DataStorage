using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources.FileSource;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Options;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers;
using PhlegmaticOne.DataStorage.Infrastructure.KeyResolvers;

namespace PhlegmaticOne.DataStorage.Tests.Runtime.Shared
{
    internal static class Mock
    {
        public static class FileSource
        {
            public static FileDataSource<T> Get<T>(string directoryName) where T : class, IModel
            {
                var fileSerializer = new JsonFileSerializerDefault();
                var options = new FileOptionsAppPersistentPath(directoryName);
                var keyResolver = new TypeNameKeyResolver();
                return new FileDataSource<T>(fileSerializer, options, keyResolver);
            }
        }
    }
}