using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysLookupCategoryService : IGenericQueryService<SysLookupCategoryDto, SysLookupCategory>, IGenericWriteService<SysLookupCategoryDto, SysLookupCategoryEditDto>
    {

    }
    //public interface ISysLookupCategoryService : IGenericService<SysLookupCategoryDto> 
    //{
    //    Task<IResult<SysLookupCategoryEditDto>> GetForUpdate(int Id, CancellationToken cancellationToken = default);
    //    Task<IResult<SysLookupCategoryEditDto>> Update(SysLookupCategoryEditDto entity, CancellationToken cancellationToken = default);
    //}
}
