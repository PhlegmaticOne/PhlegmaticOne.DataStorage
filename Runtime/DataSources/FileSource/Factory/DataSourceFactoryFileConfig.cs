using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers.Base;

namespace PhlegmaticOne.DataStorage.DataSources.FileSource.Factory
{
    public class DataSourceFactoryFileConfig
    {
        public DataSourceFactoryFileConfig()
        {
            DirectoryName = "PlayerSaves";
            Serializer = new JsonFileSerializerDefault();
        }
        
        public string DirectoryName { get; set; }
        public IFileSerializer Serializer { get; set; }
    }
}