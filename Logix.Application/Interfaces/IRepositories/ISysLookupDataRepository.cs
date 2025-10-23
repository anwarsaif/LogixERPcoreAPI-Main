
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysLookupDataRepository : IGenericRepository<SysLookupData>
    {
        Task<IEnumerable<SysLookupData>> GetDataByCategory(int categoryId);
    }
}
