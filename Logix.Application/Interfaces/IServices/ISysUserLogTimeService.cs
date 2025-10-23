using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysUserLogTimeService : IGenericQueryService<SysUserLogTimeDto, SysUserLogTimeVw>, IGenericWriteService<SysUserLogTimeDto, SysUserLogTimeDto>
    {
        Task<IResult<IEnumerable<SysUserTypeDto>>> GetAllUserTypes();
    }
}
