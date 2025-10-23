using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysDynamicAttributeService : IGenericQueryService<SysDynamicAttributeDto, SysDynamicAttributesVw>, IGenericWriteService<SysDynamicAttributeDto, SysDynamicAttributeEditDto>
    {
        Task<IResult<IEnumerable<SysDynamicAttributeDataTypeDto>>> GetAttributeTypes(CancellationToken cancellationToken = default);
    }
}
