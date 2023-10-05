using System.Threading;
using System.Threading.Tasks;

namespace PhlegmaticOne.DataStorage.DataSources.Base {
    public interface IDataSource {
        Task DeleteAsync(CancellationToken ct = default);
    }
}