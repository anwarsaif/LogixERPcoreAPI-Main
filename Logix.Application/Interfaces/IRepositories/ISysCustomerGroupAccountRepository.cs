
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysCustomerGroupAccountRepository : IGenericRepository<SysCustomerGroupAccount>
    {
        Task<IEnumerable<SysCustomerGroupAccountsVw>> GetAllVw();
    }
}
