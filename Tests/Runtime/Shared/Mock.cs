using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources.FileSource;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Options;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers;
using PhlegmaticOne.DataStorage.KeyResolvers;

namespace PhlegmaticOne.DataStorage.Tests.Runtime.Shared {
    internal class Mock {
        public class FileSource {
            public static FileDataSource<T> Get<T>(string directoryName) where T : class, IModel {
                var fileSerializer = new JsonFileSerializer();
                var options = new FileOptionsAppPersistentPath(directoryName);
                var keyResolver = new TypeNameKeyResolver();
                return new FileDataSource<T>(fileSerializer, options, keyResolver);
            }
        }
    }
}