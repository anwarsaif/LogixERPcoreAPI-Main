using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.DTOs.WF;
using Logix.Application.Interfaces.IServices;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Services;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SqlParameter = Microsoft.Data.SqlClient.SqlParameter;

namespace Logix.MVC.Helpers
{
    public interface IWorkflowSendHelper
    {
        Task<long> SaveFiles(long Applicants_ID, long Screen_ID, string App_Type_ID, long Alternative_Emp_ID = 0);
    }

    public class WorkflowSendHelper : IWorkflowSendHelper
    {
        private readonly IMainServiceManager mainServiceManager;
        private readonly IHttpContextAccessor httpContextAccessor1;
        private readonly IWFServiceManager wFServiceManager;
        private readonly ISysCustomerFileService customerFileService;
        private readonly IWebHostEnvironment env;
        private readonly ISession _session;
        private readonly IHttpContextAccessor httpContextAccessor;

        public WorkflowSendHelper(IMainServiceManager mainServiceManager, IWFServiceManager wFServiceManager, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            this.mainServiceManager = mainServiceManager;
            this.wFServiceManager = wFServiceManager;
            this.env = env;
            this._session = httpContextAccessor.HttpContext.Session;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<long> SaveFiles(long Applicants_ID, long Screen_ID, string App_Type_ID, long Alternative_Emp_ID = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(App_Type_ID))
                {
                    var WorkflowId = await mainServiceManager.SysScreenWorkflowService.GetOne(s => s.WorkflowId, x => x.ScreenId == Screen_ID);
                }

                if (!string.IsNullOrEmpty(App_Type_ID))
                {
                    var WfApplicationDto = new WfApplicationDto();
                    WfApplicationDto.ApplicantsId = Applicants_ID;
                    WfApplicationDto.ApplicationsTypeId = int.Parse(App_Type_ID);
                    WfApplicationDto.ApplicationDate = DateTime.Now.ToString("yyyy/MM/dd");
                    WfApplicationDto.StatusId = 2;
                    WfApplicationDto.CreatedBy = 0;
                    WfApplicationDto.StepId = 1;
                    WfApplicationDto.BranchId = 0;
                    WfApplicationDto.AlternativeEmpId = Alternative_Emp_ID;
                    var addRes = await wFServiceManager.WfApplicationService.Add(WfApplicationDto);
                    Applicants_ID = addRes.Data.Id;
                }
                Dictionary<Guid, Microsoft.Data.SqlClient.SqlParameter> AttributeValues = new Dictionary<Guid, Microsoft.Data.SqlClient.SqlParameter>();
                var DynamicAttributes = await wFServiceManager.WfDynamicAttributeService.GetAllVW(x => x.AppTypeId == int.Parse(App_Type_ID));
                if (DynamicAttributes.Succeeded)
                {
                    foreach (var attr in DynamicAttributes.Data)
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



        // ... الاستيرادات الأخرى

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
    }
}