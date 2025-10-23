using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysScreenPropertyService : IGenericQueryService<SysScreenPropertyDto, SysScreenProperty>, IGenericWriteService<SysScreenPropertyDto, SysScreenPropertyDto>
    {
 
    }
}
