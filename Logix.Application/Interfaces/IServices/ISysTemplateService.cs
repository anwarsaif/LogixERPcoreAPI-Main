using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysTemplateService : IGenericQueryService<SysTemplateDto, SysTemplateVw>, IGenericWriteService<SysTemplateDto, SysTemplateEditDto>
    {
        Task<string> ReplaceDate(string document, int typeId, long empId,int TemplateId);

    }
}