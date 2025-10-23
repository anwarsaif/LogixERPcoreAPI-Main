using DevExpress.CodeParser;
using DevExpress.ReportServer.ServiceModel.DataContracts;
using DevExpress.XtraReports.UI;
using Logix.Application.Common;
using Logix.Application.DTOs.RPT;
using Logix.Application.Interfaces.IServices;
using Logix.MVC.Reports;
using Logix.MVC.Reports.FXA.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;

namespace Logix.MVC.Reports
{
    public class CustomReportStorageWebExtension : DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
    {
        readonly string ReporteDirectoryTemp;
        //readonly string ReportDirectory;
        private readonly IWebHostEnvironment env;
        private readonly ICurrentData session;
        private readonly IMvcSession mvcSession;
        private readonly IRptServiceManager rptServiceManager;
        const string FileExtension = ".repx";
        public CustomReportStorageWebExtension(IWebHostEnvironment env,
            ICurrentData session,
            IMvcSession mvcSession,
            IRptServiceManager rptServiceManager)
        {

            ReporteDirectoryTemp = Path.Combine(env.ContentRootPath, "Reports");
            if (!Directory.Exists(ReporteDirectoryTemp))
            {
                Directory.CreateDirectory(ReporteDirectoryTemp);
            }
            this.env = env;
            this.session = session;
            this.mvcSession = mvcSession;
            this.rptServiceManager = rptServiceManager;
        }

        private bool IsWithinReportsFolder(string url, string folder)
        {
            var rootDirectory = new DirectoryInfo(folder);
            var fileInfo = new FileInfo(Path.Combine(folder, url));
            return fileInfo.Directory.FullName.ToLower().StartsWith(rootDirectory.FullName.ToLower());
        }
        private string GetReportDirectory()
        {
            string tempPath = mvcSession.GetData<string>("CustomReportPath");
            string ReportDirectory = Path.Combine(env.ContentRootPath, "") + tempPath;
            if (!Directory.Exists(ReportDirectory))
            {
                Directory.CreateDirectory(ReportDirectory);
            }



            return ReportDirectory;
        }

        public override bool CanSetData(string url)
        {
            // Determines whether or not it is possible to store a report by a given URL. 
            // For instance, make the CanSetData method return false for reports that should be read-only in your storage. 
            // This method is called only for valid URLs (i.e., if the IsValidUrl method returned true) before the SetData method is called.

            return true;
        }


        public override bool IsValidUrl(string url)
        {
            // Determines whether or not the URL passed to the current Report Storage is valid. 
            // For instance, implement your own logic to prohibit URLs that contain white spaces or some other special characters. 
            // This method is called before the CanSetData and GetData methods.
            string ReportDirectory = GetReportDirectory();
            var rootDirectory = new DirectoryInfo(ReportDirectory);
            var fileInfo = new FileInfo(Path.Combine(ReportDirectory, url));
            return fileInfo.Directory.FullName.ToLower().StartsWith(rootDirectory.FullName.ToLower());
        }

        public override byte[] GetData(string url)
        {
            // Returns report layout data stored in a Report Storage using the specified URL. 
            // This method is called only for valid URLs after the IsValidUrl method is called.
            try
            {
                string ReportDirectory = GetReportDirectory();
                var filename = url.Split('\\').Last();
                var fullPath = Path.Combine(ReportDirectory, url + FileExtension);
                if (File.Exists(fullPath))
                {
                    return File.ReadAllBytes(fullPath);
                }
                if (ReportsFactory.Reports.ContainsKey(url))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ReportsFactory.Reports[url]().SaveLayoutToXml(ms);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DevExpress.XtraReports.Web.ClientControls.FaultException("Could not get report data.", ex);
            }
            throw new DevExpress.XtraReports.Web.ClientControls.FaultException(string.Format("Could not find report '{0}'.", url));
        }

        public override Dictionary<string, string> GetUrls()
        {
            string ReportDirectorytt = ReporteDirectoryTemp;
            string ReportDirectory = GetReportDirectory();
            // Returns a dictionary of the existing report URLs and display names. 
            // This method is called when running the Report Designer, 
            // before the Open Report and Save Report dialogs are shown and after a new report is saved to a storage.

            //var repxFiles = new DirectoryInfo(ReportDirectory).GetFiles("*" + FileExtension, SearchOption.AllDirectories);
            var repxFiles = new DirectoryInfo(ReportDirectory).GetFiles("*" + FileExtension, SearchOption.AllDirectories);
            var dictionary = repxFiles
                .Select(x =>
                {
                    var directory = x.Directory.FullName == ReportDirectory ? "" : x.Directory.FullName.Substring(ReportDirectory.Length + 1);
                    return Path.Combine(directory, Path.GetFileNameWithoutExtension(x.Name));
                })
                .ToDictionary(k => k, v => v);
            foreach (var predefinedReportList in ReportsFactory.Reports)
                if (!dictionary.ContainsKey(predefinedReportList.Key))
                    dictionary.Add(predefinedReportList.Key, predefinedReportList.Key);
            return dictionary;
        }

        public override async Task SetDataAsync(XtraReport report, string url)
        {
            try
            {
                // THE URL parameter contains object dto  for model popup saved
                string ReportDirectory = GetReportDirectory();

                var rptCustomReportAddDto = JsonSerializer.Deserialize<RptCustomReportAddDto>(url);
                // Stores the specified report to a Report Storage using the specified URL. 
                // This method is called only after the IsValidUrl and CanSetData methods are called.
                if (!IsWithinReportsFolder(url, ReportDirectory))
                    throw new DevExpress.XtraReports.Web.ClientControls.FaultException("Invalid report name.");
                /*
                                var parts = url.Split('\\');
                                var newDirectory = ReportDirectory + "\\" + parts[0];
                                if (parts.Length == 2 && !Directory.Exists(newDirectory))
                                {
                                    Directory.CreateDirectory(newDirectory);
                                }*/
                await Save(report, rptCustomReportAddDto);
            }
            catch (Exception e)
            {
                throw new DevExpress.XtraReports.Web.ClientControls.FaultException(e.Message);
            }

        }


        public async Task<bool> Save(XtraReport report, RptCustomReportAddDto obj)
        {
            try
            {
                var newObject = new RptCustomReportDto();


                newObject.GoupsAccess = stringFromConvertListStringToListLong(obj.GoupsAccess);
                newObject.GoupsPermissionEdit = stringFromConvertListStringToListLong(obj.GoupsPermissionEdit);
                newObject.UsersPermissionEdit = stringFromConvertListStringToListLong(obj.UsersPermissionEdit);
                newObject.UsersAccess = stringFromConvertListStringToListLong(obj.UsersAccess);

                newObject.Active = true;
                newObject.ReportType = 2;
                newObject.IsDefault = obj.IsDefault;
                newObject.ScreenId = mvcSession.GetData<long>("ReportScreenID");
                newObject.ReportId = mvcSession.GetData<long>("Report_ID");

                newObject.Name = obj?.Name;
                newObject.Name2 = obj?.Name2;

                if (mvcSession.GetData<bool>("IsMain"))
                {
                    newObject.FacilityId = 0;
                }
                else
                {
                    newObject.FacilityId = (int)session.FacilityId;
                }
                if (obj?.DDltype == 1)
                {
                    var Report_Link = mvcSession.GetData<string>("Report_Link");
                    newObject.ReportLink = Report_Link;
                    if (!mvcSession.GetData<bool>("IsMain"))
                    {
                        report.SaveLayoutToXml(Path.Combine(env.ContentRootPath, Report_Link));
                    }
                    ///update date base 
                    newObject.Id = mvcSession.GetData<long>("Cus_RP_ID");
                    //newObject.ModifiedBy = session.UserId;
                    var result = await rptServiceManager.RptCustomReportService.CustomUpdate(newObject);

                    return result.Succeeded;

                }
                else
                {
                    var reportPath = mvcSession.GetData<string>("CustomReportPath");

                    string finalFileName = Guid.NewGuid().ToString() + FileExtension;
                    var ReportDirectory = Path.Combine(env.ContentRootPath, reportPath);

                    if (!Directory.Exists(ReportDirectory))
                    {
                        Directory.CreateDirectory(ReportDirectory);
                    }
                    string fullPath = Path.Combine(ReportDirectory, finalFileName);
                    report.SaveLayoutToXml(fullPath);
                    //newObject.ReportLink = mvcSession.GetData<string>("CustomReportPath").Replace("~", "") + finalFileName;
                    newObject.ReportLink = mvcSession.GetData<string>("CustomReportPath").Replace("~", "") + Path.DirectorySeparatorChar + finalFileName;
                    //newObject.CreatedBy = session.UserId;
                    var res = await rptServiceManager.RptCustomReportService.CustomAdd(newObject);
                    return res.Succeeded;
                }
            }
            catch (Exception exp)
            {
                return false;

            }

        }
        public override async Task<string> SetNewDataAsync(XtraReport report, string defaultUrl)
        {
            try
            {
                await SetDataAsync(report, defaultUrl);
                return "succefull";
            }
            catch (Exception ex)
            {
                return "error";
            }
        }
        public string stringFromConvertListStringToListLong(List<string> stringList)
        {
            List<long> longList = new List<long>();

            foreach (var str in stringList)
            {
                if (long.TryParse(str, out long number))
                {
                    longList.Add(number);
                }
            }
            return string.Join(",", longList);
        }
    }


    public static class ReportsFactory
    {
        public static Dictionary<string, Func<XtraReport>> Reports = new Dictionary<string, Func<XtraReport>>()
        {
            ["TestReport"] = () => new TestReport(),
        };
    }
}
