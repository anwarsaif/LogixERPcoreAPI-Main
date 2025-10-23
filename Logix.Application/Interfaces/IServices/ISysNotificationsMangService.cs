using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysNotificationsMangService : IGenericQueryService<SysNotificationsMangDto, SysNotificationsMangVw>, IGenericWriteService<SysNotificationsMangDto, SysNotificationsMangEditDto>
    {
        Task<IResult<List<SysNotificationsMangResultDto>>> GetNotificationsByUserAndGroupAsync();

    }
}
