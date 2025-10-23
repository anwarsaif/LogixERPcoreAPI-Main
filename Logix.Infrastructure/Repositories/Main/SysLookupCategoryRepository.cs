using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysLookupCategoryRepository : GenericRepository<SysLookupCategory>, ISysLookupCategoryRepository
    {
        public SysLookupCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
