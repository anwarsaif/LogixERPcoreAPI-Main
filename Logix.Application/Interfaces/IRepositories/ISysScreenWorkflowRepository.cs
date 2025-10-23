using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysScreenWorkflowRepository : IGenericRepository<SysScreenWorkflow>
    {
        Task<IEnumerable<SysScreenWorkflowDto>> GetScreenWorkflowByScreen(long ScreenID);
        Task<IEnumerable<DynamicAttributeDto>> GetAttributes(long screenId, long? appTypeId, int? stepId = null);
        Task<IEnumerable<DynamicAttributeValueDto>> GetAttributeValues(long screenId, long appId, long? appTypeId = null);
        Task<string?> GetDefaultValue(string defaultValue, long? empId, string currDate, long? facilityId = 1, long? appId = null, long? finYear = null);

    }
}
