using Logix.Application.DTOs.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysSmsService : IGenericQueryService<SysSmsDto>, IGenericWriteService<SysSmsDto, SysSmsDto>
    {

    }
}
