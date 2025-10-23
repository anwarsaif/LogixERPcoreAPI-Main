using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysDynamicValueService : IGenericQueryService<SysDynamicValueDto, SysDynamicValue>, IGenericWriteService<SysDynamicValueDto, SysDynamicValueEditDto>
    {
    }

}


