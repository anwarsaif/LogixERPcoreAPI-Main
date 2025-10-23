using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    ////الطريقة القديمة
    //public interface ISysScreenPermissionService : IGenericService<SysScreenPermissionDto>
    //{
    //    Task<IResult<SysScreenPermissionDto>> GetByScreenAndGroup(long screenId, int groupId, CancellationToken cancellationToken = default);
    //}

    public interface ISysScreenPermissionService : IGenericQueryService<SysScreenPermissionDto, SysScreenPermissionVw>, IGenericWriteService<SysScreenPermissionDto, SysScreenPermissionDto>
    {
        Task<IResult<SysScreenPermissionDto>> GetByScreenAndGroup(long screenId, int groupId, CancellationToken cancellationToken = default);

        Task<IResult<IEnumerable<SysScreenPermissionDtoVM>>> Update(IEnumerable<SysScreenPermissionDtoVM> entities, CancellationToken cancellationToken = default);

        Task<IResult<List<UserPermissionSearchVm>>> GetUserPermissionReport(UserPermissionSearchVm entity, CancellationToken cancellationToken = default);
        Task<PaginatedResult<List<UserPermissionSearchVm>>> GetUserPermissionReportAsync(UserPermissionSearchVm obj, int take = Pagination.take, int? lastSeenId = null);



    }
}
