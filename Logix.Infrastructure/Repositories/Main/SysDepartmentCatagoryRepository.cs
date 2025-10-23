using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysDepartmentCatagoryRepository : GenericRepository<SysDepartmentCatagory>, ISysDepartmentCatagoryRepository
    {
        public SysDepartmentCatagoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
