using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysSmsRepository : IGenericRepository<SysSms>
    {
        Task<int> AddByProcedure(SysSmsDto obj);
    }
}
