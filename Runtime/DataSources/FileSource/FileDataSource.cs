using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.DataSources.Base;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Options;
using PhlegmaticOne.DataStorage.DataSources.FileSource.Serializers;
using PhlegmaticOne.DataStorage.Infrastructure.Helpers;
using PhlegmaticOne.DataStorage.KeyResolvers.Base;

namespace PhlegmaticOne.DataStorage.DataSources.FileSource {
    internal sealed class FileDataSource<T> : DataSourceBase<T> where T: class, IModel {
        private readonly IFileSerializer _fileSerializer;
        private readonly IFileOptions _fileOptions;
        private readonly IKeyResolver _keyResolver;

        public FileDataSource(IFileSerializer fileSerializer, IFileOptions fileOptions, IKeyResolver keyResolver) {
            _fileSerializer = ExceptionHelper.EnsureNotNull(fileSerializer, nameof(fileSerializer));
            _fileOptions = ExceptionHelper.EnsureNotNull(fileOptions, nameof(fileOptions));
            _keyResolver = ExceptionHelper.EnsureNotNull(keyResolver, nameof(keyResolver));
            EnsurePersistentDirectoryExists();
        }

        protected override Task WriteAsync(T value, CancellationToken cancellationToken) {
            var filePath = GetFilePath();
            return Task.Run(() => SerializeObjectIntoFile(filePath, value), cancellationToken);
        }

        public override Task DeleteAsync(CancellationToken cancellationToken) {
            var filePath = GetFilePath();
            return Task.Run(() => File.Delete(filePath), cancellationToken);
        }

        public override Task<T> ReadAsync(CancellationToken cancellationToken) {
            var filePath = GetFilePath();
            return Task.Run(() => DeserializeObjectFromFile(filePath), cancellationToken);
        }

        private T DeserializeObjectFromFile(string filePath) {
            if (File.Exists(filePath) == false) {
                return default;
            }
            
            using var stream = new FileStream(filePath, FileMode.Open);
            return _fileSerializer.Deserialize<T>(stream);
        }

        private void SerializeObjectIntoFile(string filePath, T value) {
            using var stream = new FileStream(filePath, FileMode.OpenOrCreate);
            _fileSerializer.Serialize(stream, value);
        }

        private string GetFilePath() {
            var fileName = _keyResolver.ResolveKey<T>();
            var fileExtension = _fileSerializer.FileExtension;
            var persistentPath = _fileOptions.PersistentPath;
            var fileFullName = string.Concat(fileName, fileExtension);
            return Path.Combine(persistentPath, fileFullName);
        }

        private void EnsurePersistentDirectoryExists() {
            var path = _fileOptions.PersistentPath;
            if (Directory.Exists(path) == false) {
                Directory.CreateDirectory(path);
            }
        }
    }
}