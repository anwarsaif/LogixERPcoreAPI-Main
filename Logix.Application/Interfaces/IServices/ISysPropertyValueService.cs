using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysPropertyValueService : IGenericQueryService<SysPropertyValueDto, SysPropertyValuesVw>, IGenericWriteService<SysPropertyValueDto, SysPropertyValueDto>
    {
        Task<IResult> UpdatePropertyValue(long Id, string Value, CancellationToken cancellationToken = default);
        Task<IResult> SetPropertyValue(long PropertyId, long FacilityId, string Value, CancellationToken cancellationToken = default);

        Task<IResult<SysPropertyValueDto>> GetByProperty(long propertyId, long facilityId);
        Task<IResult<SysPropertyValuesVw>> GetByPropertyVw(long propertyId, long facilityId);
        Task<IResult<IEnumerable<SysPropertyValuesVw>>> GetAllVw();
		Task<IResult<List<SysPropertyValueFilterDto>>> Search(SysPropertyValueFilterDto filter, CancellationToken cancellationToken = default);

	}
}