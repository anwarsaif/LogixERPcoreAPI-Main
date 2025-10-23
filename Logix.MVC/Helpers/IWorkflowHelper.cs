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
using Logix.Application.Helpers;
using System.Net.Mail;
using System.Globalization;

namespace Logix.MVC.Helpers
{

    public interface IWorkflowHelper
    {
        Task<long> SaveFiles(long Applicants_ID, long Screen_ID, string App_Type_ID, long Alternative_Emp_ID = 0);
        Task<long> Send(long Applicants_ID, long Screen_ID, string App_Type_ID = "", long Alternative_Emp_ID = 0);

    }
    public class WorkflowHelper : IWorkflowHelper
    {
        private readonly IWFRepositoryManager wFRepositoryManager;
        private readonly IMainRepositoryManager mainRepositoryManager;
        private readonly IHrRepositoryManager hrRepositoryManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICurrentData session;
        private readonly IEmailAppHelper email;
        //private readonly IApplicationDbContext context;
        private readonly IWFServiceManager wFServiceManager;
        private readonly IEmailService emailService;


        public WorkflowHelper(
            IWFServiceManager wFServiceManager,
             IEmailService emailService,
            IHttpContextAccessor httpContextAccessor,
            ICurrentData session,
            IWFRepositoryManager wFRepositoryManager,
            IMainRepositoryManager mainRepositoryManager,
            IHrRepositoryManager hrRepositoryManager,
            IEmailAppHelper email
            )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.session = session;
            this.wFRepositoryManager = wFRepositoryManager;
            this.mainRepositoryManager = mainRepositoryManager;
            this.hrRepositoryManager = hrRepositoryManager;
            this.email = email;
            this.emailService = emailService;
            this.wFServiceManager=wFServiceManager;
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

        public async Task<long> Send(long Applicants_ID, long Screen_ID, string App_Type_ID = "", long Alternative_Emp_ID = 0)
        {
            String? Users = "";
            String? Emails = "";
            String? Mobiles = "";
            String? StatusName = "";
            String? StatusName2 = "";
            String? TypeName = "";
            String? TypeName2 = "";
            String? XPageUrl = "";
            String? FullPageUrl = "";
            String? Subject = "";
            long ApplicationId = 0;
            try
            {
                if (string.IsNullOrEmpty(App_Type_ID))
                {
                    var WorkflowId = await mainRepositoryManager.SysScreenWorkflowRepository.GetOne(s => s.WorkflowId, x => x.ScreenId == Screen_ID);
                    App_Type_ID = WorkflowId.ToString() ?? "";
                }

                if (!string.IsNullOrEmpty(App_Type_ID))
                {
                    var WfApplicationDto = new WfApplicationDto();
                    WfApplicationDto.ApplicantsId = Applicants_ID;
                    WfApplicationDto.ApplicationsTypeId = int.Parse(App_Type_ID);
                    //WfApplicationDto.ApplicationDate = DateTime.Now.ToString("yyyy/MM/dd");
                    WfApplicationDto.ApplicationDate = DateTime.Now.ToString("yyyy/MM/dd", new CultureInfo("en"));
                    WfApplicationDto.StatusId = 2;
                    WfApplicationDto.CreatedBy = 0;
                    WfApplicationDto.StepId = 1;
                    WfApplicationDto.BranchId = 0;
                    WfApplicationDto.AlternativeEmpId = Alternative_Emp_ID;
                   // var addRes = await wFRepositoryManager.WfApplicationRepository.AddAndReturn(WfApplicationDto);
                    var addRes = await wFServiceManager.WfApplicationService.Add(WfApplicationDto);
                    //await context.SaveChangesAsync(cancellationToken:default);
                    ApplicationId = addRes.Data.Id;
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
                    var ExecuteInsertProcedure = await wFRepositoryManager.WfApplicationsStatusRepository.InsertApplicationsStatus(appStatus);
                    var GetWFStatus = await wFRepositoryManager.WfStatusRepository.GetOne(X => X.Id == 1);
                    if (GetWFStatus != null)
                    {
                        StatusName = GetWFStatus.StatusName ?? "";
                        StatusName2 = GetWFStatus.StatusName2;
                    }
                    /////////////////////////
                    var GetTypeName = await wFRepositoryManager.WfAppTypeRepository.GetOne(X => X.Id.ToString() == App_Type_ID);
                    if (GetTypeName != null)
                    {
                        TypeName = GetTypeName.Name;
                        TypeName2 = GetTypeName.Name2;
                        XPageUrl = GetTypeName.Url;
                    }
                    if (XPageUrl != ".")
                    {
                        //FullPageUrl = XPageUrl + "_View.aspx?action=view&ID=" + ApplicationId + "&App_Type_ID=" + App_Type_ID;
                        FullPageUrl = XPageUrl + "_View?ID=" + ApplicationId + "&App_Type_ID=" + App_Type_ID;
                    }
                    else
                    {
                        FullPageUrl = "/Apps/Workflow/Applications/Applications_Transfer.aspx?action=view&ID=" + ApplicationId + "&App_Type_ID=" + App_Type_ID;
                    }
                    var Msgtxt = new StringBuilder();
                    Msgtxt.Append("<table class='sos' style='width:100%; text-align: center;'>");
                    Msgtxt.Append("<tr><td>Request No </td> <td> " + addRes.Data.ApplicationCode + "</td><td>رقم الطلب</td></tr>");
                    Msgtxt.Append("<tr><td>Request Type </td> <td> " + TypeName2 + " - " + TypeName + "</td><td>نوع الطلب</td></tr>");
                    Msgtxt.Append("<tr><td>Request Date </td> <td> " + DateTime.Now.ToString() + "</td><td>تاريخ الطلب</td></tr>");
                    Msgtxt.Append("<tr><td>Request Status </td> <td> " + StatusName2 + " - " + StatusName + "</td><td>حالة الطلب</td></tr>");
                    Msgtxt.Append("</table>");
                    var GetApplicationsToSendEmail = await mainRepositoryManager.StoredProceduresRepository.WF_UsersEmail_App_Sp(ApplicationId);
                    if (GetApplicationsToSendEmail != null)
                    {
                        foreach (var item in GetApplicationsToSendEmail)
                        {
                            Subject = item.Subject;
                            Emails = item.Email;
                            //   هنا سنقوم بارسال الايميلات
                            await emailService.SendEmailWithAttachmentAsync(Emails, Subject, Msgtxt.ToString());
                        }
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
                                    //   SendSMS(MobilesItem, False, Subject, True)

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

    }

}

