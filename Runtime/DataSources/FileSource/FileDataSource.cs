using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Options;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers.Base;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;

namespace PhlegmaticOne.DataStorage.DataSources.FileSource
{
    internal sealed class FileDataSource<T> : IDataSource<T> where T : class, IModel
    {
        private readonly IFileOptions _fileOptions;
        private readonly IFileSerializer _fileSerializer;

        public FileDataSource(IFileSerializer fileSerializer, IFileOptions fileOptions)
        {
            _fileSerializer = ExceptionHelper.EnsureNotNull(fileSerializer, nameof(fileSerializer));
            _fileOptions = ExceptionHelper.EnsureNotNull(fileOptions, nameof(fileOptions));
            EnsurePersistentDirectoryExists();
        }

        public Task WriteAsync(string key, T value, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => SerializeObjectIntoFile(GetFilePath(key), value), cancellationToken);
        }

        public Task DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            File.Delete(GetFilePath(key));
            return Task.CompletedTask;
        }

        public Task<T> ReadAsync(string key, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => DeserializeObjectFromFile(GetFilePath(key)), cancellationToken);
        }

        private T DeserializeObjectFromFile(string filePath)
        {
            if (File.Exists(filePath) == false)
            {
                return default;
            }

            using var stream = new FileStream(filePath, FileMode.Open);
            return _fileSerializer.Deserialize<T>(stream);
        }

        private void SerializeObjectIntoFile(string filePath, T value)
        {
            using var stream = new FileStream(filePath, FileMode.OpenOrCreate);
            _fileSerializer.Serialize(stream, value);
        }

        private string GetFilePath(string key)
        {
            var fileExtension = _fileSerializer.FileExtension;
            var persistentPath = _fileOptions.PersistentPath;
            var fileFullName = string.Concat(key, fileExtension);
            return Path.Combine(persistentPath, fileFullName);
        }

        private void EnsurePersistentDirectoryExists()
        {
            var path = _fileOptions.PersistentPath;

            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}