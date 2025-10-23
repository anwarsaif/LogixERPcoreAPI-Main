using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysScreenRepository : GenericRepository<SysScreen>, ISysScreenRepository
    {
        public SysScreenRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
