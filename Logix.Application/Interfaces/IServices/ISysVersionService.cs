using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysVersionService : IGenericQueryService<SysVersionDto, SysVersion>, IGenericWriteService<SysVersionDto, SysVersionEditDto>
    {
    }

}


