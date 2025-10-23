using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using static Logix.Application.Services.Main.SysUserTrackingService;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysUserTrackingService : IGenericQueryService<SysUserTrackingDto, SysUserTrackingVw>, IGenericWriteService<SysUserTrackingDto, SysUserTracking>
    {
		Task<IResult<List<SysUserTrackingVm>>> GetUserTrackingRp(SysUserTrackingFilterDto filter, CancellationToken cancellationToken = default);

	}
}
