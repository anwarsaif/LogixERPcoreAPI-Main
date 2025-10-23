using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysVatGroupRepository : GenericRepository<SysVatGroup>, ISysVatGroupRepository
    {
        public SysVatGroupRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
    
}
