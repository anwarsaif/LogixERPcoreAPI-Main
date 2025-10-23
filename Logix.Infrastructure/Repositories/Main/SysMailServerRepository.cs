using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysMailServerRepository : GenericRepository<SysMailServer>, ISysMailServerRepository
    {
        private readonly ApplicationDbContext context;

        public SysMailServerRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

       
    }
}
