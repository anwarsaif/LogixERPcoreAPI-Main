using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
	public interface ISysPeriodService : IGenericQueryService<SysPeriodDto, SysPeriod>, IGenericWriteService<SysPeriodDto, SysPeriodEditDto>
    {
        Task<IResult<int>> IsDateRangeOverlap(long Id, string StartDate, string EndDate, long SystemId, long FacilityId, CancellationToken cancellationToken = default);
    }
}
