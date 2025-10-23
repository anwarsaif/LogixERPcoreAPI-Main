using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysCitesService : IGenericQueryService<SysCitesDto, SysCites>, IGenericWriteService<SysCitesDto, SysCitesEditDto>
    {
    }

}


