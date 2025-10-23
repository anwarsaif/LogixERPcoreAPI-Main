using Logix.Application.DTOs.Main;
using Logix.Application.Helpers;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysWebHookService : IGenericQueryService<SysWebHookDto, SysWebHookVw>, IGenericWriteService<SysWebHookDto, SysWebHookEditDto>
    {
        Task<IResult<int>> SendToWebHook(string Id, long ScreenId, long UserId, long FacilityId, ProcessType PrType, CancellationToken cancellationToken = default);
    }
}
