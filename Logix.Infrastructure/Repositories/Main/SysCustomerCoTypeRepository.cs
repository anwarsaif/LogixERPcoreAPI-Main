using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysCustomerCoTypeRepository : GenericRepository<SysCustomerCoType>, ISysCustomerCoTypeRepository
    {
        public SysCustomerCoTypeRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
