using System;
using System.Threading;
using System.Threading.Tasks;
using PhlegmaticOne.DataStorage.Storage.Base;

namespace PhlegmaticOne.DataStorage.Contracts {
    public interface IService {
        Type ModelType { get; }
        IDataStorage DataStorage { get; set; }
        Task<object> ForceReadAsync(IDataStorage dataStorage, CancellationToken ct = default);
        Task SaveStateAsync(IDataStorage dataStorage, CancellationToken ct = default);
        Task DeleteAsync(IDataStorage dataStorage, CancellationToken ct = default);
    }
}