using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysScreenInstalledService : IGenericQueryService<SysScreenInstalledDto, SysScreenInstalledVw>, IGenericWriteService<SysScreenInstalledDto, SysScreenInstalledDto>
    {

    }
}
