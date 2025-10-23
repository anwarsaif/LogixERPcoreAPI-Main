using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysNotificationService : IGenericQueryService<SysNotificationDto, SysNotificationsVw>, IGenericWriteService<SysNotificationDto, SysNotificationDto>
    {
        Task<IResult<IEnumerable<SysNotificationsVw>>> GetTopVw(CancellationToken cancellationToken = default);
        Task<IResult> ReadNotification(long Id, CancellationToken cancellationToken = default);
        Task<IResult> ReadAllNotifications(string Ids, CancellationToken cancellationToken = default);
    }
}