using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysGroupRepository : GenericRepository<SysGroup,SysGroupVw>, ISysGroupRepository
    {
        public SysGroupRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
