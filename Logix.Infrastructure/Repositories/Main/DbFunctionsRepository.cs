using Castle.Windsor.Installer;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Polly;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace Logix.Infrastructure.Repositories.Main
{
    public class DbFunctionsRepository : IDbFunctionsRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;

        public DbFunctionsRepository(ApplicationDbContext context, IConfiguration _configuration)
        {
            this.context = context;
            this.configuration = _configuration;

        }

        public async Task<int> DateDiff_day2(string SDate, string EDate)
        {
            int result = 0;

            using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
            {
                await objCnn.OpenAsync();
                using (var command = objCnn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT dbo.DateDiff_day2(@SDate, @EDate)";
                    command.Parameters.Add(new SqlParameter("@SDate", SDate));
                    command.Parameters.Add(new SqlParameter("@EDate", EDate));
                    var obj = await command.ExecuteScalarAsync();

                    if (obj != null)
                    {
                        result = Convert.ToInt32(obj);
                    }
                }
            }

            return result;

        }

        public async Task<decimal> ApplyPolicies(long facilityId, long policyId, long employeeId)
        {
            decimal result = 0;

            using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
            {
                await objCnn.OpenAsync();

                using (var command = objCnn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT dbo.HR_Apply_Policies_Fn(@Facility_ID, @Policie_ID, @Emp_ID)";
                    command.Parameters.Add(new SqlParameter("@Facility_ID", facilityId));
                    command.Parameters.Add(new SqlParameter("@Policie_ID", policyId));
                    command.Parameters.Add(new SqlParameter("@Emp_ID", employeeId));
                    var obj = await command.ExecuteScalarAsync();

                    if (obj != null)
                    {
                        result = Convert.ToDecimal(obj);
                    }
                }
            }

            return result;
        }

        public async Task<List<string>> HR_Get_childe_Department_Fn( long DepId)
        {
            List<string> result =new  List<string>();

            using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
            {
                await objCnn.OpenAsync();

                using (var command = objCnn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT dbo.HR_Get_childe_Department_Fn(@Dep_ID)";
                    command.Parameters.Add(new SqlParameter("@Dep_ID", DepId));

                    var obj = await command.ExecuteScalarAsync();

                    if (obj != null)
                    {
                        result = obj.ToString().Split(",").ToList();
                    }
                }
            }

            return result;
        }


        public async Task<string> Steps_User_Fn(long userId, int appTypeId, int type)
        {
            // Initialize variables
            string wfSteps = string.Empty;
            string groupsId = string.Empty;

            // Get Groups ID from SYS_USER table for the given userId
            var user = await context.SysUsers
                        .Where(u => u.UserPkId == userId)
                        .Select(u => u.GroupsId)
                        .FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(user))
            {
                groupsId += user + ",";
            }

            string authEmpIds = string.Empty;
            string curDate = DateTime.Now.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture); // Get current date in yyyy/MM/dd format

            // Check the calendar type and adjust current date if needed
            var calendarType = await context.SysPropertyValues
                                .Where(spv => spv.FacilityId == 1 && spv.PropertyId == 19)
                                .Select(spv => spv.PropertyValue)
                                .FirstOrDefaultAsync();

            if (calendarType == "2")
            {
                var sysCalendar = await context.SysCalendars
                                    .Where(sc => sc.GDate == curDate)
                                    .Select(sc => sc.HDate)
                                    .FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(sysCalendar))
                {
                    curDate = sysCalendar; // Convert to Hijri date if applicable
                }
            }

            // Get employee ID for the user
            var employeeId = await context.SysUsers
                             .Where(u => u.UserPkId == userId)
                             .Select(u => u.EmpId)
                             .FirstOrDefaultAsync();

            // Get all employees who authorized the user within the current date period
            var authorizedEmpList = await context.HrAuthorizationVws
                                     .Where(auth => !auth.IsDeleted &&
                                                    auth.DelegateEmpId == employeeId &&
                                                    curDate.CompareTo(auth.StartDate) >= 0 &&
                                                    curDate.CompareTo(auth.EndDate) <= 0)
                                     .Select(auth => auth.EmpId.ToString())
                                     .ToListAsync();

            // Concatenate all authorized employee IDs
            if (authorizedEmpList.Any())
            {
                authEmpIds = string.Join(",", authorizedEmpList);
            }

            // Get group IDs for authorized employees and add to groupsId
            if (!string.IsNullOrEmpty(authEmpIds))
            {

                // Split the authEmpIds into an array before using it in the LINQ query
                var authEmpIdArray = authEmpIds.Split(',');

                var authorizedGroupIds = await context.SysUsers
                                       .Where(u => authEmpIdArray.Contains(u.EmpId.ToString()))
                                       .Select(u => u.GroupsId)
                                       .ToListAsync();


                groupsId += string.Join(",", authorizedGroupIds) + ",";
            }

            if (!string.IsNullOrEmpty(groupsId))
            {
                // Remove last comma from groupsId
                groupsId = groupsId.TrimEnd(',');

                var groupArray = groupsId.Split(','); // Explicit array for Contains()

                if (type == 1)
                {
                    // Fetch From_Step_ID based on conditions
                    var fromStepIds = await context.WfStepsTransactions
                                       .Where(wf => wf.IsDeleted == false && wf.AppTypeId == appTypeId &&
                                                    groupArray.Contains(wf.GroupsId))
                                       .Select(wf => wf.FromStepId.ToString())
                                       .ToListAsync();

                    wfSteps = string.Join(",", fromStepIds);
                }
                else if (type == 2)
                {
                    // Fetch To_Step_ID based on conditions
                    var toStepIds = await context.WfStepsTransactions
                                     .Where(wf => wf.IsDeleted == false && wf.AppTypeId == appTypeId &&
                                                  groupArray.Contains(wf.GroupsId))
                                     .Select(wf => wf.ToStepId.ToString())
                                     .ToListAsync();

                    wfSteps = string.Join(",", toStepIds);
                }

                // Remove last comma from wfSteps
                if (!string.IsNullOrEmpty(wfSteps))
                {
                    wfSteps = wfSteps.TrimEnd(',');
                }
            }

            return wfSteps;
        }


        public async Task<decimal> HR_Ticket_Balance_Fn(string CurrentDate, long EmpId)
        {
            decimal result = 0;
            using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
            {
                await objCnn.OpenAsync();

                using (var command = objCnn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select dbo.HR_Ticket_Balance_Fn(@Curr_Date,@Emp_ID) as Balance";
                    command.Parameters.Add(new SqlParameter("@Curr_Date", CurrentDate));
                    command.Parameters.Add(new SqlParameter("@Emp_ID", EmpId));
                    var obj = await command.ExecuteScalarAsync();

                    if (obj != null)
                    {
                        result = Convert.ToDecimal(obj);
                    }
                }
            }

            return result;
        }

        public async Task<decimal> HR_Visa_Balance_Fn(string CurrentDate, long EmpId)
        {
            decimal result = 0;
            using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
            {
                await objCnn.OpenAsync();

                using (var command = objCnn.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select dbo.HR_Visa_Balance_Fn(@Curr_Date,@Emp_ID) as Balance";
                    command.Parameters.Add(new SqlParameter("@Curr_Date", CurrentDate));
                    command.Parameters.Add(new SqlParameter("@Emp_ID", EmpId));
                    var obj = await command.ExecuteScalarAsync();

                    if (obj != null)
                    {
                        result = Convert.ToDecimal(obj);
                    }
                }
            }

            return result;
        }
    }
}
