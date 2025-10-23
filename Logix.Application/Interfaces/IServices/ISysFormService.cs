using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysFormService : IGenericQueryService<SysFormDto, SysFormsVw>, IGenericWriteService<SysFormDto, SysFormEditDto>
    {

    }

}
