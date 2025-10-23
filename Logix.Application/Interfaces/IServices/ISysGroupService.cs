using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysGroupService : IGenericQueryService<SysGroupDto, SysGroupVw>, IGenericWriteService<SysGroupDto, SysGroupEditDto>
    {
        Task<IResult<CopyGroupVM>> Copy(CopyGroupVM entity, CancellationToken cancellationToken = default);
		Task<IResult<List<SysGroupFilterDto>>> Search(SysGroupFilterDto filter, CancellationToken cancellationToken = default);

	}
}
