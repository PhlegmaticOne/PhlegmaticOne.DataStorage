using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Contracts;
using PhlegmaticOne.DataStorage.Storage.Base;

namespace PhlegmaticOne.DataStorage.Migrations {
    public class DataStorageMigration : IDataStorageMigration {
        private readonly IList<IService> _services;
        private readonly IDataStorageMigrationData _migrationData;
        private readonly IMigrationVersionProvider _migrationVersionProvider;

        public DataStorageMigration(IList<IService> services, IDataStorageMigrationData migrationData,
            IMigrationVersionProvider migrationVersionProvider) {
            _services = services;
            _migrationData = migrationData;
            _migrationVersionProvider = migrationVersionProvider;
        }
        
        public bool IsMigrationAvailable => _migrationData.MigrationVersion > _migrationVersionProvider.GetVersion();

        public async Task MigrateAsync(CancellationToken ct = default) {
            if (IsMigrationAvailable == false) {
                _migrationVersionProvider.SetVersion(_migrationData.MigrationVersion);
                return;
            }
            
            var previousStorage = GetStorage(_migrationData.PreviousStorage);
            var currentStorage = GetStorage(_migrationData.CurrentStorage);
            await Migrate(previousStorage, currentStorage, ct);
            _migrationVersionProvider.SetVersion(_migrationData.MigrationVersion);
        }

        private async Task Migrate(IDataStorage previousStorage, IDataStorage currentStorage, CancellationToken ct) {
            var services = GetServices();
            await ReadFromCurrentStorage(previousStorage, services, ct);
            await previousStorage.ClearAsync(ct);
            await WriteToMigratingStorage(currentStorage, services, ct);
        }

        private IList<IService> GetServices() {
            var services = new Dictionary<Type, IService>();
            
            foreach (var service in _services) {
                if (services.ContainsKey(service.ModelType)) {
                    continue;
                }
                
                services.Add(service.ModelType, service);
            }

            return services.Values.ToList();
        }

        private DataStorage.Storage.DataStorage GetStorage(IDataStorageConfiguration configuration) {
            return new DataStorage.Storage.DataStorage(configuration.GetSourceContainer(), _services);
        }

        private static async Task ReadFromCurrentStorage(IDataStorage currentStorage, IList<IService> services, CancellationToken ct) {
            foreach (var service in services) {
                await service.ForceReadAsync(currentStorage, ct);
            }
        }

        private static async Task WriteToMigratingStorage(IDataStorage migratingStorage, IList<IService> services, CancellationToken ct) {
            foreach (var service in services) {
                await service.SaveStateAsync(migratingStorage, ct);
            }
        }
    }
}