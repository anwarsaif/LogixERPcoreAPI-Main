using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysCountryService : IGenericQueryService<SysCountryDto, SysCountryVw>, IGenericWriteService<SysCountryDto, SysCountryEditDto>
    {
    }
}
