using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysCustomersFilesSettingRepository : GenericRepository<SysCustomersFilesSetting>, ISysCustomersFilesSettingRepository
    {
        public SysCustomersFilesSettingRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
