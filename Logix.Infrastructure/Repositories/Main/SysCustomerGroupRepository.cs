using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysCustomerGroupRepository : GenericRepository<SysCustomerGroup>, ISysCustomerGroupRepository
    {
        public SysCustomerGroupRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
