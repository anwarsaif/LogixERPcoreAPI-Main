using Logix.Application.DTOs.HR;
using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysDepartmentService : IGenericQueryService<SysDepartmentDto, SysDepartmentVw>, IGenericWriteService<SysDepartmentDto, SysDepartmentEditDto>
    {
        Task<IResult<List<string>>> GetchildDepartment(long DeptId, CancellationToken cancellationToken = default);

    }
}
