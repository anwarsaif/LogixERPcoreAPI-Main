using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysFavMenuRepository : GenericRepository<SysFavMenu>, ISysFavMenuRepository
    {
        public SysFavMenuRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}