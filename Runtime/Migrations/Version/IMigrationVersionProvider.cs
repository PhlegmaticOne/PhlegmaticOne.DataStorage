namespace PhlegmaticOne.DataStorage.Migrations {
    public interface IMigrationVersionProvider {
        int GetVersion();
        void SetVersion(int version);
        void ResetVersion();
    }
}