using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections.Generic;
using Logix.Application.Common;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Logix.Application.DTOs.WF;
using Logix.Domain.Main;
using System.Data;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Logix.Domain.WF;
using Logix.Application.Interfaces.IRepositories;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Threading;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;
using DocumentFormat.OpenXml.Vml.Office;
using System.Linq;

namespace Logix.Application.Helpers
{

    public interface IWorkflowHelper
    {
        Task<long> SaveFiles(long Applicants_ID, long Screen_ID, string App_Type_ID, long Alternative_Emp_ID = 0);
        Task<long> Send(long Applicants_ID, long Screen_ID, int? App_Type_ID = 0, long Alternative_Emp_ID = 0, string Subject = "", long? CustomerId = 0, CancellationToken cancellationToken = default);

    }
    public class WorkflowHelper : IWorkflowHelper
    {
        private readonly IWFRepositoryManager wFRepositoryManager;
        private readonly IMainRepositoryManager mainRepositoryManager;
        private readonly IHrRepositoryManager hrRepositoryManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICurrentData session;
        private readonly IEmailAppHelper email;
        private readonly ISendSmsHelper sendSmsHelper;
        private readonly ISysConfigurationAppHelper sysConfigurationAppHelper;



        public WorkflowHelper(
            IHttpContextAccessor httpContextAccessor,
            ICurrentData session,
            IWFRepositoryManager wFRepositoryManager,
            IMainRepositoryManager mainRepositoryManager,
            IHrRepositoryManager hrRepositoryManager,
            IEmailAppHelper email,
            ISendSmsHelper sendSmsHelper,
            ISysConfigurationAppHelper sysConfigurationAppHelper
            )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.session = session;
            this.wFRepositoryManager = wFRepositoryManager;
            this.mainRepositoryManager = mainRepositoryManager;
            this.hrRepositoryManager = hrRepositoryManager;
            this.email = email;
            this.sendSmsHelper = sendSmsHelper;
            this.sysConfigurationAppHelper = sysConfigurationAppHelper;

        }

        public async Task<long> SaveFiles(long Applicants_ID, long Screen_ID, string App_Type_ID, long Alternative_Emp_ID = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(App_Type_ID))
                {
                    var WorkflowId = await mainRepositoryManager.SysScreenWorkflowRepository.GetOne(s => s.WorkflowId, x => x.ScreenId == Screen_ID);
                }
                if (!string.IsNullOrEmpty(App_Type_ID))
                {
                    var WfApplicationDto = new WfApplication();
                    WfApplicationDto.ApplicantsId = Applicants_ID;
                    WfApplicationDto.ApplicationsTypeId = int.Parse(App_Type_ID);
                    WfApplicationDto.ApplicationDate = DateTime.Now.ToString("yyyy/MM/dd");
                    WfApplicationDto.StatusId = 2;
                    WfApplicationDto.CreatedBy = 0;
                    WfApplicationDto.StepId = 1;
                    WfApplicationDto.BranchId = 0;
                    WfApplicationDto.AlternativeEmpId = Alternative_Emp_ID;
                    var addRes = await wFRepositoryManager.WfApplicationRepository.AddAndReturn(WfApplicationDto);

                    Applicants_ID = addRes.Id;
                }
                Dictionary<Guid, System.Data.SqlClient.SqlParameter> AttributeValues = new Dictionary<Guid, System.Data.SqlClient.SqlParameter>();
                var DynamicAttributes = await wFRepositoryManager.WfDynamicAttributeRepository.GetAllView(x => x.AppTypeId == int.Parse(App_Type_ID));
                if (DynamicAttributes != null)
                {
                    foreach (var attr in DynamicAttributes)
                    {
                        Guid DynamicAttributeId = attr.DynamicAttributeId;
                        var DataTypeId = (DataTypeIdEnum)Convert.ToInt32(attr.DataTypeId);
                        long Table_ID = Convert.ToInt64(attr.TableId.ToString());
                        AttributeValues[DynamicAttributeId] = GetValueForCustomAttribute(DynamicAttributeId, DataTypeId, Table_ID);                        // Rest of the code...
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }

        private SqlParameter GetValueForCustomAttribute(Guid dynamicAttributeId, DataTypeIdEnum dataTypeId, long tableId)
        {
            SqlParameter userInputParam = new SqlParameter
            {
                ParameterName = "@DynamicValue",
                Value = DBNull.Value
            };

            string ctrlId = GetID(dynamicAttributeId);
            string controlValue = httpContextAccessor.HttpContext.Request.Form[ctrlId];

            switch (dataTypeId)
            {

                case DataTypeIdEnum.String:
                    userInputParam.DbType = DbType.String;
                    userInputParam.Value = controlValue?.Trim();
                    break;

                case DataTypeIdEnum.Boolean:
                    if (bool.TryParse(controlValue, out bool boolValue))
                    {
                        userInputParam.Value = boolValue;
                        userInputParam.DbType = DbType.Boolean;
                    }
                    break;

                case DataTypeIdEnum.Numeric:
                    userInputParam.DbType = DbType.Double;
                    if (double.TryParse(controlValue, out double doubleValue))
                    {
                        userInputParam.Value = doubleValue;
                    }
                    break;

                case DataTypeIdEnum.Date:
                    userInputParam.DbType = DbType.String;
                    userInputParam.Value = controlValue?.Trim();
                    break;

                case DataTypeIdEnum.PickList:
                    userInputParam.DbType = DbType.String;
                    if (controlValue != "0")
                    {
                        userInputParam.Value = controlValue;
                    }
                    break;
            }

            return userInputParam;
        }
        private string GetID(Guid dynamicAttributeId)
        {
            return dynamicAttributeId.ToString().Replace("-", "_");
        }

        public async Task<long> Send(long Applicants_ID, long Screen_ID, int? App_Type_ID = 0, long Alternative_Emp_ID = 0, string AppSubject = "", long? CustomerId = 0, CancellationToken cancellationToken = default)
        {
            string? Users = "";
            string? Emails = "";
            string? Mobiles = "";
            string? StatusName = "";
            string? StatusName2 = "";
            string? TypeName = "";
            string? TypeName2 = "";
            string? XPageUrl = "";
            string? FullPageUrl = "";
            string? Subject = "";
            long ApplicationId = 0;
            try
            {
                App_Type_ID ??= 0;
                if (App_Type_ID == 0)
                {
                    var WorkflowId = await mainRepositoryManager.SysScreenWorkflowRepository.GetOne(s => s.WorkflowId, x => x.ScreenId == Screen_ID);
                    App_Type_ID = (int?)(WorkflowId ?? 0);
                }

                if (App_Type_ID > 0)
                {
                    var WfApplicationDto = new WfApplication();
                    WfApplicationDto.ApplicantsId = Applicants_ID;
                    WfApplicationDto.ApplicationsTypeId = App_Type_ID;
                    WfApplicationDto.ApplicationDate = DateTime.Now.ToString("yyyy/MM/dd", new CultureInfo("en"));
                    WfApplicationDto.StatusId = 2;
                    WfApplicationDto.CreatedBy = session.UserId;
                    WfApplicationDto.StepId = 1;
                    WfApplicationDto.BranchId = session.BranchId;
                    WfApplicationDto.CustomerId = 0;
                    WfApplicationDto.ProjectId = 0;
                    WfApplicationDto.ProjectItemId = 0;
                    WfApplicationDto.AlternativeEmpId = Alternative_Emp_ID;
                    WfApplicationDto.Subject = AppSubject;
                    WfApplicationDto.CustomerId = CustomerId;


                    var addRes = await AddWfApplicationAsync(WfApplicationDto);
                    ApplicationId = addRes.Id;
                    Dictionary<Guid, System.Data.SqlClient.SqlParameter> AttributeValues = new Dictionary<Guid, System.Data.SqlClient.SqlParameter>();
                    var DynamicAttributes = await wFRepositoryManager.WfDynamicAttributeRepository.GetAllView(x => x.AppTypeId == App_Type_ID);
                    if (DynamicAttributes != null)
                    {
                        foreach (var attr in DynamicAttributes)
                        {
                            Guid DynamicAttributeId = attr.DynamicAttributeId;
                            var DataTypeId = (DataTypeIdEnum)Convert.ToInt32(attr.DataTypeId);
                            long Table_ID = Convert.ToInt64(attr.TableId.ToString());
                            AttributeValues[DynamicAttributeId] = GetValueForCustomAttribute(DynamicAttributeId, DataTypeId, Table_ID);                        // Rest of the code...
                        }
                    }
                    foreach (var AttributeId in AttributeValues.Keys)
                    {
                        var newDynamicValue = new WfDynamicValue
                        {
                            AppTypeId = Convert.ToInt64(App_Type_ID),
                            AttributeId = AttributeId,
                            CreatedBy = session.UserId,
                            CreatedOn = DateTime.Now,
                            ApplicationId = ApplicationId,
                            DynamicValue = AttributeValues[AttributeId],
                        };
                        var addToDynamicValue = await wFRepositoryManager.WfDynamicValueRepository.AddAndReturn(newDynamicValue);
                        await wFRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                    }
                    ////////////////////////////////////////////////////////
                    var GetStep_ID = await wFRepositoryManager.WfStepsTransactionRepository.GetOne(x => x.ToStepId, x => x.IsDeleted == false && x.SortNo == 1 && x.AppTypeId == Convert.ToInt32(App_Type_ID));
                    if (GetStep_ID == null)
                    {
                        GetStep_ID = "0";
                    }
                    ////////////////////////////////////////////////////////
                    // ApplicationsStatus  هنا تبدأ الاضافة الى جدول 
                    var appStatus = new WfApplicationsStatusDto
                    {
                        ApplicantsId = session.UserId,
                        ApplicationsId = ApplicationId,
                        CreatedBy = session.UserId,
                        CreatedOn = DateTime.Now,
                        NewStatusId = 1,
                        Note = "",
                        StepId = Convert.ToInt32(GetStep_ID),
                    };
                    var StatusId = await wFRepositoryManager.WfApplicationsStatusRepository.ProcessApplicationStatusAsync((long)appStatus.ApplicationsId, 0, appStatus.NewStatusId, appStatus.StepId, appStatus.Note, appStatus.DesNo, appStatus.CouncilDate);

                    //  var ExecuteInsertProcedure = await wFRepositoryManager.WfApplicationsStatusRepository.InsertApplicationsStatus(appStatus);
                    var GetWFStatus = await wFRepositoryManager.WfStatusRepository.GetOne(X => X.Id == 1);
                    if (GetWFStatus != null)
                    {
                        StatusName = GetWFStatus.StatusName ?? "";
                        StatusName2 = GetWFStatus.StatusName2;
                    }
                    /////////////////////////
                    var GetTypeName = await wFRepositoryManager.WfAppTypeRepository.GetOne(X => X.Id == App_Type_ID);
                    if (GetTypeName != null)
                    {
                        TypeName = GetTypeName.Name;
                        TypeName2 = GetTypeName.Name2;
                        XPageUrl = GetTypeName.Url;
                    }
                    if (XPageUrl != ".")
                    {
                        FullPageUrl = XPageUrl + "_View?ID=" + ApplicationId + "&App_Type_ID=" + App_Type_ID;

                    }
                    else
                    {

                        FullPageUrl = "/Apps/Workflow/Applications/Applications_Transfer.aspx?action=view&ID=" + ApplicationId + "&App_Type_ID=" + App_Type_ID;
                    }
                    var Msgtxt = new StringBuilder();
                    Msgtxt.Append("<table class='sos' style='width:100%'>");
                    Msgtxt.Append("<tr><td>رقم الطلب </td> <td> " + addRes.ApplicationCode + "</td><td>Request No</td></tr>");
                    Msgtxt.Append("<tr><td>نوع الطلب </td> <td> " + TypeName + " - " + TypeName2 + "</td><td>Request Type</td></tr>");
                    Msgtxt.Append("<tr><td>تاريخ الطلب </td> <td> " + DateTime.Now.ToString() + "</td><td>Request Date</td></tr>");
                    Msgtxt.Append("<tr><td>حالة الطلب </td> <td> " + StatusName + " - " + StatusName2 + "</td><td>Request Status</td></tr>");
                    Msgtxt.Append("</table>");
                    //var GetApplicationsToSendEmail = await mainRepositoryManager.StoredProceduresRepository.WF_UsersEmail_App_Sp(ApplicationId);
                    var GetApplicationsToSendEmail = await wFRepositoryManager.WfApplicationRepository.GetUsersEmailByApplicationIdAsync(ApplicationId);
                    if (GetApplicationsToSendEmail != null)
                    {
                        Subject = GetApplicationsToSendEmail.Subject;
                        Emails = GetApplicationsToSendEmail.Email;
                        //   هنا سنقوم بارسال الايميلات
                        await email.SendEmailAsync(Emails, Subject, "");

                    }
                    //   ارسال اشعار اضافي بناء على جدول الاشعارات في الخطوات

                    var GetNotificationsStep = await wFRepositoryManager.WfStepsNotificationRepository.GetAll(x => x.IsDeleted == false && x.StepId == Convert.ToInt64(GetStep_ID));

                    if (GetNotificationsStep.Any())
                    {
                        foreach (var item in GetNotificationsStep)
                        {
                            //   ارسال على المستخدمين

                            if (item.TypeId == 1)
                            {
                                var result = await Get_Data_For_Users_ID(item.UsersId);
                                Users = result.Users;
                                Emails = result.Emails;
                                Mobiles = result.Mobiles;
                            }
                            //  ارسال على المجموعات
                            else if (item.TypeId == 2)
                            {
                                var result = await Get_Data_For_Users_ID(item.GroupsId);
                                Users = result.Users;
                                Emails = result.Emails;
                                Mobiles = result.Mobiles;
                            }

                            //  ارسال على بريد الكترني
                            else if (item.TypeId == 3)
                            {
                                Users = "";
                                Emails = item.Emails;
                                Mobiles = "";
                            }
                            //  ارسال على مدير المباشر
                            else if (item.TypeId == 4)
                            {
                                var result = await Get_Data_For_Manger_ID(Applicants_ID.ToString());
                                Users = result.Users;
                                Emails = result.Emails;
                                Mobiles = result.Mobiles;
                            }
                            // ارسال على مدير إداري 
                            else if (item.TypeId == 5)
                            {
                                var result = await Get_Data_For_Manager2_ID(Applicants_ID.ToString());
                                Users = result.Users;
                                Emails = result.Emails;
                                Mobiles = result.Mobiles;
                            }
                            // ارسال على مدير إداري 2
                            else if (item.TypeId == 6)
                            {
                                var result = await Get_Data_For_Manager3_ID(Applicants_ID.ToString());
                                Users = result.Users;
                                Emails = result.Emails;
                                Mobiles = result.Mobiles;
                            }
                            //  ارسال على الموظف البديل
                            else if (item.TypeId == 7)
                            {
                                var result = await Get_Data_For_Alternative(ApplicationId.ToString());
                                Users = result.Users;
                                Emails = result.Emails;
                                Mobiles = result.Mobiles;
                            }

                            ///////////////////////////////////////////////////////////////////

                            //  ارسال اشعار عبر النظام
                            if (!string.IsNullOrEmpty(item.SysMessage))
                            {
                                var UserslistArray = Users.Split(',');
                                foreach (var UserItem in UserslistArray)
                                {
                                    var newSysNotification = new SysNotification
                                    {
                                        CreatedBy = session.UserId,
                                        CreatedOn = DateTime.Now,
                                        Url = FullPageUrl,
                                        MsgTxt = item.SysMessage,
                                    };
                                    await mainRepositoryManager.SysNotificationRepository.Add(newSysNotification);

                                    await mainRepositoryManager.UnitOfWork.CompleteAsync();
                                }
                            }

                            //  ارسال اشعار عبر البريد
                            if (!string.IsNullOrEmpty(item.EmailMessage))
                            {
                                var EmailslistArray = Emails.Split(',');
                                foreach (var EmailItem in EmailslistArray)
                                {
                                    //   SendMsgEmail(EmailItem, "إشعار عن طلب ", item.EmailMessage)

                                }
                            }

                            //  ارسال اشعار عبر الجوال
                            if (!string.IsNullOrEmpty(item.SmsMessage))
                            {
                                var MobileslistArray = Mobiles.Split(',');
                                foreach (var MobilesItem in MobileslistArray)
                                {
                                    await sendSmsHelper.SendSms(MobilesItem, Subject);

                                }
                            }
                        }
                    }
                }

                return ApplicationId;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<(string Users, string Emails, string Mobiles)> Get_Data_For_Users_ID(string UsersID)
        {
            string Users = "";
            string Emails = "";
            string Mobiles = "";

            var getData = await mainRepositoryManager.SysUserRepository.GetAllVw(x => x.Enable == 1 && x.IsDeleted == false && x.Isdel == false && x.Mobile != null && x.UserEmail != null && x.UserId.ToString() == UsersID);
            foreach (var item in getData)
            {
                Users += item.UserId.ToString() + ",";
                Emails += item.UserEmail.ToString() + ",";
                Mobiles += "966" + item.Mobile.Substring(2, 9).ToString() + ",";
            }

            if (Users.Length > 1)
            {
                Users = Users.Substring(0, Users.Length - 1);
            }
            if (Emails.Length > 1)
            {
                Emails = Emails.Substring(0, Emails.Length - 1);
            }
            if (Mobiles.Length > 1)
            {
                Mobiles = Mobiles.Substring(0, Mobiles.Length - 1);
            }

            return (Users, Emails, Mobiles);
        }
        public async Task<(string Users, string Emails, string Mobiles)> Get_Data_For_Groups_ID(string GroupsID)
        {
            string Users = "";
            string Emails = "";
            string Mobiles = "";

            var getData = await mainRepositoryManager.SysUserRepository.GetAllVw(x => x.Enable == 1 && x.IsDeleted == false && x.Isdel == false && x.Mobile != null && x.UserEmail != null && x.GroupsId != null && x.GroupsId.ToString() == GroupsID);
           
            foreach (var item in getData)
            {
                Users += item.UserId.ToString() + ",";
                Emails += item.UserEmail.ToString() + ",";
                Mobiles += "966" + item.Mobile.Substring(2, 9).ToString() + ",";
            }

            if (Users.Length > 1)
            {
                Users = Users.Substring(0, Users.Length - 1);
            }
            if (Emails.Length > 1)
            {
                Emails = Emails.Substring(0, Emails.Length - 1);
            }
            if (Mobiles.Length > 1)
            {
                Mobiles = Mobiles.Substring(0, Mobiles.Length - 1);
            }

            return (Users, Emails, Mobiles);
        }
        public async Task<(string Users, string Emails, string Mobiles)> Get_Data_For_Manger_ID(string EmpID)
        {
            string Users = "";
            string Emails = "";
            string Mobiles = "";
            var getManagerData = await hrRepositoryManager.HrEmployeeRepository.GetAllVw(x => x.Isdel == false && x.IsDeleted == false && x.Id.ToString() == EmpID);
            var ManagerId = getManagerData.Select(x => x.ManagerId).FirstOrDefault().ToString();
            var getData = await mainRepositoryManager.SysUserRepository.GetAllVw(x => x.Enable == 1 && x.IsDeleted == false && x.Isdel == false && x.Mobile != null && x.UserEmail != null && x.EmpId.ToString() == ManagerId);
            foreach (var item in getData)
            {
                Users += item.UserId.ToString() + ",";
                Emails += item.UserEmail.ToString() + ",";
                Mobiles += "966" + item.Mobile.Substring(2, 9).ToString() + ",";
            }

            if (Users.Length > 1)
            {
                Users = Users.Substring(0, Users.Length - 1);
            }
            if (Emails.Length > 1)
            {
                Emails = Emails.Substring(0, Emails.Length - 1);
            }
            if (Mobiles.Length > 1)
            {
                Mobiles = Mobiles.Substring(0, Mobiles.Length - 1);
            }

            return (Users, Emails, Mobiles);
        }
        public async Task<(string Users, string Emails, string Mobiles)> Get_Data_For_Manager2_ID(string EmpID)
        {
            string Users = "";
            string Emails = "";
            string Mobiles = "";
            var getManagerData = await hrRepositoryManager.HrEmployeeRepository.GetAllVw(x => x.Isdel == false && x.IsDeleted == false && x.Id.ToString() == EmpID);
            var ManagerId = getManagerData.Select(x => x.Manager2Id).FirstOrDefault().ToString();
            var getData = await mainRepositoryManager.SysUserRepository.GetAllVw(x => x.Enable == 1 && x.IsDeleted == false && x.Isdel == false && x.Mobile != null && x.UserEmail != null && x.EmpId.ToString() == ManagerId);
            foreach (var item in getData)
            {
                Users += item.UserId.ToString() + ",";
                Emails += item.UserEmail.ToString() + ",";
                Mobiles += "966" + item.Mobile.Substring(2, 9).ToString() + ",";
            }

            if (Users.Length > 1)
            {
                Users = Users.Substring(0, Users.Length - 1);
            }
            if (Emails.Length > 1)
            {
                Emails = Emails.Substring(0, Emails.Length - 1);
            }
            if (Mobiles.Length > 1)
            {
                Mobiles = Mobiles.Substring(0, Mobiles.Length - 1);
            }

            return (Users, Emails, Mobiles);
        }
        public async Task<(string Users, string Emails, string Mobiles)> Get_Data_For_Manager3_ID(string EmpID)
        {
            string Users = "";
            string Emails = "";
            string Mobiles = "";
            var getManagerData = await hrRepositoryManager.HrEmployeeRepository.GetAllVw(x => x.Isdel == false && x.IsDeleted == false && x.Id.ToString() == EmpID);
            var ManagerId = getManagerData.Select(x => x.Manager3Id).FirstOrDefault().ToString();
            var getData = await mainRepositoryManager.SysUserRepository.GetAllVw(x => x.Enable == 1 && x.IsDeleted == false && x.Isdel == false && x.Mobile != null && x.UserEmail != null && x.EmpId.ToString() == ManagerId);
            foreach (var item in getData)
            {
                Users += item.UserId.ToString() + ",";
                Emails += item.UserEmail.ToString() + ",";
                Mobiles += "966" + item.Mobile.Substring(2, 9).ToString() + ",";
            }

            if (Users.Length > 1)
            {
                Users = Users.Substring(0, Users.Length - 1);
            }
            if (Emails.Length > 1)
            {
                Emails = Emails.Substring(0, Emails.Length - 1);
            }
            if (Mobiles.Length > 1)
            {
                Mobiles = Mobiles.Substring(0, Mobiles.Length - 1);
            }

            return (Users, Emails, Mobiles);
        }
        public async Task<(string Users, string Emails, string Mobiles)> Get_Data_For_Alternative(string ApplicationsID)
        {
            string Users = "";
            string Emails = "";
            string Mobiles = "";
            var getAlternativeData = await wFRepositoryManager.WfApplicationRepository.GetAllView(x => x.Id.ToString() == ApplicationsID);
            var AlternativeId = getAlternativeData.Select(x => x.AlternativeEmpId).FirstOrDefault().ToString();
            var getData = await mainRepositoryManager.SysUserRepository.GetAllVw(x => x.Enable == 1 && x.IsDeleted == false && x.Isdel == false && x.Mobile != null && x.UserEmail != null && x.EmpId.ToString() == AlternativeId);
            foreach (var item in getData)
            {
                Users += item.UserId.ToString() + ",";
                Emails += item.UserEmail.ToString() + ",";
                Mobiles += "966" + item.Mobile.Substring(2, 9).ToString() + ",";
            }
            if (Users.Length > 1)
            {
                Users = Users.Substring(0, Users.Length - 1);
            }
            if (Emails.Length > 1)
            {
                Emails = Emails.Substring(0, Emails.Length - 1);
            }
            if (Mobiles.Length > 1)
            {
                Mobiles = Mobiles.Substring(0, Mobiles.Length - 1);
            }

            return (Users, Emails, Mobiles);
        }

        private async Task<(long? ApplicationCode, long Id)> AddWfApplicationAsync(WfApplication obj, CancellationToken cancellationToken = default)
        {
            try
            {
                // Step 1: Retrieve the branch flag value
                var readBranchFromEmployee = await sysConfigurationAppHelper.GetValue(155, session.FacilityId);

                long branchId = 0;
                if (readBranchFromEmployee == "1")
                {
                    // Check if the employee exists
                    var checkEmpExist = await mainRepositoryManager.InvestEmployeeRepository.GetOne(x => x.Id == obj.ApplicantsId && x.IsDeleted == false && x.Isdel == false);
                    if (checkEmpExist != null && checkEmpExist.BranchId > 0)
                        branchId = Convert.ToInt64(checkEmpExist.BranchId);
                }

                // Step 2: Retrieve the numbering by year value
                var numberingByYear = await sysConfigurationAppHelper.GetValue(116, session.FacilityId);
                string yearNumbering = numberingByYear ?? "0";

                // Step 3: Get Application Code by filtering in memory
                // Step 3: Retrieve all applications and filter in memory
                var applications = await wFRepositoryManager.WfApplicationRepository.GetAll();

                // Filter the applications based on the year of ApplicationDate in memory
                var filteredApplications = applications
                   .Where(x => x.ApplicationDate != null && x.ApplicationDate.Substring(0, 4) == (yearNumbering == "1" ? obj.ApplicationDate.Substring(0, 4) : obj.ApplicationDate.Substring(0, 4)))
                   .ToList();

                // Find the maximum application code, or default to 1 if no records exist
                var applicationCode = filteredApplications.Any() ? filteredApplications.Max(x => x.ApplicationCode) + 1 : 1;


                // Step 4: Get Step ID if Step_ID == 1
                if (obj.StepId == 1)
                {
                    var wfSteps = await wFRepositoryManager.WfStepRepository.GetAll(x => x.IsDeleted == false && x.StepTypeId == 1);

                    // Create a list of string-ified Id values
                    var wfStepsStringList = wfSteps.Select(x => x.Id.ToString()).ToList();

                    // Filter the WfStepsTransaction based on the string list
                    var wfStepTransaction = await wFRepositoryManager.WfStepsTransactionRepository
                        .GetOne(x => x.AppTypeId == obj.ApplicationsTypeId
                            && x.SortNo == 1
                            && x.IsDeleted == false
                            && x.FromStepId != null
                            && wfStepsStringList.Contains(x.FromStepId));

                    if (wfStepTransaction != null)
                    {
                        obj.StepId = Convert.ToInt32(wfStepTransaction.FromStepId);
                    }
                }

                // Step 5: Insert a new record into WF_Applications
                obj.ApplicationCode = applicationCode;
                var addRes = await wFRepositoryManager.WfApplicationRepository.AddAndReturn(obj);
                await wFRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                // Return the ApplicationCode and Id as a tuple
                return (obj.ApplicationCode, addRes.Id);
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new InvalidOperationException("An error occurred while adding the WF application.", ex);
            }
        }




        //public async Task<List<object>> GetCustomAttributeUI(Guid dynamicAttributeId, DataTypeIdEnum dataTypeId, bool attributeRequired, int lookUpCategoriesId, int tableId, string defaultValue, int sysLookUpCategoriesId, string lookUpSql, int lookUpType, bool isReadOnly, string validationGroup = "send")
        //{
        //    var controls = new List<object>();
        //    string dfValue = "";

        //    if (!string.IsNullOrEmpty(defaultValue))
        //    {
        //        var obj = new HR_LBR.Universal();
        //        var objDate = DateTime.Now;
        //        dfValue = obj.Get_GetDefaultValue(defaultValue, session.EmpId, objDate, session.FacilityId, 0, session.FinYear);
        //    }

        //    switch (dataTypeId)
        //    {
        //        case DataTypeIdEnum.String:
        //            controls.Add(new
        //            {
        //                type = "text",
        //                id = GetID(dynamicAttributeId),
        //                value = dfValue,
        //                readOnly = isReadOnly,
        //                cssClass = "span12"
        //            });

        //            if (attributeRequired)
        //            {
        //                controls.Add(new
        //                {
        //                    type = "validator",
        //                    id = GetID(dynamicAttributeId),
        //                    message = "الحقل مطلوب.",
        //                    validationGroup = validationGroup
        //                });
        //            }
        //            break;

        //        case DataTypeIdEnum.Boolean:
        //            controls.Add(new
        //            {
        //                type = "checkbox",
        //                id = GetID(dynamicAttributeId),
        //                Ischecked = false
        //            });
        //            break;

        //        case DataTypeIdEnum.Numeric:
        //            controls.Add(new
        //            {
        //                type = "number",
        //                id = GetID(dynamicAttributeId),
        //                value = dfValue,
        //                readOnly = isReadOnly,
        //                cssClass = "span12"
        //            });

        //            if (attributeRequired)
        //            {
        //                controls.Add(new
        //                {
        //                    type = "validator",
        //                    id = GetID(dynamicAttributeId),
        //                    message = "القيمة غير رقمية.",
        //                    validationGroup = validationGroup
        //                });
        //            }
        //            break;

        //        case DataTypeIdEnum.Date:
        //            controls.Add(new
        //            {
        //                type = "text",
        //                id = GetID(dynamicAttributeId),
        //                value = dfValue,
        //                cssClass = "popupDatepicker",
        //                readOnly = isReadOnly
        //            });

        //            if (attributeRequired)
        //            {
        //                controls.Add(new
        //                {
        //                    type = "validator",
        //                    id = GetID(dynamicAttributeId),
        //                    message = "الإدخال غير صحيح.",
        //                    validationGroup = validationGroup
        //                });
        //            }
        //            break;

        //        case DataTypeIdEnum.PickList:
        //            var items = GetPickListItems(lookUpType, lookUpCategoriesId, sysLookUpCategoriesId, lookUpSql);
        //            controls.Add(new
        //            {
        //                type = "dropdown",
        //                id = GetID(dynamicAttributeId),
        //                items = items,
        //                selectedValue = dfValue
        //            });

        //            if (attributeRequired)
        //            {
        //                controls.Add(new
        //                {
        //                    type = "validator",
        //                    id = GetID(dynamicAttributeId),
        //                    message = "الحقل مطلوب.",
        //                    validationGroup = validationGroup
        //                });
        //            }
        //            break;

        //        case DataTypeIdEnum.Longstring:
        //            controls.Add(new
        //            {
        //                type = "textarea",
        //                id = GetID(dynamicAttributeId),
        //                value = dfValue,
        //                readOnly = isReadOnly,
        //                cssClass = "span12",
        //                rows = 5,
        //                cols = 50
        //            });

        //            if (attributeRequired)
        //            {
        //                controls.Add(new
        //                {
        //                    type = "validator",
        //                    id = GetID(dynamicAttributeId),
        //                    message = "الحقل مطلوب.",
        //                    validationGroup = validationGroup
        //                });
        //            }
        //            break;

        //        case DataTypeIdEnum.Time:
        //            controls.Add(new
        //            {
        //                type = "text",
        //                id = GetID(dynamicAttributeId),
        //                value = dfValue,
        //                readOnly = isReadOnly,
        //                cssClass = "timepicker"
        //            });

        //            if (attributeRequired)
        //            {
        //                controls.Add(new
        //                {
        //                    type = "validator",
        //                    id = GetID(dynamicAttributeId),
        //                    message = "الحقل مطلوب.",
        //                    validationGroup = validationGroup
        //                });
        //            }
        //            break;

        //        case DataTypeIdEnum.File:
        //            controls.Add(new
        //            {
        //                type = "file",
        //                id = GetID(dynamicAttributeId)
        //            });

        //            if (attributeRequired)
        //            {
        //                controls.Add(new
        //                {
        //                    type = "validator",
        //                    id = GetID(dynamicAttributeId),
        //                    message = "الحقل مطلوب.",
        //                    validationGroup = validationGroup
        //                });
        //            }
        //            break;

        //        case DataTypeIdEnum.Table:
        //            var tableData = GetTableData(tableId, dynamicAttributeId);
        //            controls.Add(new
        //            {
        //                type = "table",
        //                id = GetID(dynamicAttributeId),
        //                columns = tableData.Columns,
        //                rows = tableData.Rows
        //            });
        //            break;
        //    }

        //    return controls;
        //}

    }
}

