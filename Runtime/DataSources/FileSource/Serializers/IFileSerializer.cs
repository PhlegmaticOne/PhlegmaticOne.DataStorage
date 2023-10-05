using System.IO;

namespace PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers {
    public interface IFileSerializer {
        void Serialize<T>(Stream stream, T value);
        T Deserialize<T>(Stream stream);
        string FileExtension { get; }
    }
}