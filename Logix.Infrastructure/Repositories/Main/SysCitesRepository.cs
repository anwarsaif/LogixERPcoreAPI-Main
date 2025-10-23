using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysCitesRepository : GenericRepository<SysCites>, ISysCitesRepository
    {
        public SysCitesRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
