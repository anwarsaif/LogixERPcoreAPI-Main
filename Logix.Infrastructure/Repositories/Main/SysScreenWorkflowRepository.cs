using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Logix.Infrastructure.Repositories.Main
{


    public class SysScreenWorkflowRepository : GenericRepository<SysScreenWorkflow>, ISysScreenWorkflowRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ICurrentData currentData;

        public SysScreenWorkflowRepository(ApplicationDbContext context, ICurrentData currentData) : base(context)
        {
            this.context = context;
            this.currentData = currentData;
        }

        public async Task<IEnumerable<SysScreenWorkflowDto>> GetScreenWorkflowByScreen(long screenId)
        {
            var items = await context.SysScreenWorkflows
                                     .Where(x => x.ScreenId == screenId)
                                     .ToListAsync();

            var result = items.Select(x => new SysScreenWorkflowDto
            {
                Id = x.Id,
                ScreenId = x.ScreenId,
                WorkflowId = x.WorkflowId,
                Description = x.Description,
                IsDeleted = x.IsDeleted,
                CreatedBy = x.CreatedBy,
                ModifiedBy = x.ModifiedBy,
                CreatedOn = x.CreatedOn,
                ModifiedOn = x.ModifiedOn
            });

            return result;
        }
        public async Task<IEnumerable<DynamicAttributeDto>> GetAttributes(long screenId, long? appTypeId, int? stepId = null)
        {
            // المرحلة الأولى: استخراج App_Type_ID من SysScreenWorkflow إذا لم يُرسل
            if (appTypeId == null)
            {
                var workflow = await context.SysScreenWorkflows
                                            .Where(x => x.ScreenId == screenId)
                                            .FirstOrDefaultAsync();

                if (workflow == null)
                    return Enumerable.Empty<DynamicAttributeDto>();

                appTypeId = workflow.WorkflowId;
            }

            // المرحلة الثانية: استعلام على View WF_DynamicAttributes_VW
            var query = context.WfDynamicAttributesVws
                               .Where(v => v.AppTypeId == appTypeId && v.IsDeleted == false);

            if (stepId.HasValue)
                query = query.Where(v => v.StepId == stepId.Value);

            var result = await query
                .OrderBy(v => v.SortOrder)
                .Select(v => new DynamicAttributeDto
                {
                    DynamicAttributeId = v.DynamicAttributeId,
                    DataTypeId = (DataTypeIdEnum)v.DataTypeId,
                    AttributeName = (currentData.Language == 1 ? v.AttributeName : v.AttributeName2),
                    Required = v.Required,
                    LookUpCatagoriesId = v.LookUpCatagoriesId,
                    SysLookUpCatagoriesId = v.SysLookUpCatagoriesId,
                    LookUpSql = v.LookUpSql,
                    LookUpType = v.LookUpType,
                    TableId = v.TableId,
                    DefaultValue = v.DefaultValue,
                    IsReadOnly = v.IsReadOnly,
                    LayoutSpan = v.LayoutSpan,
                    UserControlID = v.UserControlID,
                    UserControlPath = v.UserControlPath
                })
                .ToListAsync();

            return result;
        }
        public async Task<IEnumerable<DynamicAttributeValueDto>> GetAttributeValues(long screenId, long appId, long? appTypeId = null)
        {
            // الخطوة 1: استخراج AppTypeId إن لم يُرسل
            if (appTypeId == null)
            {
                var workflow = await context.SysScreenWorkflows
                    .FirstOrDefaultAsync(x => x.ScreenId == screenId);

                if (workflow == null)
                    return Enumerable.Empty<DynamicAttributeValueDto>();

                appTypeId = workflow.WorkflowId;
            }

            // الخطوة 2: الاستعلام المكافئ لـ CROSS APPLY + JOIN + الشروط
            var query = from attr in context.WfDynamicAttributes
                        where attr.IsDeleted == false && attr.AppTypeId == appTypeId
                        let latestValue = context.WfDynamicValues
                            .Where(v => v.AttributeId == attr.DynamicAttributeId && v.ApplicationId == appId)
                            .OrderByDescending(v => v.CreatedOn)
                            .FirstOrDefault()
                        join control in context.WFFormsControls
                            on attr.UserControlID equals (int)control.Id into controlJoin
                        from control in controlJoin.DefaultIfEmpty()
                        select new DynamicAttributeValueDto
                        {
                            DynamicAttributeId = attr.DynamicAttributeId,
                            DataTypeId = (DataTypeIdEnum)attr.DataTypeId,
                            AttributeName = currentData.Language == 1 ? attr.AttributeName : attr.AttributeName2,
                            Required = attr.Required,
                            LookUpCatagoriesId = attr.LookUpCatagoriesId,
                            SysLookUpCatagoriesId = attr.SysLookUpCatagoriesId,
                            LookUpSql = attr.LookUpSql,
                            LookUpType = attr.LookUpType,
                            AttributeValue = latestValue != null ? latestValue.DynamicValue : null,
                            LayoutSpan = context.WfLayoutAttributes
                                          .Where(l => l.Id == attr.LayoutAttributeId)
                                          .Select(l => l.LayoutSpan)
                                          .FirstOrDefault(),
                            UserControlID = attr.UserControlID,
                            UserControlPath = control != null ? control.UserControlPath : null,
                            TableId = attr.TableId,
                            SortOrder = attr.SortOrder
                        };

            return await query.OrderBy(x => x.SortOrder).ToListAsync();
        }


        public async Task<string?> GetDefaultValue(string defaultValue, long? empId, string currDate, long? facilityId = 1, long? appId = null, long? finYear = null)
        {
            if (string.IsNullOrEmpty(defaultValue) || empId == null)
                return null;

            facilityId ??= 1;

            async Task<string?> ExecuteScalarFunction(string sql)
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = sql;
                command.CommandType = System.Data.CommandType.Text;

                if (command.Connection.State != System.Data.ConnectionState.Open)
                    await command.Connection.OpenAsync();

                var result = await command.ExecuteScalarAsync();
                return result?.ToString();
            }

            switch (defaultValue)
            {
                case "Vacation_Balance":
                    return await ExecuteScalarFunction($"SELECT dbo.HR_Vacation_Balance_FN('{currDate}', {empId})");

                case "Iast_Start_Work":
                    return await context.HrDirectJobVws
                        .Where(x => !x.IsDeleted && x.EmpId == empId.Value) // EmpId هو long؟ لذلك قارن مع empId.Value
                        .OrderByDescending(x => x.DateDirect)
                        .Select(x => x.DateDirect)
                        .FirstOrDefaultAsync();

                case "Loan_EndOfService":
                    var loanSum = await context.HrLoans
                        .Where(x => x.IsDeleted == false && x.Type == 2 && x.EmpId == empId.Value.ToString()) // EmpId هنا نص، لذلك حول الـ long لـ string
                        .SumAsync(x => (decimal?)x.LoanValue);
                    return loanSum?.ToString();

                case "Amount_EndOfService":
                    return await ExecuteScalarFunction($"SELECT CAST(ISNULL(dbo.HR_End_Service_Due({empId}, '{currDate}', 2), 0) AS NVARCHAR(50))");

                case "HalfAmount_EndOfService":
                    return await ExecuteScalarFunction($"SELECT CAST(ISNULL(dbo.HR_End_Service_Due({empId}, '{currDate}', 2), 0) / 2 AS NVARCHAR(50))");

                case "BasicSalary":
                    var salary = await context.HrEmployees
                        .Where(e => e.Id == empId)
                        .Select(e => e.Salary)
                        .FirstOrDefaultAsync();
                    return salary?.ToString();

                case "HousingAllowance":
                    var housing = await context.HrAllowanceDeductions
                        .Where(x => x.EmpId == empId && !x.IsDeleted && x.FixedOrTemporary == 1 && x.TypeId == 1 && x.AdId == 1)
                        .OrderByDescending(x => x.Id)
                        .Select(x => x.Amount * 12)
                        .FirstOrDefaultAsync();
                    return housing.ToString();

                case "ContractSDate":
                    var contractStart = await context.HrEmployees
                        .Where(e => e.Id == empId)
                        .Select(e => e.ContarctDate)  // تأكد أن الخاصية اسمها ContractDate أو عدلها للاسم الصحيح في كلاس HrEmployee
                        .FirstOrDefaultAsync();

                    return contractStart;

                case "ContractEDate":
                    var contractEnd = await context.HrEmployees
                        .Where(e => e.Id == empId)
                        .Select(e => e.ContractExpiryDate)  // تأكد من اسم الخاصية الصحيح
                        .FirstOrDefaultAsync();

                    return contractEnd;

                case "Visa_Balance":
                    return await ExecuteScalarFunction($"SELECT dbo.HR_Visa_Balance_Fn('{currDate}', {empId})");

                case "Ticket_Balance":
                    return await ExecuteScalarFunction($"SELECT dbo.HR_Ticket_Balance_Fn('{currDate}', {empId})");

                case "Permission_Balance":
                    return await GetPermissionBalance(empId.Value, currDate);

                case "Business_Trip_Amount":
                    var levelId = await context.HrEmployees
                        .Where(e => e.Id == empId)
                        .Select(e => e.LevelId)
                        .FirstOrDefaultAsync();

                    var mandate = await context.HrJobLevels
                        .Where(x => x.Id == levelId)
                        .Select(x => x.Mandate ?? 0)
                        .FirstOrDefaultAsync();
                    return mandate.ToString();

                case "Total_Salary":
                case "Net_Salary":
                    return await ExecuteScalarFunction($"SELECT dbo.HR_GetTotal_Salary({empId})");

                case "Mobile":
                    return await context.HrEmployees
                        .Where(e => e.Id == empId)
                        .Select(e => e.Mobile)
                        .FirstOrDefaultAsync();

                case "ID_No":
                    return await context.HrEmployees
                        .Where(e => e.Id == empId)
                        .Select(e => e.IdNo)
                        .FirstOrDefaultAsync();

                default:
                    return defaultValue;
            }
        }
        private async Task<string?> GetPermissionBalance(long empId, string currDate)
        {
            var monthlyLimitString = await context.SysPropertyValues
     .Where(p => p.PropertyId == 66 && p.FacilityId == 1)
     .Select(p => p.PropertyValue)
     .FirstOrDefaultAsync();

            int monthlyLimit = 0;

            if (!string.IsNullOrEmpty(monthlyLimitString) && int.TryParse(monthlyLimitString, out int parsedValue))
            {
                monthlyLimit = parsedValue;
            }


            //var currentCount = await context.Hr_Permissions_VW
            //    .CountAsync(x => x.EmpId == empId && x.PermissionDate.StartsWith(currDate.Substring(0, 8)));

            //var remaining = monthlyLimit - currentCount;
            //return remaining <= 0 ? null : remaining.ToString();
            return null;
        }


    }
}
