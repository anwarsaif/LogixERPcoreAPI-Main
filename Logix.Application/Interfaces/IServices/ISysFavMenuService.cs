using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysFavMenuService : IGenericQueryService<SysFavMenuDto>, IGenericWriteService<SysFavMenuDto, SysFavMenuDto>
    {
    }
}