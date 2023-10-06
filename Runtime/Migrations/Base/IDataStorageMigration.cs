using System.Threading;
using System.Threading.Tasks;

namespace PhlegmaticOne.DataStorage.Migrations {
    public interface IDataStorageMigration {
        bool IsMigrationAvailable { get; }
        Task MigrateAsync(CancellationToken ct = default);
    }
}