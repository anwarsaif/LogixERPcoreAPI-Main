using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysDynamicValueRepository : GenericRepository<SysDynamicValue>, ISysDynamicValueRepository
    {
        public SysDynamicValueRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
    
}
