using DevExpress.XtraReports.UI;
using Logix.Application.Common;
using Logix.Application.DTOs.RPT;
using Logix.Application.Interfaces.IServices;
using Logix.Application.Wrapper;
using Logix.MVC.Reports;
using Newtonsoft.Json;
using System.Reflection;
using System.Web;

namespace Logix.MVC.Helpers
{
    public interface IDevReportHelper
    {
        void InitSession();
        Task<ReportBasicDataDto> GetBasicData(string ReportNameArabic, string ReportNameEnglish);
        Dictionary<string, string> DecodeFilterToDictionary(string data);
        Task<IResult<RptCustomReportAddDto>> SetCustomReportPath(string SystemName, long Rep_ID, long Cus_RP_ID);
        Task<IResult<XtraReport>> OpenReport(XtraReport Report, object dataSource, bool IsAutoDirection = true);
    }

    public class DevReportHelper : IDevReportHelper
    {
        private readonly ICurrentData session;
        private readonly IMvcSession mvcSession;
        private readonly IAccServiceManager accServiceManager;
        private readonly IMainServiceManager mainServiceManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISysConfigurationHelper configurationHelper;
        private readonly IRptServiceManager rptServiceManager;
        private readonly IWebHostEnvironment _env;

        public DevReportHelper(ICurrentData session,
            IMvcSession mvcSession,
            IAccServiceManager accServiceManager,
            IMainServiceManager mainServiceManager,
            IHttpContextAccessor httpContextAccessor,
            ISysConfigurationHelper configurationHelper,
            IRptServiceManager rptServiceManager,
            IWebHostEnvironment env)
        {
            this.session = session;
            this.mvcSession = mvcSession;
            this.accServiceManager = accServiceManager;
            this.mainServiceManager = mainServiceManager;
            _httpContextAccessor = httpContextAccessor;
            this.configurationHelper = configurationHelper;
            this.rptServiceManager = rptServiceManager;
            _env = env;
        }

        public void InitSession()
        {
            mvcSession.AddData<string>("CustomReportPath", "");
            mvcSession.AddData<string>("AllowedFiles", "");
            mvcSession.AddData<long>("ReportScreenID", 0);
            mvcSession.AddData<long>("Report_ID", 0);
            mvcSession.AddData<long>("Cus_RP_ID", 0);
            mvcSession.AddData<string>("Report_Link", "");
            mvcSession.AddData<bool>("IsMain", false);
            mvcSession.AddData<bool>("IsPermission_Edit", false);
            mvcSession.AddData<bool>("BtnEditVisible", false);
            mvcSession.AddData<string>("ReportLayout", "");
            mvcSession.AddData<string>("ReportObjectName", "");
            mvcSession.AddData<int>("DDltypeValue", 0);
            mvcSession.AddData<string>("filters", "");
        }

        public async Task<ReportBasicDataDto> GetBasicData(string ReportNameArabic, string ReportNameEnglish)
        {
            try
            {
                var lang = session.Language;
                var getFacility = await accServiceManager.AccFacilityService.GetOne(x => x.FacilityId == session.FacilityId);
                var facilityData = getFacility.Data;
                ReportBasicDataDto basicData = new();
                basicData.ReportNameArabic = ReportNameArabic;
                basicData.ReportNameEnglish = ReportNameEnglish;
                basicData.ReportName = lang == 1 ? basicData.ReportNameArabic : basicData.ReportNameEnglish;
                basicData.ReportFullName = basicData.ReportNameEnglish + " - " + basicData.ReportNameArabic;

                basicData.FinanceYearHijri = session.FinyearGregorian.ToString();
                basicData.FinanceYearGregorian = session.FinyearGregorian.ToString();
                basicData.CompanyNameArabic = facilityData.FacilityName;
                basicData.CompanyPhone = facilityData.FacilityPhone;
                basicData.CompanyAddress = facilityData.FacilityAddress;
                basicData.CompanyNameEnglish = string.IsNullOrEmpty(facilityData.FacilityName2) ? facilityData.FacilityName : facilityData.FacilityName2;
                basicData.CompanyName = lang == 1 ? basicData.CompanyNameArabic : basicData.CompanyNameEnglish;

                var Request = _httpContextAccessor.HttpContext?.Request;
                var baseUrl = Request != null ? $"{Request.Scheme}://{Request.Host}" : "";
                basicData.LogoPrint = baseUrl + "/" + facilityData.LogoPrint;
                basicData.CompanyLogo = baseUrl + "/" + facilityData.FacilityLogo;
                basicData.ImgFooter = baseUrl + "/" + facilityData.ImgFooter;

                var getEmp = await mainServiceManager.InvestEmployeeService.GetById((long)session.EmpId);
                if (getEmp.Succeeded)
                {
                    basicData.FullNameArabic = getEmp.Data.EmpName;
                    basicData.FullNameEnglish = getEmp.Data.EmpName2 ?? getEmp.Data.EmpName;
                    basicData.FullName = lang == 1 ? basicData.FullNameArabic : basicData.FullNameEnglish;
                }

                return basicData;
            }
            catch
            {
                return new ReportBasicDataDto();
            }
        }

        //public Dictionary<string, string> DecodeFilterToDictionary(string data)
        //{
        //    if (string.IsNullOrEmpty(data))
        //        return new Dictionary<string, string>();
        //    string decodedData = HttpUtility.UrlDecode(data);
        //    return JsonConvert.DeserializeObject<Dictionary<string, string>>(decodedData) ?? new Dictionary<string, string>();
        //}

        public Dictionary<string, string> DecodeFilterToDictionary(string data)
        {
            if (string.IsNullOrEmpty(data))
                return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            string decodedData = HttpUtility.UrlDecode(data);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(decodedData)
                             ?? new Dictionary<string, string>();

            // Create a case-insensitive dictionary
            return new Dictionary<string, string>(dictionary, StringComparer.OrdinalIgnoreCase);
        }

        public async Task<IResult<RptCustomReportAddDto>> SetCustomReportPath(string SystemName, long Rep_ID, long Cus_RP_ID)
        {
            try
            {
                bool btnEditVisible = false; bool isPermission_Edit = false; // modify and then assign to ViewData
                //string path = $"CustomReport{Path.DirectorySeparatorChar}DevReport{Path.DirectorySeparatorChar}" + SystemName + Path.DirectorySeparatorChar;
                string path = $"CustomReport{Path.DirectorySeparatorChar}DevReport{Path.DirectorySeparatorChar}" + SystemName;
                string allowedFiles = await configurationHelper.GetValue(423, session.FacilityId);

                RptCustomReportDto obj = new()
                {
                    Id = Cus_RP_ID,
                    ReportId = Rep_ID
                };

                bool chkEditPermission = await rptServiceManager.RptCustomReportService.Permission_Edit_Report(obj);
                if (chkEditPermission)
                {
                    btnEditVisible = true;
                    isPermission_Edit = true;
                }

                var getReportByIdAndReportId = await rptServiceManager.RptCustomReportService.GetOne(x => x.Id == Cus_RP_ID && x.ReportId == Rep_ID
                    && x.IsDeleted == false);
                if (!getReportByIdAndReportId.Succeeded)
                {
                    return await Result<RptCustomReportAddDto>.FailAsync("Report not found");
                }
                obj = getReportByIdAndReportId.Data;

                //fill view data for model popub
                RptCustomReportAddDto rptCustomReportAddDto = new RptCustomReportAddDto();

                rptCustomReportAddDto.Name = obj.Name;
                rptCustomReportAddDto.Name2 = obj.Name2;
                rptCustomReportAddDto.IsDefault = obj.IsDefault;
                rptCustomReportAddDto.UsersPermissionEdit = new List<string>((obj.UsersPermissionEdit ?? "").Split(','));//{ "18225", "18227", "18228", "18229", "18230" };
                rptCustomReportAddDto.UsersAccess = new List<string>((obj.UsersAccess ?? "").Split(','));
                rptCustomReportAddDto.GoupsPermissionEdit = new List<string>((obj.GoupsPermissionEdit ?? "").Split(','));
                rptCustomReportAddDto.GoupsAccess = new List<string>((obj.GoupsAccess ?? "").Split(','));

                if (obj.IsMain)
                    rptCustomReportAddDto.DDltype = 2;

                // fill session
                mvcSession.AddData<string>("CustomReportPath", path);
                mvcSession.AddData<string>("AllowedFiles", allowedFiles);
                mvcSession.AddData<long>("ReportScreenID", obj.ScreenId ?? 0);
                mvcSession.AddData<long>("Report_ID", obj.ReportId ?? 0);
                mvcSession.AddData<long>("Cus_RP_ID", Cus_RP_ID);
                mvcSession.AddData<string>("Report_Link", obj.ReportLink ?? "");
                mvcSession.AddData<bool>("IsMain", obj.IsMain);
                mvcSession.AddData<bool>("IsPermission_Edit", isPermission_Edit);
                mvcSession.AddData<bool>("BtnEditVisible", btnEditVisible);
                mvcSession.AddData<string>("ReportLayout", "ReportLayout" + Cus_RP_ID.ToString() + Rep_ID.ToString() + "-" + session.UserId);
                mvcSession.AddData<string>("ReportObjectName", "ReportObjectName" + Cus_RP_ID.ToString() + Rep_ID.ToString() + "-" + session.UserId);

                bool chkAccessPermission = await rptServiceManager.RptCustomReportService.Permission_Access_Report(obj);
                if (!chkAccessPermission)
                {
                    return await Result<RptCustomReportAddDto>.FailAsync("Access Denied");
                }

                string customReportPath = Path.Combine(_env.ContentRootPath, path);
                if (!System.IO.Directory.Exists(customReportPath))
                {
                    System.IO.Directory.CreateDirectory(customReportPath);
                }

                return await Result<RptCustomReportAddDto>.SuccessAsync(rptCustomReportAddDto);
            }
            catch (Exception exp)
            {
                return await Result<RptCustomReportAddDto>.FailAsync(exp.Message);
            }
        }

        public async Task<IResult<XtraReport>> OpenReport(XtraReport Report, object dataSource, bool IsAutoDirection = true)
        {
            try
            {
                bool isMain = mvcSession.GetData<bool>("IsMain");
                if (isMain)
                {
                    mvcSession.AddData<int>("DDltypeValue", 2);//
                }
                else
                {
                    string Report_Link = mvcSession.GetData<string>("Report_Link");
                    string fullPath = Path.Combine(_env.ContentRootPath, Report_Link);
                    Report.LoadLayout(fullPath);
                }
                DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(dataSource.GetType());

                Report.DataSource = new List<object>() { dataSource };

                // Use reflection to find the BasicData property,, to get the Report Name
                PropertyInfo? basicDataProperty = dataSource.GetType().GetProperty("BasicData");
                if (basicDataProperty != null)
                {
                    // Get the BasicData object
                    var basicData = basicDataProperty.GetValue(dataSource);

                    // Use reflection to find the ReportNameEnglish property
                    PropertyInfo? reportNameProperty = basicData?.GetType().GetProperty("ReportName");
                    if (reportNameProperty != null)
                    {
                        // Get the value of the ReportNameEnglish property
                        var reportName = reportNameProperty.GetValue(basicData);
                        if (reportName != null)
                            Report.ExportOptions.PrintPreview.DefaultFileName = reportName.ToString();
                    }
                }

                if (IsAutoDirection)
                {
                    if (session.Language == 1)
                    {
                        Report.RightToLeft = RightToLeft.Yes;
                        Report.RightToLeftLayout = RightToLeftLayout.Yes;
                        Report.ApplyLocalization("Default");
                    }
                    else
                    {
                        Report.RightToLeft = RightToLeft.No;
                        Report.RightToLeftLayout = RightToLeftLayout.No;
                        Report.ApplyLocalization("en");
                    }
                }

                //ViewData["ReportName"] = Report;
                //return true;
                return await Result<XtraReport>.SuccessAsync(Report);
            }
            catch (Exception exp)
            {
                //ViewData["RptException"] = exp.Message;
                //return false;
                return await Result<XtraReport>.FailAsync(exp.Message);
            }
        }
    }
}
