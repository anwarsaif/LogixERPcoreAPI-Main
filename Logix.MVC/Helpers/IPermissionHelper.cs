using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IServices;
using Logix.Application.Wrapper;

namespace Logix.MVC.Helpers
{
    public interface IPermissionHelper
    {
        Task<bool> HasPermission(long screenId, PermissionType permissionType);
        Task<IResult<SysScreenPermissionDto>> ScreenAllTPermission(long screenId);
    }

    public class PermissionHelper : IPermissionHelper
    {
        //private readonly ISession _session;
        private readonly IMainServiceManager serviceManager;
        private readonly ICurrentData currentData;

        public PermissionHelper(IMainServiceManager serviceManager, /*IHttpContextAccessor httpContextAccessor,*/ ICurrentData currentData)
        {
            //_session = httpContextAccessor.HttpContext.Session;
            this.serviceManager = serviceManager;
            this.currentData = currentData;
        }

        public async Task<bool> HasPermission(long screenId, PermissionType permissionType)
        {
            try
            {
                //var user = _session.GetData<SysUser>("user");
                //if (user == null)
                //{
                //    return false;
                //}

                //if (string.IsNullOrEmpty(user.GroupsId))
                //{
                //    return false;
                //}
                //int groupId;
                //var hasGroup = int.TryParse(user.GroupsId, out groupId);
                //if (!hasGroup || groupId == 0)
                //{
                //    return false;
                //}

                int groupId = currentData.GroupId;
                if(!(groupId > 0))
                    return false;

                var getPerm = await serviceManager.SysScreenPermissionService.GetByScreenAndGroup(screenId, groupId);
                if (!getPerm.Succeeded)
                {
                    return false;
                }

                switch (permissionType)
                {
                    case PermissionType.Add: return getPerm.Data.ScreenAdd ?? false;
                    case PermissionType.Edit: return getPerm.Data.ScreenEdit ?? false;
                    case PermissionType.Delete: return getPerm.Data.ScreenDelete ?? false;
                    case PermissionType.Show: return getPerm.Data.ScreenShow ?? false;
                    case PermissionType.Print: return getPerm.Data.ScreenPrint ?? false;

                    case PermissionType.Export: return getPerm.Data.ScreenExport ?? false;
                    case PermissionType.Import: return getPerm.Data.ScreenImport ?? false;
                    case PermissionType.Approval: return getPerm.Data.ScreenApproval ?? false;
                    case PermissionType.Reject: return getPerm.Data.ScreenReject ?? false;
                    case PermissionType.View: return getPerm.Data.ScreenView ?? false;
                    default: return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<IResult<SysScreenPermissionDto>> ScreenAllTPermission(long screenId)
        {
            try
            {
                //var user = _session.GetData<SysUser>("user");
                //if (user == null)
                //{
                //    return Result<SysScreenPermissionDto>.Fail($"--- there is no Data with this screen {screenId}");
                //}

                //if (string.IsNullOrEmpty(user.GroupsId))
                //{
                //    return Result<SysScreenPermissionDto>.Fail($"--- there is no Data with this screen {screenId}");
                //}
                //int groupId;
                //var hasGroup = int.TryParse(user.GroupsId, out groupId);
                //if (!hasGroup || groupId == 0)
                //{
                //    return Result<SysScreenPermissionDto>.Fail($"--- there is no Data with this screen {screenId} and group: {groupId}---");
                //}

                int groupId = currentData.GroupId;
                if (!(groupId > 0))
                    return Result<SysScreenPermissionDto>.Fail($"--- there is no Data with this screen {screenId} and group: {groupId}---");

                var getPerm = await serviceManager.SysScreenPermissionService.GetByScreenAndGroup(screenId, groupId);
                if (!getPerm.Succeeded)
                {
                    return Result<SysScreenPermissionDto>.Fail($"--- there is no Data with this screen {screenId} and group: {groupId}---");
                }

                return getPerm;
            }
            catch
            {
                return Result<SysScreenPermissionDto>.Fail($"--- there is no Data with this screen {screenId}, exeption");
            }
        }
    }
}
