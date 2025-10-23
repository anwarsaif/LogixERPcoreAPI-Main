
using Logix.Application.DTOs.Main;
using Logix.Application.DTOs.WF;
using Logix.Domain.Main;
using Logix.Domain.SAL;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysDynamicAttributeRepository : IGenericRepository<SysDynamicAttribute>
    {
        Task<SysDynamicAttribute> UpdateUsingProcedure(SysDynamicAttributeEditDto obj);
        Task<SysDynamicAttribute> RemoveUsingProcedure(long id);
        Task<List<DynamicAttributeResult>> GetDynamicAttributeData(long screenId, long appId);

    }
}
