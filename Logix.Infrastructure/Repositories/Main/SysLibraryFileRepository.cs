using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysLibraryFileRepository : GenericRepository<SysLibraryFile>, ISysLibraryFileRepository
    {
        private readonly ApplicationDbContext context;

        public SysLibraryFileRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }    
}
