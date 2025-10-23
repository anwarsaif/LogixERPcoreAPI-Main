using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysVatGroupService : IGenericQueryService<SysVatGroupDto, SysVatGroupVw>, IGenericWriteService<SysVatGroupDto, SysVatGroupEditDto>
    {

    }
}
