using Logix.Application.Common;
using Logix.Application.DTOs.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysLookupDataService : IGenericQueryService<SysLookupDataDto, SysLookupDataVw>, IGenericWriteService<SysLookupDataDto, SysLookupDataDto>
    {
        Task<IResult<IEnumerable<DDItem>>> GetDataByCategory(int categoryId, int lang = 0);
    }
}
