using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysNotificationRepository : GenericRepository<SysNotification>, ISysNotificationRepository
    {
        private readonly ApplicationDbContext context;

        public SysNotificationRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<SysNotificationsVw>> GetTop(Expression<Func<SysNotificationsVw, bool>> expression)
        {
            return await context.SysNotificationsVws.Where(expression).Take(10).ToListAsync();
        }
    }
}
