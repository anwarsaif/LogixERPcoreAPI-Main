using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysPackageRepository : GenericRepository<SysPackage>, ISysPackageRepository
    {
        public SysPackageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}