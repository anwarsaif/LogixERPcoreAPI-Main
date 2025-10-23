using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysCustomerContactService : IGenericQueryService<SysCustomerContactDto, SysCustomerContact>, IGenericWriteService<SysCustomerContactDto, SysCustomerContactDto>
    {
    }

}
