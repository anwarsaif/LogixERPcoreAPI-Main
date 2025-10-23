using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysCustomerBranchService : IGenericQueryService<SysCustomerBranchDto, SysCustomerBranchVw>, IGenericWriteService<SysCustomerBranchDto, SysCustomerBranchEditDto>
    {
    }
}
