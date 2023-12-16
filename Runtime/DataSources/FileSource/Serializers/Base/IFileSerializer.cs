using System.IO;

namespace PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers.Base
{
    public interface IFileSerializer
    {
        string FileExtension { get; }
        void Serialize<T>(Stream stream, T value);
        T Deserialize<T>(Stream stream);
    }
}