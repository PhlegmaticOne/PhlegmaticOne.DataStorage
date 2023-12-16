using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Options;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers.Base;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;
using PhlegmaticOne.DataStorage.Infrastructure.KeyResolvers.Base;

namespace PhlegmaticOne.DataStorage.DataSources.FileSource
{
    public class FileDataSource<T> : IDataSource<T> where T : class, IModel
    {
        private readonly IFileOptions _fileOptions;
        private readonly string _filePath;
        private readonly IFileSerializer _fileSerializer;
        private readonly IKeyResolver _keyResolver;

        public FileDataSource(IFileSerializer fileSerializer, IFileOptions fileOptions, IKeyResolver keyResolver)
        {
            _fileSerializer = ExceptionHelper.EnsureNotNull(fileSerializer, nameof(fileSerializer));
            _fileOptions = ExceptionHelper.EnsureNotNull(fileOptions, nameof(fileOptions));
            _keyResolver = ExceptionHelper.EnsureNotNull(keyResolver, nameof(keyResolver));
            _filePath = GetFilePath();
            EnsurePersistentDirectoryExists();
        }

        public Task WriteAsync(T value, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => SerializeObjectIntoFile(_filePath, value), cancellationToken);
        }

        public Task DeleteAsync(CancellationToken cancellationToken = default)
        {
            return Task.Run(() => File.Delete(_filePath), cancellationToken);
        }

        public Task<T> ReadAsync(CancellationToken cancellationToken = default)
        {
            return Task.Run(() => DeserializeObjectFromFile(_filePath), cancellationToken);
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

        private string GetFilePath()
        {
            var fileName = _keyResolver.ResolveKey<T>();
            var fileExtension = _fileSerializer.FileExtension;
            var persistentPath = _fileOptions.PersistentPath;
            var fileFullName = string.Concat(fileName, fileExtension);
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