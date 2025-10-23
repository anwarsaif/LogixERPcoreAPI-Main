using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysMailServerService : IGenericQueryService<SysMailServerDto, SysMailServer>, IGenericWriteService<SysMailServerDto, SysMailServerDto>
    {
    }
}
