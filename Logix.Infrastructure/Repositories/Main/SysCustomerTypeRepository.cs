using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysCustomerTypeRepository : GenericRepository<SysCustomerType>, ISysCustomerTypeRepository
    {
        public SysCustomerTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
