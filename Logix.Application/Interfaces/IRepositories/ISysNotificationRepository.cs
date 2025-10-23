using Logix.Domain.Main;
using System.Linq.Expressions;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysNotificationRepository : IGenericRepository<SysNotification>
    {
        Task<IEnumerable<SysNotificationsVw>> GetTop(Expression<Func<SysNotificationsVw, bool>> expression);
    }
}
