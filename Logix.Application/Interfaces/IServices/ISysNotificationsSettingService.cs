using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysNotificationsSettingService : IGenericQueryService<SysNotificationsSettingDto, SysNotificationsSettingVw>, IGenericWriteService<SysNotificationsSettingDto, SysNotificationsSettingEditDto>
    {

    }
}
