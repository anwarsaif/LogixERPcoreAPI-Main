using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysPoliciesProcedureRepository : GenericRepository<SysPoliciesProcedure>, ISysPoliciesProcedureRepository
    {
        public SysPoliciesProcedureRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}