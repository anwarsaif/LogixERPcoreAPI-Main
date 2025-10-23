using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysScreenPermissionRepository : IGenericRepository<SysScreenPermission>
    {
        Task<SysScreenPermission> GetByScreenAndGroup(long screenId, int groupId);
        Task<List<UserPermissionSearchVm>> GetUserPermissionReport(UserPermissionSearchVm obj);
        Task<PaginatedResult<List<UserPermissionSearchVm>>> GetUserPermissionReportAsync(UserPermissionSearchVm obj, int take = Pagination.take, int? lastSeenId = null);

    }
}
