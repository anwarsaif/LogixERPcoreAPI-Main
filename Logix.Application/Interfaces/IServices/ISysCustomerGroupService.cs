using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysCustomerGroupService: IGenericQueryService<SysCustomerGroupDto, SysCustomerGroup>, IGenericWriteService<SysCustomerGroupDto, SysCustomerGroupEditDto>
    {
    }
}
