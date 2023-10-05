using System.IO;
using UnityEngine;

namespace PhlegmaticOne.DataStorage.DataSources.FileSource.Options {
    public sealed class FileOptionsAppPersistentPath : IFileOptions {
        private readonly string _directoryName;
        private readonly string _persistentDataPath;
        public FileOptionsAppPersistentPath(string directoryName) {
            _directoryName = directoryName;
            _persistentDataPath = Application.persistentDataPath;
        }

        public string PersistentPath => Path.Combine(_persistentDataPath, _directoryName);
    }
}