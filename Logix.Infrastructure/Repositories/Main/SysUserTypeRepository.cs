using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysUserTypeRepository : GenericRepository<SysUserType>, ISysUserTypeRepository
    {
        public SysUserTypeRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
    
}
