using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysCustomersFilesSettingService : IGenericQueryService<SysCustomersFilesSettingDto, SysCustomersFilesSettingsVw>, IGenericWriteService<SysCustomersFilesSettingDto, SysCustomersFilesSettingDto>
    {
    }

}


