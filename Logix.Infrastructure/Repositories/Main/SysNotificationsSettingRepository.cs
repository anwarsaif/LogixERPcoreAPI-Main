using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysNotificationsSettingRepository : GenericRepository<SysNotificationsSetting>, ISysNotificationsSettingRepository
    {
        private readonly ApplicationDbContext _context;
        public SysNotificationsSettingRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }
    }
    
}
