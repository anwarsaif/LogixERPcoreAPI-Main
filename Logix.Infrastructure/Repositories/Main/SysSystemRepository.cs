using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.DTOs.RPT;
using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.ACC;
using Logix.Domain.Main;
using Logix.Domain.TS;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysSystemRepository : GenericRepository<SysSystem>, ISysSystemRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ICurrentData session;

        public SysSystemRepository(ApplicationDbContext context, ICurrentData session) : base(context)
        {
            this.context = context;
            this.session = session;
        }

        //public async Task<IEnumerable<MainListDto>> GetMainList(long userId, int sysId, int cmdType)
        //{
        //    var r = context.Set<MainListDto>().FromSqlRaw($"exec SYS_USER_SP @cmdType={cmdType}, @User_id={userId}, @system_id={sysId}").AsEnumerable().ToList();

        //    return r;


        //}

        public async Task<IEnumerable<ScreenSearchDto>> SearchScreen(string name, int groupId, int language)
        {
            /*var r1 = context.Database.ExecuteSqlRaw("select (case when {0}=1 then  SCREEN_NAME else SCREEN_NAME2 end) as SCREEN_NAME,SCREEN_NAME2,SCREEN_ID,Folder,SCREEN_URL, URL,Sys_Screen.IsCore  from Sys_Screen,Sys_System where Sys_System.System_Id=Sys_Screen.System_Id and Sys_System.ISDEL=0 and (SCREEN_NAME like N'%'+{1}+'%' or SCREEN_NAME2 like N'%'+{1}+'%') and Sys_Screen.isdel=0 and Sys_SCREEN.SCREEN_ID in (select SCREEN_ID from sys_screen_installed where isdeleted=0) and SCREEN_URL<>'#' and SCREEN_ID in (select SCREEN_ID from Sys_SCREEN_PERMISSION where GroupID = {2} and SCREEN_SHOW = 1)", language, name, groupId);
             var r = context.Set<ScreenSearchDto>().FromSqlRaw("select (case when {0}=1 then  SCREEN_NAME else SCREEN_NAME2 end) as SCREEN_NAME,SCREEN_NAME2,SCREEN_ID,Folder,SCREEN_URL, URL,Sys_Screen.IsCore  from Sys_Screen,Sys_System where Sys_System.System_Id=Sys_Screen.System_Id and Sys_System.ISDEL=0 and (SCREEN_NAME like N'%'+{1}+'%' or SCREEN_NAME2 like N'%'+{1}+'%') and Sys_Screen.isdel=0 and Sys_SCREEN.SCREEN_ID in (select SCREEN_ID from sys_screen_installed where isdeleted=0) and SCREEN_URL<>'#' and SCREEN_ID in (select SCREEN_ID from Sys_SCREEN_PERMISSION where GroupID = {2} and SCREEN_SHOW = 1)",language, name, groupId)
             .AsEnumerable().Select(s=> new ScreenSearchDto {
             Folder = s.Folder,
             IsCore = s.IsCore,
             SCREEN_ID = s.SCREEN_ID,
             SCREEN_NAME = s.SCREEN_NAME,
             SCREEN_NAME2 = s.SCREEN_NAME2,
             SCREEN_URL = s.SCREEN_URL,
             URL = s.URL,
             }).ToList();

             return r;*/

            var result = context.SysScreens
    .Join(context.SysSystems,
        screen => screen.SystemId,
        system => system.SystemId,
        (screen, system) => new { Screen = screen, System = system })
    .Where(joinResult => joinResult.System.Isdel == false &&
        (joinResult.Screen.ScreenName.Contains(name) || joinResult.Screen.ScreenName2.Contains(name)) &&
        joinResult.Screen.Isdel == false &&
        context.SysScreenInstalleds
            .Where(installed => installed.IsDeleted == false)
            .Select(installed => installed.ScreenId)
            .Contains(joinResult.Screen.ScreenId) &&
        joinResult.Screen.ScreenUrl != "#" &&
        context.SysScreenPermissions
            .Where(permission => permission.GroupId == groupId && permission.ScreenShow == true)
            .Select(permission => permission.ScreenId)
            .Contains(joinResult.Screen.ScreenId))
    .Select(joinResult => new ScreenSearchDto
    {
        ScreenName = language == 1 ? joinResult.Screen.ScreenName : joinResult.Screen.ScreenName2,
        ScreenName2 = joinResult.Screen.ScreenName2,
        ScreenId = joinResult.Screen.ScreenId,
        Folder = joinResult.System.Folder,
        ScreenUrl = joinResult.Screen.ScreenUrl,
        IsCore = joinResult.Screen.IsCore,
        IsAngular = joinResult.Screen.IsAngular
    })
    .ToList();


            return result;

        }

        //public async Task<IEnumerable<SubListDto>> GetSubList(long userId, int sysId, int cmdType, int parent)
        //{

        //    var r = context.Set<SubListDto>().FromSqlRaw($"exec SYS_USER_SP @cmdType={cmdType}, @User_id={userId}, @system_id={sysId}, @parent_id={parent}").AsEnumerable().ToList();

        //    return r;

        //}

        public async Task<IEnumerable<SysBranchVw>> GetSysBranchVw()
        {
            return await context.SysBranchVws.ToListAsync();
        }

        public async Task<IEnumerable<AccFacilitiesVw>> GetFacilities()
        {
            return await context.AccFacilitiesVws.ToListAsync();
        }

        public async Task<IEnumerable<AccFinancialYear>> GetFinancialYears(long facilityId)
        {
            return await context.AccFinancialYears.Where(f => f.FacilityId == facilityId && f.FinState == 1).ToListAsync();
        }
        public async Task<IEnumerable<AccFinancialYear>> GetFinancialYearsAll(long facilityId)
        {
            return await context.AccFinancialYears.Where(f => f.FacilityId == facilityId && f.IsDeleted == false).ToListAsync();
        }
        public async Task<IEnumerable<AccFinancialYear>> GetFinancialYears()
        {
            return await context.AccFinancialYears.ToListAsync();
        }
        public async Task<IEnumerable<TsTasksVw>> GetTasksByUser(long userId)
        {
            DateTime curDate = DateTime.Now; // Assuming you have the current date value

            var tasks = await context.TsTasksVws
                .Where(t => t.StatusId != 4 && t.StatusId != 5 && t.StatusId != 6
                    && t.AssigneeToUserId != null
                    && t.Isdel == false)

                .OrderBy(t => t.Priority)
                .ToListAsync();

            return tasks;
        }

        public async Task<List<MainListDto>> GetUserScreens(long userId, int sysId)
        {
            // Fetch user group IDs from SysUsers
            var userGroups = await context.SysUsers
                .Where(u => u.Id == userId)
                .Select(u => u.GroupsId)
                .FirstOrDefaultAsync();

            // Split the group IDs into a list
            var groupIds = userGroups.Split(',')
                .Select(int.Parse)
                .ToList();

            var screens = await context.SysScreens
                .Join(context.SysScreenPermissions,
                    screen => screen.ScreenId,
                    permission => permission.ScreenId,
                    (screen, permission) => new { screen, permission })
                .Join(context.SysSystems,
                    screenPermission => screenPermission.screen.SystemId,
                    system => system.SystemId,
                    (screenPermission, system) => new MainListDto
                    {
                        Icon_Css = screenPermission.screen.IconCss,
                        Color_Css = screenPermission.screen.ColorCss,
                        SCREEN_ID = screenPermission.screen.ScreenId,
                        SCREEN_NAME = screenPermission.screen.ScreenName,
                        SCREEN_NAME2 = screenPermission.screen.ScreenName2,
                        ISDEL = screenPermission.screen.Isdel,
                        System_Id = screenPermission.screen.SystemId,
                        Parent_Id = screenPermission.screen.ParentId,
                        Sort_no = screenPermission.screen.SortNo,
                        SCREEN_URL = screenPermission.screen.ScreenUrl,
                        PRIVE_ID = screenPermission.permission.PriveId,
                        USER_ID = screenPermission.permission.UserId,
                        SCREEN_SHOW = screenPermission.permission.ScreenShow,
                        SCREEN_ADD = screenPermission.permission.ScreenAdd,
                        SCREEN_EDIT = screenPermission.permission.ScreenEdit,
                        SCREEN_DELETE = screenPermission.permission.ScreenDelete,
                        SCREEN_PRINT = screenPermission.permission.ScreenPrint,
                        GroupID = screenPermission.permission.GroupId,
                        Cnt = context.SysScreens
                            .Where(s => s.ParentId == screenPermission.screen.ScreenId && s.Isdel == false && s.ParentId != s.ScreenId)
                            .Count(),
                        Folder = system.Folder,
                        URL = screenPermission.screen.Url,
                        IsCore = screenPermission.screen.IsCore,
                        IsAngular = screenPermission.screen.IsAngular,
                        SubScreens = new List<SubListDto>()
                    })
                .Where(sp => groupIds.Contains((int)sp.GroupID)
                             && sp.SCREEN_SHOW == true
                             && sp.Parent_Id == sp.SCREEN_ID
                             && sp.ISDEL == false
                && sp.System_Id == sysId)
                .Where(sp => context.SysScreenInstalleds
                             .Where(si => si.IsDeleted == false)
                             .Select(si => si.ScreenId)
                             .Contains(sp.SCREEN_ID))
                .OrderBy(sp => sp.Sort_no)
                .ToListAsync();

            foreach (var screen in screens)
            {
                screen.SubScreens = await context.SysScreens
                    .Join(context.SysScreenPermissions,
                        subscreen => subscreen.ScreenId,
                        permission => permission.ScreenId,
                        (subscreen, permission) => new { subscreen, permission })
                    .Join(context.SysSystems,
                        subscreenPermission => subscreenPermission.subscreen.SystemId,
                        system => system.SystemId,
                        (subscreenPermission, system) => new SubListDto
                        {
                            Icon_Css = subscreenPermission.subscreen.IconCss,
                            SCREEN_ID = subscreenPermission.subscreen.ScreenId,
                            SCREEN_NAME = subscreenPermission.subscreen.ScreenName,
                            SCREEN_NAME2 = subscreenPermission.subscreen.ScreenName2,
                            ISDEL = subscreenPermission.subscreen.Isdel,
                            System_Id = subscreenPermission.subscreen.SystemId,
                            Parent_Id = subscreenPermission.subscreen.ParentId,
                            Sort_no = subscreenPermission.subscreen.SortNo,
                            SCREEN_URL = subscreenPermission.subscreen.ScreenUrl,
                            PRIVE_ID = subscreenPermission.permission.PriveId,
                            USER_ID = subscreenPermission.permission.UserId,
                            SCREEN_SHOW = subscreenPermission.permission.ScreenShow,
                            SCREEN_ADD = subscreenPermission.permission.ScreenAdd,
                            SCREEN_EDIT = subscreenPermission.permission.ScreenEdit,
                            SCREEN_DELETE = subscreenPermission.permission.ScreenDelete,
                            SCREEN_PRINT = subscreenPermission.permission.ScreenPrint,
                            GroupID = subscreenPermission.permission.GroupId,
                            Cnt = context.SysScreens
                                .Where(s => s.ParentId == subscreenPermission.subscreen.ScreenId && s.Isdel == false && s.ParentId != s.ScreenId)
                                .Count(),
                            Folder = system.Folder,
                            URL = subscreenPermission.subscreen.ScreenUrl.StartsWith("/") ? subscreenPermission.subscreen.ScreenUrl : (system.Folder + "/" + subscreenPermission.subscreen.ScreenUrl),
                            IsCore = subscreenPermission.subscreen.IsCore,
                            IsAngular = subscreenPermission.subscreen.IsAngular
                        })
                    .Where(sp => groupIds.Contains((int)sp.GroupID)
                                 && sp.SCREEN_SHOW == true
                                 && sp.ISDEL == false
                                 && sp.System_Id == sysId
                                 && sp.Parent_Id == screen.SCREEN_ID
                                 && sp.Parent_Id != sp.SCREEN_ID)
                    .Where(sp => context.SysScreenInstalleds
                                 .Where(si => si.IsDeleted == false)
                                 .Select(si => si.ScreenId)
                                 .Contains(sp.SCREEN_ID))
                    .OrderBy(sp => sp.Sort_no)
                    .ToListAsync();
            }
            return screens;

        }
        public async Task<List<SysSystemDto>> GetSystemsToShowInHome()
        {
            try
            {
                var groupId = session.GroupId;

                var systems = await (from s in context.SysSystems
                                     join sp in context.SysScreenPermissionVws
                                     on s.SystemId equals sp.SystemId
                                     where s.ShowInHome == true
                                     && s.Isdel == false
                                     && sp.Isdel == false
                                     && sp.ScreenShow == true
                                     && sp.GroupId == groupId
                                     orderby s.SysSort
                                     select new SysSystemDto
                                     {
                                         SystemId = s.SystemId,
                                         SystemName = s.SystemName,
                                         SystemName2 = s.SystemName2,
                                         ShowInHome = s.ShowInHome,
                                         SysSort = s.SysSort,
                                         ColorCss = s.ColorCss,
                                         Desc1 = s.Desc1,
                                         Desc2 = s.Desc2,
                                         Folder = s.Folder,
                                         Action = s.Action,
                                         Area = s.Area,
                                         Controller = s.Controller,
                                         DefaultPage = s.DefaultPage,
                                         IconCss = s.IconCss,
                                         IsCore = s.IsCore,
                                         ShortName = s.ShortName,
                                         ShortName2 = s.ShortName2,
                                         IsAngular = s.IsAngular,
                                         Isdel = s.Isdel
                                     }).Distinct().OrderBy(x => x.SysSort).ToListAsync();

                return systems;
            }
            catch (Exception ex)
            {
                // Log the exception (Logging framework or custom logging solution can be used here)
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Return an empty list if an exception occurs
                return new List<SysSystemDto>();
            }
        }
        public async Task<List<RptReportDto>> GetCustomReportMenu(long systemId, string sysGroupId)
        {
            // أولاً: جلب التقارير المطابقة لنظام محدد والتي لم تُحذف
            var allReports = await context.RptReports
                .Where(r => r.IsDeleted == false && r.SystemId == systemId && r.SysGroupId != null)
                .ToListAsync();

            // ثانياً: تصفية النتائج في الذاكرة بناءً على تطابق sysGroupId
            var filtered = allReports
                .Where(r => r.SysGroupId.Split(',').Contains(sysGroupId))
                .Select(r => new RptReportDto
                {
                    Id = r.Id,
                    ReportName = r.ReportName,
                    ReportName2 = r.ReportName2,
                    SystemId = r.SystemId,
                    SysGroupId = r.SysGroupId,
                })
                .ToList();

            return filtered;
        }



    }
}