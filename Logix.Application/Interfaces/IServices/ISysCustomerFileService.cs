using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysCustomerFileService : IGenericQueryService<SysCustomerFileDto, SysCustomerFile>, IGenericWriteService<SysCustomerFileDto, SysCustomerFileDto>
    {
    }

}
