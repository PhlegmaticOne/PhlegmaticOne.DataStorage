namespace PhlegmaticOne.DataStorage.Migrations {
    public interface IDataStorageMigrationData {
        int MigrationVersion { get; }
        IDataStorageConfiguration PreviousStorage { get; }
        IDataStorageConfiguration CurrentStorage { get; }
    }
}