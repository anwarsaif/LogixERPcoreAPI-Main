using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysScreenWorkflowService : IGenericQueryService<SysScreenWorkflowDto, SysScreenWorkflow>, IGenericWriteService<SysScreenWorkflowDto, SysScreenWorkflowDto>
    {
        Task<IResult<IEnumerable<SysScreenWorkflowDto>>> GetScreenWorkflowByScreen(long screenId);
        Task<IResult<IEnumerable<DynamicAttributeDto>>> GetAttributes(long screenId, long? appTypeId, int? stepId = null);
        Task<IResult<IEnumerable<DynamicAttributeValueDto>>> GetAttributeValues(long screenId, long appId, long? appTypeId = null);
        Task<IResult<string?>> GetDefaultValue(string defaultValue, long? empId, string currDate, long? facilityId = 1, long? appId = null, long? finYear = null);
    }
}
