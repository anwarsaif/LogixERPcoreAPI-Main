using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysActivityLogService : IGenericQueryService<SysActivityLogDto, SysActivityLogVw>, IGenericWriteService<SysActivityLogDto, SysActivityLogDto>
    {

    }
}
