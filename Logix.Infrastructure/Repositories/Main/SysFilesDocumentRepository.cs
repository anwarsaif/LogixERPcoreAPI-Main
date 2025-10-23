using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysFilesDocumentRepository : GenericRepository<SysFilesDocument>, ISysFilesDocumentRepository
    {
        public SysFilesDocumentRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}