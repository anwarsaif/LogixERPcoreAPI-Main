using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysCustomerGroupAccountService : IGenericQueryService<SysCustomerGroupAccountDto, SysCustomerGroupAccountsVw>, IGenericWriteService<SysCustomerGroupAccountDto, SysCustomerGroupAccountDto>
    {
        //Task<IResult<IEnumerable<SysCustomerGroupAccountsVw>>> GetAllVw();
    }
}
