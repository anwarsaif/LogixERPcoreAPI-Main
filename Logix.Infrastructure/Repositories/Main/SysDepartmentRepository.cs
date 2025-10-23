using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysDepartmentRepository : GenericRepository<SysDepartment>, ISysDepartmentRepository
    {
        public SysDepartmentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
