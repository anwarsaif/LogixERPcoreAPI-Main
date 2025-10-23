using DevExpress.XtraReports.UI;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IServices;
using Logix.MVC.Helpers;
using Logix.MVC.Reports.Main.DevReports;
using Logix.MVC.Reports.Main.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Logix.MVC.Reports.Main.Controllers
{
    [Area("Main")]
    public class DevReportController : Controller
    {
        private readonly IDevReportHelper devReportHelper;
        private readonly ICurrentData session;
        private readonly IFxaServiceManager fxaServiceManager;
        private readonly IMainServiceManager mainServiceManager;

        public DevReportController(IDevReportHelper devReportHelper,
            ICurrentData session,
            IFxaServiceManager fxaServiceManager,
            IMainServiceManager mainServiceManager)
        {
            this.devReportHelper = devReportHelper;
            this.session = session;
            this.fxaServiceManager = fxaServiceManager;
            this.mainServiceManager = mainServiceManager;
        }

        [HttpGet]
        public async Task<IActionResult> DevReport_Viewer(long Rep_ID, long Cus_RP_ID, long T_ID, string Filters, string ChkIds)
        {
            try
            {
                // make session for data we need it in view and design report
                devReportHelper.InitSession();
                //session.AddData<string>("filters", filters);

                // first call the SetCustomReportPath function to get the custom report data
                var setRptPath = await devReportHelper.SetCustomReportPath("Main", Rep_ID, Cus_RP_ID);
                if (!setRptPath.Succeeded)
                {
                    ViewData["RptException"] = setRptPath.Status.message;
                    return View("~/Views/_ReportViewerError.cshtml");
                }
                ViewData["rptCustomReportAddDto"] = setRptPath.Data; // RptCustomReport data

                // convert filters from json to dictionary, then use it in get source data
                var filtersDictionary = devReportHelper.DecodeFilterToDictionary(Filters);

                var basicData = new ReportBasicDataDto();
                XtraReport myReport = new(); object myDataSource = new(); bool IsAutoDirection = true;
                switch (Rep_ID)
                {
                    case 1:
                        basicData = await devReportHelper.GetBasicData("تقرير الدخول للمستخدمين", "User Login Report");
                        var loginsFilter = GetLoginsFilter(filtersDictionary);
                        var getLogins = await mainServiceManager.SysUserService.GetUsersLoginsRpt(loginsFilter);
                        myReport = new RptUsersLogin();
                        myDataSource = new UsersLoginDS() { BasicData = basicData, Filter = loginsFilter, Details = getLogins.Data.ToList() };
                        break;
                    case 2://تقرير الصلاحيات للمستخدمين screenId = 2149
                        basicData = await devReportHelper.GetBasicData("تقرير الصلاحيات للمستخدمين", "Users Permissions Report");
                        var filter = GetLoginsFilter(filtersDictionary);
                        var getUserPermission = await mainServiceManager.SysUserService.GetUsersPermissionsRpt(filter);
                        myReport = new RptUsersPermissions();
                        myDataSource = new UsersLoginDS() { BasicData = basicData, Filter = filter, Details = getUserPermission.Data.ToList() };
                        break;
                    case 3://User Account
                        basicData = await devReportHelper.GetBasicData("المستخدمين والصلاحيات", "Users Accounts");
                        var filter3 = GetUserAccount(filtersDictionary);
                        var getUserAccount = await mainServiceManager.SysUserService.Search(filter3);
                        myReport = new RptUserAccount();
                        myDataSource = new PermitionDS() { BasicData = basicData, Filter = filter3, Details = getUserAccount.Data.ToList() };
                        break;
                    case 4:
                        basicData = await devReportHelper.GetBasicData("المجموعات والصلاحيات", "Groups & Permissions");
                        var filter4 = GetGroupsPermasition(filtersDictionary);
                        var getGroupPer = await mainServiceManager.SysGroupService.Search(filter4);
                        myReport = new RptGroupsPermasition();
                        myDataSource = new GroupsPermasitionDS() { BasicData = basicData, Filter = filter4, Details = getGroupPer.Data.ToList() };
                        break;
                    case 5:
                        basicData = await devReportHelper.GetBasicData("تقرير بتتبع وصول المستخدمين", "User access tracking");
                        var filter5 = GetUserTracking(filtersDictionary);
                        var getUserTracking = await mainServiceManager.SysUserTrackingService.GetUserTrackingRp(filter5);
                        myReport = new RptUserTracking();
                        myDataSource = new UserTrackingDS() { BasicData = basicData, Filter = filter5, Details = getUserTracking.Data.ToList() };
                        break;
                    case 6:
                        basicData = await devReportHelper.GetBasicData("إعدادات النظام", "System Configration");
                        var filter6 = GetConfigration(filtersDictionary);
                        var getConfigration = await mainServiceManager.SysPropertyValueService.Search(filter6);
                        myReport = new RptConfigration();
                        myDataSource = new ConfigrationDS() { BasicData = basicData, Filter = filter6, Details = getConfigration.Data.ToList() };
                        break;
                    case 7://تقرير صلاحيات مستخدم screenId = 1238
                        basicData = await devReportHelper.GetBasicData("تقرير صلاحيات مستخدم", "Report User Permissions");
                        var filter7 = GetUsersPermissions(filtersDictionary);
                        var getUserPermissions = await mainServiceManager.SysScreenPermissionService.GetUserPermissionReport(filter7);
                        myReport = new RptUserPermation();
                        myDataSource = new UsersPermissionsDS() { BasicData = basicData, Filter = filter7, Details = getUserPermissions.Data.ToList() };
                        break;
                    default:
                        ViewData["RptException"] = "Invalid reportId";
                        return View("~/Views/_ReportViewerError.cshtml");
                }
                var openReport = await devReportHelper.OpenReport(myReport, myDataSource, IsAutoDirection);
                if (openReport.Succeeded)
                {
                    ViewData["ReportName"] = openReport.Data;
                    return View("~/Views/_ReportViewer.cshtml");
                }
                else
                {
                    ViewData["RptException"] = openReport.Status.message;
                    return View("~/Views/_ReportViewerError.cshtml");
                }
            }
            catch (Exception exp)
            {
                ViewData["RptException"] = exp.Message;
                return View("~/Views/_ReportViewerError.cshtml");
            }
        }

        [NonAction]
        private UserPermissionSearchVm GetUsersPermissions(Dictionary<string, string> dictionary)
        {
            return new UserPermissionSearchVm()
            {
                UserId = Convert.ToInt32(dictionary["UserId"]),
                SystemId = Convert.ToInt32(dictionary["SystemId"]),
            };
        }

        [NonAction]
        private SysUserFilterDto GetUserAccount(Dictionary<string, string> dictionary)
        {
            return new SysUserFilterDto()
            {
                BranchId = Convert.ToInt64(dictionary["BranchId"]),
                UserFullname = dictionary["UserFullname"],
                Enable = Convert.ToInt32(dictionary["Enable"]),
                GroupsId = dictionary["GroupsId"],
                EmpCode = dictionary["EmpCode"],
                FacilityId = Convert.ToInt64(dictionary["FacilityId"]),
                UserTypeId = Convert.ToInt32(dictionary["UserTypeId"]),
                UserName = dictionary["UserName"],
            };
        }

        [NonAction]
        private SysGroupFilterDto GetGroupsPermasition(Dictionary<string, string> dictionary)
        {
            return new SysGroupFilterDto()
            {
                SystemId = Convert.ToInt32(dictionary["SystemId"]),
                GroupName = dictionary["GroupName"],
                GroupName2 = dictionary["GroupName2"],
            };
        }

        [NonAction]
        private SysUserTrackingFilterDto GetUserTracking(Dictionary<string, string> dictionary)
        {
            return new SysUserTrackingFilterDto()
            {
                ActivityDateFrom = dictionary["ActivityDateFrom"],
                ActivityDateTo = dictionary["ActivityDateTo"],
                UserId = Convert.ToInt32(dictionary["UserId"]),
            };
        }

        [NonAction]
        private SysPropertyValueFilterDto GetConfigration(Dictionary<string, string> dictionary)
        {
            return new SysPropertyValueFilterDto()
            {
                PropertyName = dictionary["PropertyName"],
                PropertyId = Convert.ToInt64(dictionary["PropertyId"]),
                SystemId = Convert.ToInt32(dictionary["SystemId"]),
                ClassificationsId = Convert.ToInt64(dictionary["ClassificationsId"]),
                IsRequired = Convert.ToBoolean(dictionary["IsRequired"]),
                IsEmptyValue = Convert.ToBoolean(dictionary["IsEmptyValue"]),
            };
        }

        [NonAction]
        private SysUsersLoginsVm GetLoginsFilter(Dictionary<string, string> dictionary)
        {
            return new SysUsersLoginsVm()
            {
                UserId = Convert.ToInt64(dictionary["UserId"]),
                GroupId = Convert.ToInt64(dictionary["GroupId"]),
                EmpCode = dictionary["EmpCode"],
                StartDate = dictionary["StartDate"],
                EndDate = dictionary["EndDate"]
            };
        }
    }
}
