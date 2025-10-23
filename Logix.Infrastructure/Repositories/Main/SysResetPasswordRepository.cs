using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysResetPasswordRepository : GenericRepository<SysResetPassword>, ISysResetPasswordRepository
    {
        public SysResetPasswordRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
    
}
