using System.Data;
using System.Globalization;
using Logix.Application.Common;
using Logix.Application.DTOs.ACC;
using Logix.Application.DTOs.HR;
using Logix.Application.DTOs.Main;
using Logix.Application.DTOs.WF;
using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Logix.Infrastructure.Repositories.Main
{
    public class StoredProceduresRepository : IStoredProceduresRepository
    {
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;
        private readonly ICurrentData currentData;

        public StoredProceduresRepository(ApplicationDbContext context, ICurrentData currentData, IConfiguration _configuration)
        {
            this.context = context;
            this.currentData = currentData;
            this.configuration = _configuration;
        }
        public async Task<IEnumerable<SendEmailDto>> WF_UsersEmail_App_Sp(long id)
        {
            List<SendEmailDto> result = new List<SendEmailDto>();

            try
            {
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                {
                    await objCnn.OpenAsync();

                    using (SqlCommand command = new SqlCommand("WF_UsersEmail_App_Sp", objCnn))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@App_ID", id));
                        command.CommandTimeout = 600;
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            foreach (DataRow row in dataTable.Rows)
                            {
                                SendEmailDto reportDto = new SendEmailDto
                                {
                                    Email = Convert.ToString(row[0]),
                                    Subject = Convert.ToString(row[1]),
                                };
                                result.Add(reportDto);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return result;

        }
        public async Task<IEnumerable<HrVacationBalanceALLFilterDto>> HR_VacationBalance_SP(HrVacationBalanceALLSendFilterDto filter)
        {
            List<HrVacationBalanceALLFilterDto> result = new List<HrVacationBalanceALLFilterDto>();

            try
            {
                var query = context.Set<HrVacationBalanceALLFilterDto>()
                    .FromSqlRaw("exec HR_VacationBalance_SP @Emp_Id={0}, @Emp_name={1}, @BRANCH_ID={2}, @BRANCHS_ID={3}, @Curr_Date={4}, @Vacation_Type_Id={5}, @Facility_ID={6}, @Location={7}, @Dept_ID={8}, @Nationality_ID={9}, @Status_ID={10}, @Job_Catagories_ID={11}, @CMDTYPE={12}",
                        filter.EmpId,
                        filter.EmpName,
                        filter.BranchId ?? 0,
                        filter.BranchsId,
                        filter.CurrentDate,
                        filter.VacationTypeId ?? 0,
                        filter.FacilityId ?? 0,
                        filter.Location ?? 0,
                        filter.DeptId ?? 0,
                        filter.NationalityId ?? 0,
                        filter.StatusId ?? 0,
                        filter.JobCatagoriesId ?? 0,
                        7)
                    .AsEnumerable()
                    .ToList();

                result.AddRange(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return result;
        }



        public async Task<HrVacationEmpBalanceDto> HR_Vacation_Balance(HrVacationEmpBalanceDto filter)
        {
            HrVacationEmpBalanceDto result = new();

            try
            {
                var r = context.Set<HrVacationEmpBalanceDto>()
                    .FromSqlRaw("exec HR_Vacation_Balance @Emp_ID={0}, @CurrentDate={1}", filter.Emp_ID, filter.Currentdate ?? "")
                    .AsEnumerable().ToList();
                if (r.Count > 0)
                {
                    result = r[0];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return result;
        }

        public async Task<IEnumerable<HrVacationEmpBalanceDto>> HR_Vacation_Balance_Search(HrVacationEmpBalanceDto filter)
        {
            List<HrVacationEmpBalanceDto> result = new List<HrVacationEmpBalanceDto>();

            try
            {
                var r = context.Set<HrVacationEmpBalanceDto>()
                    .FromSqlRaw("exec HR_Vacation_Balance @Emp_ID={0}, @CurrentDate={1}",
                        filter.Emp_ID,
                        filter.Currentdate ?? "")
                    .AsEnumerable()
                    .ToList();

                result.AddRange(r);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return result;
        }


        //  شغال
        public async Task<long> HR_Payroll_D_SP(HRPayrollDStoredProcedureDto entity)
        {

            try
            {
                var returnParameter = new SqlParameter("@return_value", SqlDbType.BigInt)
                {
                    Direction = ParameterDirection.Output
                };
                var oo = context.Database.ExecuteSqlRaw(" EXEC  HR_Payroll_D_SP  @MS_ID = {0}, @Emp_ID = {1},  @Emp_Account_No = {2}, @BankId = {3}, @Loan = {4}, @Net = {5}, @CreatedBy = {6}, @Commission = {7}, @allowance = {8}, @Deduction = {9}, @Absence = {10}, @Delay = {11}, @Salary = {12}, @Salary_Orignal = {13}, @Count_Day_Work = {14}, @Penalties = {15} ,@OverTime = {16}, @H_OverTime = {17}, @Mandate = {18}, @Refrance_No = {19}, @CMDTYPE = {20}"
                   , entity.MS_ID,//0
                    entity.Emp_ID,//1
                    entity.Emp_Account_No,//2
                    entity.BankId,//3
                    entity.Loan, //4
                    entity.Net,//5
                    entity.CreatedBy,//6
                    entity.Commission,//7
                    entity.Allowance,//8
                    entity.Deduction,//9
                    entity.Absence,//10
                    entity.Delay,//11
                    entity.Salary,//12
                    entity.Salary_Orignal ?? 0,//13
                    entity.Count_Day_Work,//14
                    entity.Penalties,//15
                    entity.OverTime,//16
                    entity.H_OverTime,//17
                    entity.Mandate,//18
                    entity.Refrance_No,//19
                    entity.CMDTYPE//20
                    );

                int output = oo;
                return Convert.ToInt64(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
            throw new NotImplementedException();
        }




        public async Task<IEnumerable<HRAttendanceReportDto>> HR_Attendance_Report_SP(HRAttendanceReportFilterDto filter)
        {
            List<HRAttendanceReportDto> result = new List<HRAttendanceReportDto>();

            try
            {
                var query = context.Set<HRAttendanceReportDto>()
                    .FromSqlRaw("exec HR_Attendance_Report_SP @Emp_Code={0}, @Emp_name={1}, @BRANCH_ID={2}, @BRANCHS_ID={3}, @TimeTable_ID={4}, @Day_Date_Gregorian={5}, @Day_Date_Gregorian2={6}, @Location={7}, @Dept_ID={8}, @Status_ID={9}, @Attendance_Type={10}, @Sponsors_ID={11},@Manager_ID={12},@Shit_ID={13}",
                        filter.EmpCode ?? (object)DBNull.Value,//0
                        filter.EmpName ?? (object)DBNull.Value,//1
                        filter.BranchId ?? 0,//2
                        filter.BranchsId ?? (object)DBNull.Value,//3
                        filter.TimeTableId ?? 0,//4
                        filter.DayDateGregorian ?? (object)DBNull.Value,//5
                        filter.DayDateGregorian2 ?? (object)DBNull.Value,//6
                        filter.Location ?? (object)DBNull.Value,//7
                        filter.DeptId ?? (object)DBNull.Value,//8
                        filter.StatusId ?? (object)DBNull.Value,//9
                        filter.AttendanceType ?? (object)DBNull.Value,//10
                        filter.SponsorsId ?? (object)DBNull.Value,//11
                        filter.ManagerId ?? 0,//12
                        filter.ShitId ?? 0,//13
                        7)
                    .AsEnumerable()
                    .ToList();

                result.AddRange(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return result;
        }

        public async Task<IEnumerable<HRAttendanceTotalReportSPDto>> HR_Attendance_TotalReport_SP(HRAttendanceTotalReportSPFilterDto filter)
        {
            // Fetch logic using stored procedure
            try
            {
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    filter.BranchID ??= 0;
                    filter.DeptID ??= 0;
                    filter.facilityId ??= 0;
                    filter.Location ??= 0;
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "HR_Attendance_TotalReport_SP";

                    objCmd.Parameters.AddWithValue("@BRANCH_ID", filter.BranchID);
                    objCmd.Parameters.AddWithValue("@BRANCHS_ID", filter.BranchsId);
                    objCmd.Parameters.AddWithValue("@Dept_ID", filter.DeptID);
                    objCmd.Parameters.AddWithValue("@Location", filter.Location);
                    objCmd.Parameters.AddWithValue("@Facility_ID", filter.facilityId);
                    objCmd.Parameters.AddWithValue("@Emp_ID", filter.EmpCode ?? "");
                    objCmd.Parameters.AddWithValue("@Emp_Name", filter.EmpName ?? "");
                    objCmd.Parameters.AddWithValue("@Satrt_Date", filter.FromDate);
                    objCmd.Parameters.AddWithValue("@End_Date", filter.ToDate);
                    objCmd.Parameters.AddWithValue("@CMDTYPE", 1);

                    objCmd.CommandTimeout = 600;

                    await objCnn.OpenAsync();
                    var result = objCmd.ExecuteReader();
                    DataTable dt = new();
                    dt.Load(result);

                    var list = new List<HRAttendanceTotalReportSPDto>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var item = new HRAttendanceTotalReportSPDto
                        {
                            EmpCode = row["Emp_ID"].ToString(),
                            EmpName = row["Emp_Name"].ToString(),
                            DeptName = row["Dep_name"].ToString(),
                            LocationName = row["Location_Name"].ToString(),
                            Absence = row["Absence"].ToString() == null ? 0 : Convert.ToInt64(row["Absence"].ToString()),
                            Delay2 = row["Delay2"].ToString() == null ? "0" : row["Delay2"].ToString(),
                            H_OverTime = row["H_OverTime"].ToString() ?? "0"

                        };
                        list.Add(item);
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching dashboard data: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<HRApprovalAbsencesReportDto>> HRApprovalAbsencesReport(HRApprovalAbsencesReportFilterDto filter)
        {
            // Fetch logic using stored procedure
            try
            {
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    filter.BranchID ??= 0;
                    filter.DeptID ??= 0;
                    filter.facilityId ??= 0;
                    filter.Location ??= 0;
                    filter.NationalityType ??= 0;
                    filter.GroupShiftId ??= 0;
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "HR_Attendance_TotalReport_SP";

                    objCmd.Parameters.AddWithValue("@BRANCH_ID", filter.BranchID);
                    objCmd.Parameters.AddWithValue("@BRANCHS_ID", filter.BranchsId);
                    objCmd.Parameters.AddWithValue("@Dept_ID", filter.DeptID);
                    objCmd.Parameters.AddWithValue("@Location", filter.Location);
                    objCmd.Parameters.AddWithValue("@Facility_ID", filter.facilityId);
                    objCmd.Parameters.AddWithValue("@Emp_ID", filter.EmpCode ?? "");
                    objCmd.Parameters.AddWithValue("@Emp_Name", filter.EmpName ?? "");
                    objCmd.Parameters.AddWithValue("@Satrt_Date", filter.FromDate);
                    objCmd.Parameters.AddWithValue("@End_Date", filter.ToDate);
                    objCmd.Parameters.AddWithValue("@Nationality_Type", filter.NationalityType);
                    objCmd.Parameters.AddWithValue("@Group_Shift_ID", filter.GroupShiftId);
                    objCmd.Parameters.AddWithValue("@CMDTYPE", 1);

                    objCmd.CommandTimeout = 600;

                    await objCnn.OpenAsync();
                    var result = objCmd.ExecuteReader();
                    DataTable dt = new();
                    dt.Load(result);

                    var list = new List<HRApprovalAbsencesReportDto>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var item = new HRApprovalAbsencesReportDto
                        {
                            EmpCode = row["Emp_ID"].ToString(),
                            EmpName = currentData.Language == 1 ? row["Emp_name"].ToString() : row["Emp_name2"].ToString(),
                            CatName = currentData.Language == 1 ? row["cat_name"].ToString() : row["cat_name2"].ToString(),
                            NationalityName = currentData.Language == 1 ? row["Nationality_Name"].ToString() : row["Nationality_Name2"].ToString(),
                            DeptName = currentData.Language == 1 ? row["Dep_Name"].ToString() : row["Dep_Name2"].ToString(),
                            LocationName = currentData.Language == 1 ? row["Location_Name"].ToString() : row["Location_Name2"].ToString(),
                            MonthDay = row["Month_Day"].ToString() ?? "0",
                            Attendances = row["Attendances"].ToString() == null ? 0 : Convert.ToInt64(row["Attendances"].ToString()),
                            OffDays = row["Off_days"].ToString() == null ? 0 : Convert.ToInt64(row["Off_days"].ToString()),
                            //VacationDays = row["Vacation_days"].ToString() == null ? 0 : Convert.ToInt64(row["Vacation_days"].ToString()),
                            VacationDays = await CalculateVacationCountAsync(Convert.ToInt64(row["ID"].ToString()), filter.FromDate, filter.ToDate),
                            AbsenceNotRecorded = Convert.ToInt64(row["Month_Day"].ToString()) - Convert.ToInt64(row["Attendances"].ToString()) - Convert.ToInt64(row["Absence"].ToString()) - Convert.ToInt64(row["Off_days"].ToString()) - Convert.ToInt64(row["Vacation_days"].ToString()),
                            Absence = row["Absence"].ToString() == null ? 0 : Convert.ToInt64(row["Absence"].ToString())
                        };
                        list.Add(item);
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching dashboard data: {ex.Message}", ex);
            }
        }



        public async Task<int> CalculateVacationCountAsync(long Emp_ID, string fromDate, string toDate)
        {
            int checkVacation = 0;
            long vacationTypeId = 0;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var TimpDate = DateTime.ParseExact(fromDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            var EndDate = DateTime.ParseExact(toDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);

            while (EndDate > TimpDate)
            {
                //var CheckIsVacationsInSameDay3 = context.HrVacations.Where(x => x.EmpId == Emp_ID && x.IsDeleted == false && (!string.IsNullOrEmpty(x.VacationSdate) && (
                //    TimpDate >= DateTime.ParseExact(x.VacationSdate, "yyyy/MM/dd", CultureInfo.InvariantCulture) &&
                //    TimpDate <= DateTime.ParseExact(x.VacationSdate, "yyyy/MM/dd", CultureInfo.InvariantCulture)))).ToList();
                var allVacations = await context.HrVacations
           .Where(x => x.EmpId == Emp_ID && x.IsDeleted == false && !string.IsNullOrEmpty(x.VacationSdate) && !string.IsNullOrEmpty(x.VacationEdate))
           .ToListAsync(); // Pull to memory

                var vacationsInDay = allVacations
                    .Where(x =>
                    {
                        var vacStart = DateTime.ParseExact(x.VacationSdate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                        var vacEnd = DateTime.ParseExact(x.VacationEdate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                        return TimpDate.Date >= vacStart.Date && TimpDate.Date <= vacEnd.Date;
                    })
                    .ToList();
                if (vacationsInDay.Count > 0)
                {
                    vacationTypeId = vacationsInDay.First().VacationTypeId ?? 0;

                    string getCountDays = await GetCountDays(TimpDate, TimpDate, vacationTypeId);
                    if (int.TryParse(getCountDays, out int countDays) && countDays == 0)
                    {
                        checkVacation += 1;
                    }
                }
                TimpDate = TimpDate.AddDays(1);
            }

            return checkVacation;
        }

        public async Task<string> GetCountDays(DateTime sDate, DateTime eDate, long Vacation_Type_Id)
        {
            // Fetch logic using stored procedure
            try
            {
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "HR_VacationRequest_SP";

                    objCmd.Parameters.AddWithValue("@Vacation_Type_Id", Vacation_Type_Id);
                    objCmd.Parameters.AddWithValue("@SDate", sDate);
                    objCmd.Parameters.AddWithValue("@EDate", eDate);
                    objCmd.Parameters.AddWithValue("@CMDTYPE", 6);

                    objCmd.CommandTimeout = 600;

                    await objCnn.OpenAsync();
                    var result = objCmd.ExecuteReader();
                    DataTable dt = new();
                    dt.Load(result);

                    return dt.Rows.Count > 0 ? dt.Rows[0]["TotWeeks"]?.ToString() ?? "0" : "0";
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching dashboard data: {ex.Message}", ex);
            }
        }

        public async Task<bool> HR_Attendance_SP_CmdType_1(HrAttendanceDto entity)
        {
            try
            {
                int objRet = 0;
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                {
                    await objCnn.OpenAsync();

                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[HR_Attendance_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@Attendance_Id", entity.AttendanceId));
                        objCmd.Parameters.Add(new SqlParameter("@Emp_Id", entity.EmpId));
                        objCmd.Parameters.Add(new SqlParameter("@Time_In", entity.TimeIn));
                        objCmd.Parameters.Add(new SqlParameter("@Time_Out", entity.TimeOut));
                        objCmd.Parameters.Add(new SqlParameter("@Att_Type", entity.AttType));
                        objCmd.Parameters.Add(new SqlParameter("@Day_No", entity.DayNo));
                        objCmd.Parameters.Add(new SqlParameter("@Day_Date_Gregorian", entity.DayDateGregorian));
                        objCmd.Parameters.Add(new SqlParameter("@Day_Date_Hijri", entity.DayDateHijri));
                        objCmd.Parameters.Add(new SqlParameter("@CreatedBy", entity.CreatedBy));
                        objCmd.Parameters.Add(new SqlParameter("@LogInBy", entity.LogInBy));
                        objCmd.Parameters.Add(new SqlParameter("@LogOutBy", entity.LogOutBy));
                        objCmd.Parameters.Add(new SqlParameter("@Note_In", entity.NoteIn));
                        objCmd.Parameters.Add(new SqlParameter("@Note_Out", entity.NoteOut));
                        objCmd.Parameters.Add(new SqlParameter("@longitude", entity.Longitude));
                        objCmd.Parameters.Add(new SqlParameter("@Latitude", entity.Latitude));
                        objCmd.Parameters.Add(new SqlParameter("@longitude_out", entity.LongitudeOut));
                        objCmd.Parameters.Add(new SqlParameter("@Latitude_out", entity.LatitudeOut));

                        SqlParameter Sp = objCmd.Parameters.Add("@IsSave", SqlDbType.Bit);
                        Sp.Direction = ParameterDirection.Output;
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", 1));

                        object obj = objCmd.ExecuteScalar();
                        if (obj != null)
                        {
                            objRet = Convert.ToInt32(obj);
                        }
                        // Return the value of the output parameter directly
                        return (bool)Sp.Value;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<IEnumerable<HRAttendanceReport5Dto>> HR_Attendance_Report5_SP(HRAttendanceReport5FilterDto filter)
        {
            List<HRAttendanceReport5Dto> result = new List<HRAttendanceReport5Dto>();

            try
            {
                var query = context.Set<HRAttendanceReport5Dto>()
                    .FromSqlRaw("exec HR_Attendance_Report5_SP @Emp_ID={0}, @Start_Date={1}, @End_Date={2}, @Calendar_Type={3}, @Language={4}",
                        filter.EmpId ?? (object)DBNull.Value,//0
                        filter.From ?? (object)DBNull.Value,//1
                        filter.To ?? (object)DBNull.Value,//2
                        filter.CalendarType ?? (object)DBNull.Value,//3
                        filter.Language ?? 1//4

                        )
                    .AsEnumerable()
                    .ToList();

                result.AddRange(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return result;
        }

        public async Task<IEnumerable<HRAttendanceReport4Dto>> HR_Attendance_Report4_SP(HRAttendanceReport4FilterDto filter)
        {
            List<HRAttendanceReport4Dto> result = new List<HRAttendanceReport4Dto>();

            try
            {
                var query = context.Set<HRAttendanceReport4Dto>()
                    .FromSqlRaw("exec HR_Attendance_Report4_SP @BRANCH_ID={0}, @TimeTable_ID={1}, @Day_Date_Gregorian={2}, @Day_Date_Gregorian2={3}, @Emp_Code={4}, @Manager_ID={5}, @Shit_ID ={6}, @Status_ID ={7}, @Location ={8}, @Dept_ID ={9}, @Attendance_Type ={10}, @Sponsors_ID ={11}, @BRANCHS_ID={12}, @Emp_name={13}",
                 filter.BranchID ?? 0, // 0
                 filter.TimeTableID ?? 0, // 1
                 filter.DayDateGregorian, //2
                 filter.DayDateGregorian2, // 3
                 filter.EmpCode, // 4
                 filter.ManagerID ?? 0, // 5
                 filter.ShitID ?? 0, // 6
                 filter.StatusID ?? 0, // 7
                 filter.Location ?? 0, // 8
                 filter.DeptID ?? 0, // 9
                 filter.AttendanceType ?? 0, // 10
                 filter.SponsorsID ?? 0, // 11
                 filter.BranchsID ?? (object)DBNull.Value,
                 filter.EmpName ?? (object)DBNull.Value
                ).AsEnumerable().ToList();

                result.AddRange(query);
            }
            //System.InvalidCastException: 'Unable to cast object of type 'System.Boolean' to type 'System.DateTime'.'
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return result;
        }


        public async Task<IEnumerable<HRAddMultiAttendanceDto>> AttendanceSearchForMultiAdd(HRAddMultiAttendanceFilterDto filter)
        {
            List<HRAddMultiAttendanceDto> result = new List<HRAddMultiAttendanceDto>();

            try
            {
                var query = context.Set<HRAddMultiAttendanceDto>()
                    .FromSqlRaw(@"SELECT  Emp_ID, Emp_Name, Emp_Code2, Id, Dep_Name, Location_Name, Cat_name, Cat_name2,
                            ISNULL((SELECT TOP 1 1
                                    FROM HR_Attendances
                                    WHERE Day_Date_Gregorian = @Day_Date_Gregorian AND IsDeleted = 0 AND Emp_ID = HR_Employee_VW.ID), 0) AS Attend,
                            ISNULL((SELECT TOP 1 1
                                    FROM HR_Absence
                                    WHERE Absence_Date = @Day_Date_Gregorian AND IsDeleted = 0 AND Emp_ID = HR_Employee_VW.ID), 0) AS Absence,
                            ISNULL((SELECT TOP 1 1
                                    FROM HR_Vacations
                                    WHERE Vacation_SDate = @Day_Date_Gregorian AND IsDeleted = 0 AND Emp_ID = HR_Employee_VW.ID 
                                    AND Vacation_Type_Id = 8 AND Status_Id = 4), 0) AS Rest,
                            (SELECT TOP 1 Name
                                FROM HR_Att_Shift_Employee_VW
                                WHERE HR_Att_Shift_Employee_VW.Emp_ID = HR_Employee_VW.ID AND IsDeleted = 0
                                ORDER BY ID DESC) AS Shift_Name,
                            Emp_ID AS Emp_ID_Int
                    FROM HR_Employee_VW
                    WHERE ISDEL = 0 AND Status_ID NOT IN (2) AND 
                            CONVERT(NVARCHAR(10), DOAppointment, 131) <= @Day_Date_Gregorian AND 
                            (@BRANCH_ID = 0 OR BRANCH_ID = @BRANCH_ID  OR BRANCH_ID IN (SELECT value FROM dbo.fn_Split(@BranchesId, ','))) AND 
                            (@Emp_name IS NULL OR Emp_name LIKE N'%' + @Emp_name + '%') AND
                            (@Dept_ID = 0 OR Dept_ID = @Dept_ID) AND 
                            (@Emp_ID IS NULL OR Emp_ID = @Emp_ID) AND 
                            (@Location = 0 OR Location = @Location) AND
                            (@TimeTable_ID = 0 OR ID IN (
                                SELECT Emp_ID FROM HR_Att_Shift_TimeTable_Emp_VW WHERE TimeTable_ID = @TimeTable_ID AND IsDeleted = 0))
                    ORDER BY Location_Name ASC;",
                            new SqlParameter("@Day_Date_Gregorian", SqlDbType.NVarChar) { Value = filter.DayDateGregorian },
                            new SqlParameter("@BRANCH_ID", SqlDbType.Int) { Value = filter.BranchId },
                            new SqlParameter("@Emp_name", SqlDbType.NVarChar) { Value = filter.EmpName ?? (object)DBNull.Value },
                            new SqlParameter("@Dept_ID", SqlDbType.Int) { Value = filter.DeptId },
                            new SqlParameter("@Emp_ID", SqlDbType.NVarChar) { Value = filter.EmpCode ?? (object)DBNull.Value },
                            new SqlParameter("@Location", SqlDbType.Int) { Value = filter.Location },
                            new SqlParameter("@TimeTable_ID", SqlDbType.Int) { Value = filter.TimeTableId },
                            new SqlParameter("@BranchesId", SqlDbType.NVarChar) { Value = currentData.Branches }
                        )
                    .AsEnumerable()
                    .ToList();

                result.AddRange(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return result;
        }


        public async Task<long> HR_Sick_leave_Sp(long EmpID, long CmdType, long? VacationID)
        {
            try
            {
                var sql = "[HR_Sick_leave_Sp] @Vacation_ID, @Cmd_Type, @Emp_ID";
                var parameters = new object[]
                {
                    new SqlParameter("@Vacation_ID",VacationID),
                    new SqlParameter("@Cmd_Type", CmdType),
                    new SqlParameter("@Emp_ID", EmpID),
                };

                var outputId = await context.Database.ExecuteSqlRawAsync(sql, parameters);
                return outputId;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int Absence_NotAttendance(AbsenceNotAttendanceDto entity)
        {
            try
            {

                int objRet = 0;
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                {
                    objCnn.Open();
                    using (SqlCommand objCmd = objCnn.CreateCommand())
                    {
                        objCmd.CommandType = System.Data.CommandType.StoredProcedure;
                        objCmd.CommandText = "[HR_Absence_SP]";
                        objCmd.Parameters.Add(new SqlParameter("@StartDate", entity.FromDate));
                        objCmd.Parameters.Add(new SqlParameter("@EndDate", entity.ToDate));
                        objCmd.Parameters.Add(new SqlParameter("@CMDTYPE", entity.CMDTYPE));
                        objCmd.Parameters.Add(new SqlParameter("@ModifiedBy", currentData.UserId));
                        objCmd.Parameters.Add(new SqlParameter("@CreatedBy", currentData.UserId));
                        objCmd.Parameters.Add(new SqlParameter("@Facility_ID", currentData.FacilityId));
                        objCmd.Parameters.Add(new SqlParameter("@Location", entity.LocationId));
                        objCmd.Parameters.Add(new SqlParameter("@Dep_Manger_ID", entity.DepManagerID));
                        objCmd.Parameters.Add(new SqlParameter("@Branch_ID", entity.BranchId));
                        objCmd.CommandTimeout = 300;
                        object obj = objCmd.ExecuteNonQuery();
                        if (obj != null)
                        {
                            objRet = Convert.ToInt32(obj);
                        }
                        // Return the value of the output parameter directly
                        return objRet;
                    }
                }
                ;
            }

            catch (Exception ex)
            {

                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }
        public async Task<IEnumerable<HRPayrollManuallCreateSpDto>> HR_Payroll_Create2_Sp(HRPayrollCreate2SpFilterDto filter)
        {
            List<HRPayrollManuallCreateSpDto> result = new List<HRPayrollManuallCreateSpDto>();

            try
            {
                var query = context.Set<HRPayrollManuallCreateSpDto>()
                    .FromSqlRaw("exec HR_Payroll_Create2_Sp @MS_Month={0}, @FinancelYear={1}, @BRANCH_ID={2}, @Dept_ID={3}, @Location={4}, @Facility_ID={5}, @Nationality_ID={6}, @Emp_Code={7}, @Emp_Name={8}",
                        filter.MSMonth ?? (object)DBNull.Value,//0
                        filter.FinancelYear ?? (object)DBNull.Value,//1
                        filter.BRANCHID ?? 0,//2
                        filter.DeptID ?? 0,//3
                        filter.Location ?? 0,//4
                        filter.FacilityID ?? (object)DBNull.Value,//5
                        filter.NationalityID ?? 0,//6
                        filter.EmpCode ?? (object)DBNull.Value,//7
                        filter.EmpName ?? (object)DBNull.Value//8
)
                    .AsEnumerable()
                    .ToList();

                result.AddRange(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return result;
        }
        public async Task<IEnumerable<HRPayrollCreate2SpDto>> HR_Payroll_Create_Sp(HRPayrollCreateSpFilterDto filter)
        {
            List<HRPayrollCreate2SpDto> result = new List<HRPayrollCreate2SpDto>();

            try
            {
                var query = context.Set<HRPayrollCreate2SpDto>()
                    .FromSqlRaw("exec HR_Payroll_Create_Sp @MS_Month={0}, @FinancelYear={1}, @BRANCH_ID={2}, @Dept_ID={3}, @Location={4}, @Facility_ID={5}, @Emp_ID={6}, @Emp_Name={7}, @BRANCHS_ID={8}, @Salary_Group_ID={9}, @Contract_Type_ID={10}, @Payment_Type_ID={11}, @Sponsors_ID={12}, @Wages_Protection={13}",
                        filter.MSMonth ?? (object)DBNull.Value,//0
                        filter.FinancelYear ?? (object)DBNull.Value,//1
                        filter.BRANCHID ?? 0,//2
                        filter.DeptID ?? 0,//3
                        filter.Location ?? 0,//4
                        filter.FacilityID ?? 0,//5
                        filter.EmpCode ?? "",//6
                        filter.EmpName ?? "",//7
                        filter.Branches,//8
                        filter.SalaryGroupID ?? (object)DBNull.Value,//9
                        filter.ContractTypeID ?? (object)DBNull.Value,//10
                        filter.PaymentTypeID ?? (object)DBNull.Value,//11
                        filter.SponsorsID ?? (object)DBNull.Value,//12
                        filter.WagesProtection ?? (object)DBNull.Value//13
)
                    .AsEnumerable()
                    .ToList();

                result.AddRange(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return result;
        }

        public async Task<IEnumerable<HRPreparationSalariesLoanDto>> HR_Preparation_Salaries_Loan_SP(HRPreparationSalariesLoanFilterDto filter)
        {
            List<HRPreparationSalariesLoanDto> result = new List<HRPreparationSalariesLoanDto>();

            try
            {
                var query = context.Set<HRPreparationSalariesLoanDto>()
                    .FromSqlRaw("exec HR_Preparation_Salaries_Loan_SP @FinancelYear={0}, @MS_Month={1}, @BRANCH_ID={2}, @Dept_ID={3}, @Location={4}, @Facility_ID={5}, @Fin_year={6}, @CMDTYPE={7}",
                        filter.FinancelYear ?? (object)DBNull.Value,
                        filter.MSMonth ?? (object)DBNull.Value,
                        filter.BranchID ?? 0,
                        filter.DeptID ?? 0,
                        filter.Location ?? (object)DBNull.Value,
                        filter.FacilityID ?? 0,
                        filter.FinYear ?? 0,
                        filter.CMDType ?? 1
                       )
                    .AsEnumerable()
                    .ToList();

                result.AddRange(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return result;
        }


        //public async Task<IEnumerable<EmployeeGosiDto>> HR_RPEmployee_Sp(EmployeeGosiSearchtDto filter)
        //{
        //    List<EmployeeGosiDto> result = new List<EmployeeGosiDto>();

        //    try
        //    {
        //        var query = context.Set<EmployeeGosiDto>()
        //            .FromSqlRaw("exec HR_RPEmployee_Sp @Emp_ID={0}, @Emp_name={1}, @BRANCH_ID={2}, @BRANCHS_ID={3}, @Facility_ID={4}, @Location={5}, @Dept_ID={6}, @Salary_Group_ID={7}, @Month_Code={8}, @FinancelYear={9}, @CMDTYPE={10} ",
        //                filter.EmpCode ?? (object)DBNull.Value,//0
        //                filter.EmpName ?? (object)DBNull.Value,//1
        //                filter.BranchId ?? 0,//2
        //                filter.BranchsId ?? (object)DBNull.Value,//3
        //                filter.FacilityId,//4
        //                filter.Location ?? 0,//5
        //                filter.DeptId ?? 0,//6
        //                filter.SalaryGroupId ?? 0,//7
        //                filter.MonthCode,//8
        //                filter.FinancelYear,//9
        //                filter.CmdType ?? 16 //10
        //                )
        //            .AsEnumerable()
        //            .ToList();
        //        result.AddRange(query);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //        throw;
        //    }

        //    return result;
        //}
        public async Task<IEnumerable<EmployeeGosiDto>> HR_RPEmployee_Sp(EmployeeGosiSearchtDto filter)
        {
            var results = new List<EmployeeGosiDto>();

            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "HR_RPEmployee_Sp";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Emp_ID", filter.EmpCode ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@Emp_name", filter.EmpName ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@BRANCH_ID", filter.BranchId ?? 0));
                command.Parameters.Add(new SqlParameter("@BRANCHS_ID", filter.BranchsId ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@Facility_ID", filter.FacilityId));
                command.Parameters.Add(new SqlParameter("@Location", filter.Location ?? 0));
                command.Parameters.Add(new SqlParameter("@Dept_ID", filter.DeptId ?? 0));
                command.Parameters.Add(new SqlParameter("@Salary_Group_ID", filter.SalaryGroupId ?? 0));
                command.Parameters.Add(new SqlParameter("@Month_Code", filter.MonthCode ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@FinancelYear", filter.FinancelYear));
                command.Parameters.Add(new SqlParameter("@CMDTYPE", filter.CmdType ?? 16));

                await context.Database.OpenConnectionAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var dto = new EmployeeGosiDto
                        {
                            ID_NO = reader["ID_NO"] as string,
                            Gosi_Salary = reader["Gosi_Salary"] != DBNull.Value ? (decimal?)reader["Gosi_Salary"] : null,
                            Gosi_Date = reader["Gosi_Date"] as string,
                            Gosi_No = reader["Gosi_No"] as string,
                            Gois_Subscription_Expiry_Date = reader["Gois_Subscription_Expiry_Date"] as string,
                            Gosi_Rate_Facility = reader["Gosi_Rate_Facility"] != DBNull.Value ? (decimal?)reader["Gosi_Rate_Facility"] : null,
                            Id = reader["id"] != DBNull.Value ? (long?)Convert.ToInt64(reader["id"]) : null,
                            EmployeePrimaryId = reader["ID"] != DBNull.Value ? (long?)Convert.ToInt64(reader["ID"]) : null,
                            Emp_ID = reader["Emp_ID"] as string,
                            Emp_name = reader["Emp_name"] as string,
                            Emp_name2 = reader["Emp_name2"] as string,
                            Location_Name = reader["Location_Name"] as string,
                            Location_Name2 = reader["Location_Name2"] as string,
                            CostCenter_Code = reader["CostCenter_Code"] as string,
                            CostCenter_Name = reader["CostCenter_Name"] as string,
                            CC_ID = reader["CC_ID"] != DBNull.Value ? (long?)Convert.ToInt64(reader["CC_ID"]) : null,
                            Gosi_Bisc_Salary = reader["Gosi_Bisc_Salary"] != DBNull.Value ? (decimal?)reader["Gosi_Bisc_Salary"] : null,
                            Gosi_House_Allowance = reader["Gosi_House_Allowance"] != DBNull.Value ? (decimal?)reader["Gosi_House_Allowance"] : null,
                            Gosi_Allowance_Commission = reader["Gosi_Allowance_Commission"] != DBNull.Value ? (decimal?)reader["Gosi_Allowance_Commission"] : null,
                            Gosi_Other_Allowances = reader["Gosi_Other_Allowances"] != DBNull.Value ? (decimal?)reader["Gosi_Other_Allowances"] : null,
                            Gosi_Name = reader["Gosi_Name"] as string,
                            Gosi_Name2 = reader["Gosi_Name2"] as string,
                            Gosi_Salary_Facility = reader["Gosi_Salary_Facility"] != DBNull.Value ? (decimal?)reader["Gosi_Salary_Facility"] : null,
                            Gosi_Salary_Emp = reader["Gosi_Salary_Emp"] != DBNull.Value ? (decimal?)reader["Gosi_Salary_Emp"] : null
                        };

                        results.Add(dto);
                    }
                }
            }

            return results;
        }




        public async Task<HrEmployeeLeaveResultDto> GetEmployeeLeaveData(HrLeaveGetDataDto dto)
        {
            HrEmployeeLeaveResultDto result = new HrEmployeeLeaveResultDto();

            try
            {
                var r = await context.Set<HrEmployeeLeaveResultDto>()
                    .FromSqlRaw("exec HR_Emp_Leave_Sp @Emp_ID={0}, @Leave_Date={1}, @Facility_ID={2}, @Leave_Type={3}, @Leave_Type_Sub={4}",
                        dto.EmpId,
                        dto.LastworkingDay,
                        currentData.FacilityId,
                        dto.DDLLeaveType,
                        dto.DDLLeaveType2
                        )
                    .ToListAsync();

                if (r.Count > 0)
                {
                    result = r[0];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return result;
        }



        public async Task<IEnumerable<HRAttendanceTotalReportDto>> HR_Attendance_TotalReportManagerial_SP(HRAttendanceTotalReportFilterDto filter)
        {
            List<HRAttendanceTotalReportDto> result = new List<HRAttendanceTotalReportDto>();

            try
            {
                var query = context.Set<HRAttendanceTotalReportDto>()
                    .FromSqlRaw("exec HR_Attendance_TotalReportManagerial_SP @BRANCH_ID={0}, @BRANCHS_ID={1}, @Location={2}, @Facility_ID={3}, @Emp_ID={4}, @Emp_Name={5}, @Start_Date ={6}, @End_Date ={7}, @Nationality_Type ={8}, @Group_Shift_ID ={9}",
                 filter.BranchId ?? 0, // 0
                 currentData.Branches, // 1
                 filter.Location, //2
                 currentData.FacilityId, // 3
                 filter.empCode, // 4
                 filter.EmpName, // 5
                 filter.From, // 6
                 filter.To, // 7
                  0, // 8
                 0 // 9

                ).AsEnumerable().ToList();

                result.AddRange(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return result;
        }
        public async Task<IEnumerable<HrEmployeeIncremenResultDto>> HR_Increments_During_Time_SP(IncrementsBothFilterDto filter)
        {
            List<HrEmployeeIncremenResultDto> result = new List<HrEmployeeIncremenResultDto>();

            try
            {
                var query = context.Set<HrEmployeeIncremenResultDto>()
                    .FromSqlRaw("exec HR_Increments_During_Time_SP @ToDate={0}, @Branch_ID={1}, @BRANCHS_ID={2}, @Dept_ID={3}, @AnnaulIncreaseMethod={4}, @Location_ID={5}, @Nationality_ID ={6}, @Emp_Code ={7}, @Emp_Name ={8}, @CreatedBy ={9}, @Facility_ID ={10},@EvaluationFromDate ={11}, @EvaluationToDate ={12}, @CMDTYPE ={13}",
                 filter.ToDate ?? "", // 0
                 currentData.BranchId, // 1
                 filter.Branches, //2
                 filter.DeptId, // 3
                 filter.AnnaulIncreaseMethod, // 4
                 filter.Location, // 5
                 filter.Nationality, // 6
                 filter.EmpCode, // 7
                 filter.EmpName, // 8
                 currentData.UserId, // 9
                 currentData.FacilityId, // 10
                 filter.EvaluationFromDate,// 11
                 filter.EvaluationToDate,// 12
                 filter.CMDTYPE// 13

                ).AsEnumerable().ToList();

                result.AddRange(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return result;

        }

        public async Task<IEnumerable<HRAttendanceTotalReportNewSPDto>> HR_Attendance_TotalReportNew_SP(HRAttendanceTotalReportFilterDto filter)
        {
            List<HRAttendanceTotalReportNewSPDto> result = new List<HRAttendanceTotalReportNewSPDto>();

            try
            {
                var query = context.Set<HRAttendanceTotalReportNewSPDto>()
                    .FromSqlRaw("exec HR_Attendance_TotalReportNew_SP @Branch_ID={0}, @BRANCHS_ID={1}, @Dept_ID={2}, @Location={3}, @Facility_ID={4}, @Emp_ID ={5}, @Emp_Name ={6}, @Satrt_Date ={7}, @End_Date ={8},@Nationality_Type ={9}, @Group_Shift_ID ={10}, @CMDTYPE ={11}",
                 filter.BranchId, // 0
                 currentData.Branches, //1
                 0, // 2
                 filter.Location, // 3
                 currentData.FacilityId, //4
                 filter.empCode, // 5
                 filter.EmpName, // 6
                 filter.From, // 7
                 filter.To, // 8
                 0, // 9
                 0,// 10
                 1// 11

                ).AsEnumerable().ToList();

                result.AddRange(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return result;
        }

        public async Task<IEnumerable<HRAttendanceReport6SP>> HR_Attendance_Report6_SP(HRAttendanceReport6FilterSP filter)
        {
            List<HRAttendanceReport6SP> result = new List<HRAttendanceReport6SP>();

            try
            {
                var query = context.Set<HRAttendanceReport6SP>()
                    .FromSqlRaw("exec HR_Attendance_Report6_SP @Emp_Code={0}, @Emp_name={1}, @BRANCH_ID={2}, @Branchs_ID={3}, @Start_Date={4}, @End_Date ={5}, @Location ={6}, @Dept_ID ={7}, @Sponsors_ID ={8},@Status_ID ={9}, @Attendance_Type ={10}, @Calendar_Type ={11}, @Language ={12}, @Type_Working_hours ={13}, @Facility_ID ={14}, @HR_Att_Shift_ID ={15}",
                    filter.EmpCode,//0
                    filter.EmpName,//1
                    filter.BranchId,//2
                    filter.BranchsId,//3
                    filter.From,//4
                    filter.To,//5
                    filter.Location,//6
                    filter.DeptId,//7
                    filter.Sponsors,//8
                    filter.Status,//9
                    filter.AttendanceType,//10
                    currentData.CalendarType,//11
                    currentData.Language,//12
                    filter.Workinghours,//13
                    filter.Facility,//14
                    filter.ShitId//14


                ).AsEnumerable().ToList();

                result.AddRange(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return result;
        }


        public async Task<HREmpClearanceSpDto> HR_Emp_Clearance_Sp(long EmpId, string LastWorkingDate)
        {
            HREmpClearanceSpDto result = new HREmpClearanceSpDto();

            try
            {
                var r = context.Set<HREmpClearanceSpDto>()
                    .FromSqlRaw("exec HR_Emp_Clearance_Sp @Emp_ID={0}, @Clearnace_Date={1}, @Facility_ID={2}",
                        EmpId,
                        LastWorkingDate,
                        currentData.FacilityId)
                    .AsEnumerable()
                    .ToList();
                if (r.Count > 0)
                {
                    result = r[0];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return result;
        }

        public async Task<int> HR_Reaset_Attendance_SP(HrAttendanceResetDto entity)
        {
            try
            {
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "[HR_Reaset_Attendance_SP]";

                    objCmd.Parameters.AddWithValue("@To_Date", entity.To);
                    objCmd.Parameters.AddWithValue("@From_Date", entity.From);
                    objCmd.Parameters.AddWithValue("@BRANCH_ID", entity.Branch);
                    objCmd.Parameters.AddWithValue("@Emp_Id", entity.EmpId);
                    objCmd.Parameters.AddWithValue("@CMDTYPE", 1);

                    await objCnn.OpenAsync();
                    var obj = await objCmd.ExecuteScalarAsync();

                    // Safely convert the result to an integer, defaulting to 0 if null
                    return obj != null && int.TryParse(obj.ToString(), out var result) ? result : 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<HRPayrollAdvancedResultDto>> HR_Payroll_Create_Advanced_Sp(HRPayrollCreateSpFilterDto filter)
        {
            List<HRPayrollCreateAdvancedSpDto> result = new List<HRPayrollCreateAdvancedSpDto>();
            List<HRPayrollAdvancedResultDto> Finalresult = new List<HRPayrollAdvancedResultDto>();

            try
            {
                // Fetch payroll data using the stored procedure
                var query = context.Set<HRPayrollCreateAdvancedSpDto>()
                    .FromSqlRaw("exec HR_Payroll_Create_Advanced_Sp @MS_Month={0}, @FinancelYear={1}, @BRANCH_ID={2}, @Dept_ID={3}, @Location={4}, @Facility_ID={5}, @Emp_ID={6}, @Emp_Name={7}, @BRANCHS_ID={8}, @Salary_Group_ID={9}, @Contract_Type_ID={10}, @Payment_Type_ID={11}, @Sponsors_ID={12}, @Wages_Protection={13}",
                        filter.MSMonth ?? (object)DBNull.Value,
                        filter.FinancelYear ?? (object)DBNull.Value,
                        filter.BRANCHID ?? 0,
                        filter.DeptID ?? 0,
                        filter.Location ?? 0,
                        filter.FacilityID ?? 0,
                        filter.EmpCode ?? (object)DBNull.Value,
                        filter.EmpName ?? "",
                        filter.Branches ?? (object)DBNull.Value,
                        filter.SalaryGroupID ?? (object)DBNull.Value,
                        filter.ContractTypeID ?? (object)DBNull.Value,
                        filter.PaymentTypeID ?? (object)DBNull.Value,
                        filter.SponsorsID ?? (object)DBNull.Value,
                        filter.WagesProtection ?? (object)DBNull.Value
                    )
                    .AsEnumerable()
                    .ToList();

                result.AddRange(query);

                // Set up dates for fetching tax slides
                // Set up dates as strings for fetching tax slides
                var fromDate = $"{filter.FinancelYear}-{filter.MSMonth}-01";
                var toDate = $"{filter.FinancelYear}-{filter.MSMonth}-{System.DateTime.DaysInMonth(Convert.ToInt32(filter.FinancelYear), Convert.ToInt32(filter.MSMonth))}";


                // Fetch all tax slides within the date range
                var dtAllTaxSlides = await GetIncomeTaxSlidesFromContextAsync(fromDate, toDate, null);

                // Iterate over payroll results to calculate income tax
                foreach (var singleItem in result)
                {
                    decimal? incomeTax = 0;
                    decimal? startSlide = 0;
                    decimal? monthlyNetSalary = 0;
                    /////////////////////////////////////////////////////////////////
                    if (singleItem.TaxId.HasValue && singleItem.TaxId > 0)
                    {
                        // Filter tax slides based on IncomeTaxId
                        var taxSlides = dtAllTaxSlides.Where(ts => ts.IncomeTaxId == singleItem.TaxId).ToList();

                        if (taxSlides.Any())
                        {
                            // Get personal exemption value
                            decimal? personalExemptionValue = Convert.ToDecimal(taxSlides.First().PersonalExemption) / 12;

                            if (singleItem.UseIncomeTaxCalc == true)
                            {
                                // Calculate monthly net salary
                                monthlyNetSalary = singleItem.Salary - personalExemptionValue +
                                    singleItem.Allowance + singleItem.OverTime + singleItem.Commission -
                                    singleItem.Absence - singleItem.DelayHourByDay - singleItem.Deduction - singleItem.Penalties;

                                decimal? cumulativeSlideSalary = 0;

                                // Calculate cumulative slide salary
                                foreach (var slide in taxSlides)
                                {
                                    cumulativeSlideSalary += slide.TaxSlideValue;
                                    startSlide = Convert.ToDecimal(slide.TaxSlideStartingFromTheSlideNo);

                                    if (cumulativeSlideSalary > monthlyNetSalary)
                                        continue;
                                }

                                // Iterate through tax slides to calculate income tax
                                foreach (var slide in taxSlides)
                                {
                                    if (slide.TaxSlideOrderNo != 0 && startSlide <= slide.TaxSlideOrderNo)
                                    {
                                        var slideBase = monthlyNetSalary > slide.TaxSlideValue
                                            ? slide.TaxSlideValue
                                            : monthlyNetSalary;

                                        incomeTax += slideBase * (slide.TaxSlideRate / 100);
                                        monthlyNetSalary -= slideBase;


                                    }
                                }

                            }
                        }
                    }
                    var singlePayroll = new HRPayrollAdvancedResultDto
                    {
                        Emp_ID = singleItem.Emp_ID,
                        Emp_name = singleItem.Emp_name,
                        Salary = singleItem.Salary,
                        IncomeTax = incomeTax,
                        UseIncomeTaxCalc = singleItem.UseIncomeTaxCalc,
                        Facility_Name = singleItem.Facility_Name,
                        Location_Name2 = singleItem.Location_Name2,
                        Location_Name = singleItem.Location_Name,
                        Loan = singleItem.Loan,
                        Absence = singleItem.Absence,
                        Account_No = singleItem.Account_No,
                        AdvanceDeduction = singleItem.AdvanceDeduction,
                        Allowance = singleItem.Allowance,
                        Attendance = singleItem.Attendance,
                        Att_End_date = singleItem.Att_End_date,
                        Att_Start_date = singleItem.Att_Start_date,
                        Bank_Name = singleItem.Bank_Name,
                        BasicSalary = singleItem.BasicSalary,
                        H_OverTime = singleItem.H_OverTime,
                        Bank_ID = singleItem.Bank_ID,
                        BRA_NAME = singleItem.BRA_NAME,
                        BRA_NAME2 = singleItem.BRA_NAME2,
                        Cat_name = singleItem.Cat_name,
                        Cat_name2 = singleItem.Cat_name2,
                        Cnt_Absence = singleItem.Cnt_Absence,
                        Cnt_Delay = singleItem.Cnt_Delay,
                        Commission = singleItem.Commission,
                        Deduction = singleItem.Deduction,
                        Delay = singleItem.Delay,
                        DelayHourByDay = singleItem.DelayHourByDay,
                        Dep_Name = singleItem.Dep_Name,
                        Dep_Name2 = singleItem.Dep_Name2,
                        DOAppointment = singleItem.DOAppointment,
                        Emp_name2 = singleItem.Emp_name2,
                        Facility_Name2 = singleItem.Facility_Name2,
                        ID_No = singleItem.ID_No,
                        Mandate = singleItem.Mandate,
                        OverTime = singleItem.OverTime,
                        Penalties = singleItem.Penalties,
                        SocialInsurance = singleItem.SocialInsurance,
                        TaxCode = singleItem.TaxCode,
                        TaxDeduction = singleItem.TaxDeduction,
                        ID = singleItem.ID,
                        TaxId = singleItem.TaxId,
                        Total = (singleItem.Salary ?? 0) + (singleItem.Allowance ?? 0) + (singleItem.OverTime ?? 0) + (singleItem.Commission ?? 0),
                        NetAfterTax = (singleItem.Salary ?? 0) + (singleItem.Allowance ?? 0) + (singleItem.OverTime ?? 0) + (singleItem.Commission ?? 0) - (singleItem.Absence ?? 0) - (singleItem.DelayHourByDay ?? 0) - (singleItem.Loan ?? 0) - (singleItem.Deduction ?? 0) - (singleItem.SocialInsurance ?? 0) - (singleItem.Penalties ?? 0) - incomeTax,
                        NetBeforTax = (singleItem.Salary ?? 0) + (singleItem.Allowance ?? 0) + (singleItem.OverTime ?? 0) + (singleItem.Commission ?? 0) - (singleItem.Absence ?? 0) - (singleItem.DelayHourByDay ?? 0) - (singleItem.Loan ?? 0) - (singleItem.Deduction ?? 0) - (singleItem.SocialInsurance ?? 0) - (singleItem.Penalties ?? 0),
                        TotalDeductions = (singleItem.Absence ?? 0) - (singleItem.DelayHourByDay ?? 0) - (singleItem.Loan ?? 0) - (singleItem.Deduction ?? 0) + (singleItem.SocialInsurance ?? 0) + (singleItem.Penalties ?? 0)

                    };
                    Finalresult.Add(singlePayroll);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }

            return Finalresult;
        }
        private async Task<List<TaxSlideDto>> GetIncomeTaxSlidesFromContextAsync(string fromDate, string toDate, int? incomeTaxId)
        {
            // Fetch all data without date filters from the database
            var data = await (
                from itSlide in context.HrIncomeTaxSlides
                join itPeriod in context.HrIncomeTaxPeriods
                    on itSlide.IncomeTaxPeriodId equals itPeriod.Id
                join it in context.HrIncomeTaxs
                    on itPeriod.IncomeTaxId equals it.Id into itGroup
                from it in itGroup.DefaultIfEmpty() // Simulate RIGHT JOIN
                where itSlide.IsDeleted == false
                      && itPeriod.IsDeleted == false
                      && (incomeTaxId == null || it.Id == incomeTaxId)
                orderby itSlide.TaxSlideOrderNo
                select new
                {
                    Id = itSlide.Id,
                    TaxSlideOrderNo = itSlide.TaxSlideOrderNo,
                    TaxSlideValue = itSlide.TaxSlideValue / 12, // Divide by 12 as per your logic
                    TaxSlideRate = itSlide.TaxSlideRate,
                    TaxSlideStartingFromTheSlideNo = itSlide.TaxSlideStartingFromTheSlideNo,
                    TaxSlideNote = itSlide.TaxSlideNote,
                    FromDate = itPeriod.FromDate,
                    ToDate = itPeriod.ToDate,
                    PersonalExemption = itPeriod.PersonalExemption,
                    IncomeTaxId = it != null ? it.Id : (int?)null,  // Handle null case for RIGHT JOIN
                    TaxCode = it != null ? it.TaxCode : null,
                    TaxName = it != null ? it.TaxName : null,
                    TaxName2 = it != null ? it.TaxName2 : null
                }).ToListAsync();

            // Perform date filtering in memory
            var filteredData = data.Where(item =>
                DateHelper.StringToDate(item.FromDate) <= DateHelper.StringToDate(fromDate) && DateHelper.StringToDate(item.ToDate) >= DateHelper.StringToDate(toDate)).ToList();

            // Map to DTO after filtering
            var result = filteredData.Select(item => new TaxSlideDto
            {
                Id = item.Id,
                TaxSlideOrderNo = item.TaxSlideOrderNo,
                TaxSlideValue = item.TaxSlideValue,
                TaxSlideRate = item.TaxSlideRate,
                TaxSlideStartingFromTheSlideNo = item.TaxSlideStartingFromTheSlideNo,
                TaxSlideNote = item.TaxSlideNote,
                FromDate = item.FromDate,
                ToDate = item.ToDate,
                PersonalExemption = item.PersonalExemption,
                IncomeTaxId = item.IncomeTaxId,
                TaxCode = item.TaxCode,
                TaxName = item.TaxName,
                TaxName2 = item.TaxName2
            }).ToList();

            return result;
        }

        public async Task<DataTable> HR_Payroll_Clearance_Sp(string EmpCode, string EmpName)
        {
            try
            {
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "HR_Payroll_Clearance_Sp";

                    objCmd.Parameters.AddWithValue("@Emp_ID", EmpCode);
                    objCmd.Parameters.AddWithValue("@Emp_Name", null);

                    objCmd.CommandTimeout = 666666;

                    await objCnn.OpenAsync();
                    var result = objCmd.ExecuteReader();
                    DataTable dt = new();
                    dt.Load(result);

                    return dt;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Stored Procedure in main system
        public async Task<string> GetDecryptUserPassword(long id)
        {
            using (var connection = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
            {
                await connection.OpenAsync();

                string sql = "SELECT dbo.Sys_DECRYPT(USER_PASSWORD) FROM [SYS_USER_VW] WHERE USER_ID=@UserId";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserId", id);
                    string decryptedPassword = (string)command.ExecuteScalar();

                    return decryptedPassword;
                }
            }
        }

        public async Task<string> AddUserByProcedure(SysUserDto entity)
        {
            try
            {
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "[SYS_USER_SP]";

                    objCmd.Parameters.AddWithValue("@USER_FULLNAME", entity.UserFullname);
                    objCmd.Parameters.AddWithValue("@USER_EMAIL", entity.Email);
                    objCmd.Parameters.AddWithValue("@USER_NAME", entity.UserName);
                    objCmd.Parameters.AddWithValue("@USER_PASSWORD", entity.StringUserPassword);

                    objCmd.Parameters.AddWithValue("@USER_TYPE_ID", entity.UserTypeId);
                    objCmd.Parameters.AddWithValue("@USER_PK_ID", entity.UserPkId);
                    objCmd.Parameters.AddWithValue("@Emp_ID", entity.EmpId);
                    objCmd.Parameters.AddWithValue("@Groups_ID", entity.GroupsId);
                    objCmd.Parameters.AddWithValue("@Facility_ID", entity.FacilityId);
                    objCmd.Parameters.AddWithValue("@Sales_Type", entity.SalesType);
                    objCmd.Parameters.AddWithValue("@Cus_ID", entity.CusId);
                    objCmd.Parameters.AddWithValue("@Sup_ID", entity.SupId);
                    objCmd.Parameters.AddWithValue("@CreatedBy", entity.CreatedBy);
                    objCmd.Parameters.AddWithValue("@Projects_ID", entity.ProjectsId);
                    objCmd.Parameters.AddWithValue("@Cand_ID", entity.CandId);
                    objCmd.Parameters.AddWithValue("@Isupdate", entity.Isupdate);
                    objCmd.Parameters.AddWithValue("@USER_TYPE2_ID", entity.UserType2Id);
                    objCmd.Parameters.AddWithValue("@CMDTYPE", 1);
                    objCmd.CommandTimeout = 600;
                    await objCnn.OpenAsync();
                    var obj = await objCmd.ExecuteScalarAsync();

                    return obj?.ToString() ?? "";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> EditUserByProcedure(SysUserEditDto entity)
        {
            try
            {
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "[SYS_USER_SP]";

                    objCmd.Parameters.AddWithValue("@USER_NAME", entity.UserName);
                    objCmd.Parameters.AddWithValue("@USER_PASSWORD", entity.StringUserPassword);
                    objCmd.Parameters.AddWithValue("@USER_FULLNAME", entity.UserFullname);
                    objCmd.Parameters.AddWithValue("@USER_EMAIL", entity.Email);

                    objCmd.Parameters.AddWithValue("@USER_PK_ID", entity.UserPkId);
                    objCmd.Parameters.AddWithValue("@USER_TYPE_ID", entity.UserTypeId);
                    objCmd.Parameters.AddWithValue("@USER_TYPE2_ID", entity.UserType2Id);
                    objCmd.Parameters.AddWithValue("@USER_ID", entity.Id);
                    objCmd.Parameters.AddWithValue("@BRANCHS_ID", entity.BranchsId);
                    objCmd.Parameters.AddWithValue("@Groups_ID", entity.GroupsId);
                    objCmd.Parameters.AddWithValue("@Dashboard_Widget", entity.DashboardWidget);
                    objCmd.Parameters.AddWithValue("@Emp_ID", entity.EmpId);
                    objCmd.Parameters.AddWithValue("@Enable", entity.Enable);
                    objCmd.Parameters.AddWithValue("@IPS", entity.Ips);
                    objCmd.Parameters.AddWithValue("@Time_From", entity.TimeFrom);
                    objCmd.Parameters.AddWithValue("@Time_To", entity.TimeTo);
                    objCmd.Parameters.AddWithValue("@Facility_ID", entity.FacilityId);
                    objCmd.Parameters.AddWithValue("@Sales_Type", entity.SalesType);
                    objCmd.Parameters.AddWithValue("@ModifiedBy", entity.ModifiedBy);
                    objCmd.Parameters.AddWithValue("@Projects_ID", entity.ProjectsId);
                    objCmd.Parameters.AddWithValue("@PermissionsOverUserId", entity.PermissionsOverUserId);
                    objCmd.Parameters.AddWithValue("@PermissionsOverCustomerGroupsId", entity.PermissionsOverCustomerGroupsId);
                    objCmd.Parameters.AddWithValue("@CMDTYPE", 2);
                    objCmd.CommandTimeout = 600;
                    await objCnn.OpenAsync();
                    var obj = await objCmd.ExecuteScalarAsync();

                    return obj?.ToString() ?? "";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<byte[]> EncryptUserPassword(string PassWord)
        {
            using (var connection = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
            {
                await connection.OpenAsync();

                string sql = "SELECT dbo.Sys_ENCRYPT(@USER_PASSWORD)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@USER_PASSWORD", PassWord);

                    // Return the byte array directly
                    return (byte[])command.ExecuteScalar();
                }
            }
        }






        #endregion

        public async Task<DataTable> GetSalaryData_WF(long EmpId, long FacilityId, string CurrentDate)
        {
            try
            {
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "HR_Self_Service_Dashboard";

                    objCmd.Parameters.AddWithValue("@USER_ID", 0);
                    objCmd.Parameters.AddWithValue("@Emp_ID", EmpId);
                    objCmd.Parameters.AddWithValue("@Dept_ID", 0);
                    objCmd.Parameters.AddWithValue("@Location_ID", 0);
                    objCmd.Parameters.AddWithValue("@Branch_ID", 0);
                    objCmd.Parameters.AddWithValue("@BRANCHS_ID", "");
                    objCmd.Parameters.AddWithValue("@Curr_Date", CurrentDate);
                    objCmd.Parameters.AddWithValue("@Curr_Date_H", "");
                    objCmd.Parameters.AddWithValue("@Facility_ID", FacilityId);

                    objCmd.Parameters.AddWithValue("@CMDTYPE", 12);
                    objCmd.CommandTimeout = 600;

                    await objCnn.OpenAsync();
                    var result = objCmd.ExecuteReader();
                    DataTable dt = new();
                    dt.Load(result);

                    return dt;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DataTable> GetDues_WF(long EmpId, string CurrentDate)
        {
            try
            {
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "HR_Self_Service_Dashboard";

                    objCmd.Parameters.AddWithValue("@USER_ID", 0);
                    objCmd.Parameters.AddWithValue("@Emp_ID", EmpId);
                    objCmd.Parameters.AddWithValue("@Dept_ID", 0);
                    objCmd.Parameters.AddWithValue("@Location_ID", 0);
                    objCmd.Parameters.AddWithValue("@Branch_ID", 0);
                    objCmd.Parameters.AddWithValue("@BRANCHS_ID", "");
                    objCmd.Parameters.AddWithValue("@Curr_Date", CurrentDate);
                    objCmd.Parameters.AddWithValue("@Curr_Date_H", "");
                    objCmd.Parameters.AddWithValue("@Facility_ID", 0);

                    objCmd.Parameters.AddWithValue("@CMDTYPE", 13);
                    objCmd.CommandTimeout = 600;

                    await objCnn.OpenAsync();
                    var result = objCmd.ExecuteReader();
                    DataTable dt = new();
                    dt.Load(result);

                    return dt;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region======================================= Stored Procedure in Acc system
        public async Task<IEnumerable<TrialBalanceSheetDtoResult>> GetTrialBalanceSheet(TrialBalanceSheetDto obj)
        {
            try
            {
                // تحقق من المدخلات الأساسية
                if (string.IsNullOrEmpty(obj.periodStartDate) || string.IsNullOrEmpty(obj.periodEndDate))
                    throw new ArgumentException("Period start date and end date cannot be null or empty.");

                var resultList = new List<TrialBalanceSheetDtoResult>();

                using (var objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                {
                    await objCnn.OpenAsync();

                    using (var objCmd = new SqlCommand("Acc_BalanceSheet2_sp", objCnn) { CommandType = CommandType.StoredProcedure, CommandTimeout = 600 })
                    {
                        // إعداد المعاملات للإجراء المخزن
                        objCmd.Parameters.AddWithValue("@Period_Start_Date", obj.periodStartDate ?? (object)DBNull.Value);
                        objCmd.Parameters.AddWithValue("@Period_End_Date", obj.periodEndDate ?? (object)DBNull.Value);
                        objCmd.Parameters.AddWithValue("@Facility_ID", obj.facilityId);
                        objCmd.Parameters.AddWithValue("@Branch_ID", obj.branchId);
                        objCmd.Parameters.AddWithValue("@Fin_year", obj.finYear);
                        objCmd.Parameters.AddWithValue("@Account_level", obj.accountLevel);
                        objCmd.Parameters.AddWithValue("@NoZero", obj.noZero);
                        objCmd.Parameters.AddWithValue("@Account_From", obj.accountFrom ?? (object)DBNull.Value);
                        objCmd.Parameters.AddWithValue("@Account_To", obj.accountTo ?? (object)DBNull.Value);
                        objCmd.Parameters.AddWithValue("@CC_From", obj.ccFrom ?? (object)DBNull.Value);
                        objCmd.Parameters.AddWithValue("@CC_To", obj.ccTo ?? (object)DBNull.Value);
                        objCmd.Parameters.AddWithValue("@ShowAllLevel", obj.showAllLevel);

                        using (var reader = await objCmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                // استخراج القيم بشكل نظيف
                                decimal? amountPrev = reader["AMOUNTPrev"] != DBNull.Value ? Convert.ToDecimal(reader["AMOUNTPrev"]) : null;
                                decimal? amountNext = reader["AMOUNTNext"] != DBNull.Value ? Convert.ToDecimal(reader["AMOUNTNext"]) : null;

                                resultList.Add(new TrialBalanceSheetDtoResult
                                {
                                    IsSub = reader["Is_Sub"] as bool?,
                                    AccAccountId = reader["Acc_Account_ID"] != DBNull.Value ? Convert.ToInt64(reader["Acc_Account_ID"]) : (long?)null,
                                    AccAccountName = reader["Acc_Account_Name"] as string,
                                    AccAccountName2 = reader["Acc_Account_Name2"] as string,
                                    AccAccountCode = reader["Acc_Account_Code"] as string,
                                    AccountLevel = reader["Account_level"] != DBNull.Value ? Convert.ToInt32(reader["Account_level"]) : (int?)null,
                                    NatureAccount = reader["Nature_account"] != DBNull.Value ? Convert.ToInt32(reader["Nature_account"]) : (int?)null,
                                    AccGroupId = reader["Acc_group_ID"] != DBNull.Value ? Convert.ToInt64(reader["Acc_group_ID"]) : (long?)null,

                                    AMOUNTPrev = amountPrev.HasValue && amountPrev > 0 ? amountPrev * -1 : 0,
                                    PrevDebit = amountPrev.HasValue && amountPrev < 0 ? amountPrev * -1 : 0,

                                    Debit = reader["Debit"] != DBNull.Value ? Convert.ToDecimal(reader["Debit"]) : (decimal?)0,
                                    Credit = reader["Credit"] != DBNull.Value ? Convert.ToDecimal(reader["Credit"]) : (decimal?)0,

                                    AMOUNTNext = amountNext.HasValue && amountNext > 0 ? amountNext : 0,
                                    NextDebit = amountNext.HasValue && amountNext < 0 ? amountNext * -1 : 0,
                                });
                            }
                        }
                    }
                }

                return resultList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in GetTrialBalanceSheet: {ex.Message}", ex);
            }
        }



        #region ==========================================   قائمة الدخل شهري
        public async Task<IEnumerable<IncomeStatementMonthResultDto>> GetIncomeStatementMonth(IncomeStatementMonthtDto obj)
        {
            try
            {



                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "ACC_Income_Statement_Month_Sp";

                    // إعداد المعاملات للإجراء المخزن
                    objCmd.Parameters.AddWithValue("@Fin_year_Gregorian", obj.FinyearGregorian);
                    objCmd.Parameters.AddWithValue("@Facility_ID", obj.facilityId);
                    objCmd.Parameters.AddWithValue("@Fin_year", obj.finYear);
                    objCmd.Parameters.AddWithValue("@CC_ID", obj.ccId);

                    objCmd.CommandTimeout = 600;

                    // فتح الاتصال وتنفيذ الإجراء المخزن
                    await objCnn.OpenAsync();
                    var result = await objCmd.ExecuteReaderAsync();

                    // تحويل النتيجة إلى قائمة من النوع IncomeStatementResultDto
                    var resultList = new List<IncomeStatementMonthResultDto>();
                    while (await result.ReadAsync())
                    {
                        var item = new IncomeStatementMonthResultDto
                        {
                            AccountName = result["الحساب"] as string,
                            MonthlyValues = new Dictionary<string, decimal>()
                        };

                        // استخراج قيم الأشهر
                        for (int i = 1; i <= 12; i++)
                        {
                            var monthKey = i.ToString("D2");
                            if (result[monthKey] != DBNull.Value)
                                item.MonthlyValues[monthKey] = Convert.ToDecimal(result[monthKey]);
                        }

                        resultList.Add(item);
                    }

                    return resultList;
                }
            }
            catch (Exception ex)
            {
                // معالجة الأخطاء وإظهار تفاصيل الخطأ
                throw new Exception($"An error occurred while fetching income statement: {ex.Message}", ex);
            }
        }

        #endregion ==================================  قائمة الدخل شهري

        #region ==========================================  الأرباح والخسائر
        public async Task<IEnumerable<ProfitandLossResultDto>> GetProfitandLoss(ProfitandLossDto obj)
        {
            try
            {
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "ACC_Income_Statement3_Sp";

                    objCmd.Parameters.AddWithValue("@J_Date_From", obj.dateFrom);
                    objCmd.Parameters.AddWithValue("@J_Date_To", obj.dateTo);
                    objCmd.Parameters.AddWithValue("@Facility_ID", currentData.FacilityId);
                    objCmd.Parameters.AddWithValue("@Fin_year", currentData.FinYear);
                    objCmd.Parameters.AddWithValue("@CC_ID", obj.ccId);
                    objCmd.Parameters.AddWithValue("@Account_level", obj.AccountLevel);
                    objCmd.CommandTimeout = 600;

                    await objCnn.OpenAsync();
                    var reader = await objCmd.ExecuteReaderAsync();

                    var resultList = new List<ProfitandLossResultDto>();

                    while (await reader.ReadAsync())
                    {
                        var item = new ProfitandLossResultDto
                        {
                            AccAccountName = reader["Acc_Account_Name"]?.ToString(),
                            AccAccountName2 = reader["Acc_Account_Name"]?.ToString(),
                            AccAccountCode = reader["Acc_Account_Code"]?.ToString(),
                            Debit = reader["Debit"] as decimal?,
                            Credit = reader["Credit"] as decimal?,
                        };



                        resultList.Add(item);
                    }

                    return resultList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Get Profit and Loss: {ex.Message}", ex);
            }
        }

        #endregion ================================== الأرباح والخسائر


        #region ==========================================  قائمة التدفقات النقدية
        public async Task<IEnumerable<CashFlowsResultDto>> GetCashFlows(CashFlowsDto obj)
        {
            try
            {
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "ACC_Cash_Flows_Sp";

                    objCmd.Parameters.AddWithValue("@J_Date_From", obj.dateFrom);
                    objCmd.Parameters.AddWithValue("@J_Date_To", obj.dateTo);
                    objCmd.Parameters.AddWithValue("@Facility_ID", currentData.FacilityId);
                    objCmd.Parameters.AddWithValue("@Fin_year", currentData.FinYear);
                    objCmd.CommandTimeout = 600;

                    await objCnn.OpenAsync();
                    var reader = await objCmd.ExecuteReaderAsync();

                    var resultList = new List<CashFlowsResultDto>();

                    while (await reader.ReadAsync())
                    {
                        var item = new CashFlowsResultDto
                        {
                            Description = reader["Description"]?.ToString(),
                            Amount = reader["Amount"] as decimal?,
                        };



                        resultList.Add(item);
                    }

                    return resultList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Get Cash Flows: {ex.Message}", ex);
            }
        }

        #endregion ==================================قائمة التدفقات النقدية


        #region ==========================================   اعمار الديون
        public async Task<IEnumerable<AgedReceivablesResultDto>> GetAgedReceivables(AgedReceivablesDto obj)
        {
            try
            {
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "Acc_Debt_Age_Sp";

                    objCmd.Parameters.AddWithValue("@BRANCH_ID", obj.BRANCHID);
                    objCmd.Parameters.AddWithValue("@Facility_ID", obj.facilityId);
                    objCmd.Parameters.AddWithValue("@Cur_Date", obj.CurDate);
                    objCmd.Parameters.AddWithValue("@Emp_ID", obj.EmpID);
                    objCmd.Parameters.AddWithValue("@Group_ID", obj.GroupID);
                    objCmd.Parameters.AddWithValue("@CMDType", 1);

                    if (!string.IsNullOrEmpty(obj.CustomerCode))
                        objCmd.Parameters.AddWithValue("@CustomerCode", obj.CustomerCode);
                    if (!string.IsNullOrEmpty(obj.CustomerCode2))
                        objCmd.Parameters.AddWithValue("@CustomerCode2", obj.CustomerCode2);

                    await objCnn.OpenAsync();
                    var reader = await objCmd.ExecuteReaderAsync();

                    var resultList = new List<AgedReceivablesResultDto>();

                    while (await reader.ReadAsync())
                    {
                        decimal balance = reader.GetDecimal(reader.GetOrdinal("Balance"));
                        if (balance > 0)
                            continue;

                        decimal balanceDue = reader.GetDecimal(reader.GetOrdinal("BalanceDue"));
                        decimal notDue = reader.GetDecimal(reader.GetOrdinal("BlanceNotDue"));
                        decimal payment = reader.GetDecimal(reader.GetOrdinal("Payment"));
                        decimal a1_90 = reader.GetDecimal(reader.GetOrdinal("Amount_1_90"));
                        decimal a91_180 = reader.GetDecimal(reader.GetOrdinal("Amount_91_180"));
                        decimal a181_270 = reader.GetDecimal(reader.GetOrdinal("Amount_181_270"));
                        decimal aMoreThan270 = reader.GetDecimal(reader.GetOrdinal("Amount_morethan_270"));

                        decimal due = 0;
                        decimal remainingPayment = payment;

                        if (payment >= balanceDue)
                        {
                            due = 0;
                            notDue = balanceDue - payment - notDue;
                        }
                        else
                        {
                            due = balanceDue - payment;
                        }

                        if (remainingPayment >= aMoreThan270 && aMoreThan270 > 0)
                        {
                            remainingPayment -= aMoreThan270;
                            aMoreThan270 = 0;
                        }
                        else if (aMoreThan270 != 0)
                        {
                            aMoreThan270 -= remainingPayment;
                            remainingPayment = 0;
                        }

                        if (remainingPayment >= a181_270 && a181_270 > 0)
                        {
                            remainingPayment -= a181_270;
                            a181_270 = 0;
                        }
                        else if (a181_270 != 0)
                        {
                            a181_270 -= remainingPayment;
                            remainingPayment = 0;
                        }

                        if (remainingPayment >= a91_180 && a91_180 > 0)
                        {
                            remainingPayment -= a91_180;
                            a91_180 = 0;
                        }
                        else if (a91_180 != 0)
                        {
                            a91_180 -= remainingPayment;
                            remainingPayment = 0;
                        }

                        if (remainingPayment >= a1_90 && a1_90 > 0)
                        {
                            remainingPayment -= a1_90;
                            a1_90 = 0;
                        }
                        else if (a1_90 != 0)
                        {
                            a1_90 -= remainingPayment;
                            remainingPayment = 0;
                        }

                        if (remainingPayment >= notDue && notDue > 0)
                        {
                            remainingPayment -= notDue;
                            notDue = 0;
                        }
                        else if (notDue != 0)
                        {
                            notDue -= remainingPayment;
                            remainingPayment = 0;
                        }

                        resultList.Add(new AgedReceivablesResultDto
                        {
                            CustomerCode = reader.IsDBNull(reader.GetOrdinal("CustomerCode")) ? null : reader.GetString(reader.GetOrdinal("CustomerCode")),
                            CustomerName = reader.IsDBNull(reader.GetOrdinal("CustomerName")) ? null : reader.GetString(reader.GetOrdinal("CustomerName")),
                            EmpName = reader.IsDBNull(reader.GetOrdinal("Emp_Name")) ? null : reader.GetString(reader.GetOrdinal("Emp_Name")),
                            Balance = balance,
                            BalanceDue = due,
                            BlanceNotDue = notDue,
                            Payment = payment,
                            AmountMoreThan270 = aMoreThan270,
                            Amount181_270 = a181_270,
                            Amount91_180 = a91_180,
                            Amount1_90 = a1_90,
                            Creditlimit = reader.GetDecimal(reader.GetOrdinal("Credit_limit")),
                            DuePeriodDays = reader.GetInt32(reader.GetOrdinal("Due_Period_Days"))
                        });
                    }

                    return resultList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching aged receivables: {ex.Message}", ex);
            }
        }


        #endregion ==================================  اعمار الديون


        #region ==========================================   أعمار الديون - شهري
        public async Task<IEnumerable<AgedReceivablesMonthlyResultDto>> GetAgedReceivablesMonthly(AgedReceivablesMonthlyDto obj)
        {
            try
            {
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "Acc_Debt_Age_Sp";

                    objCmd.Parameters.AddWithValue("@BRANCH_ID", obj.BRANCHID);
                    objCmd.Parameters.AddWithValue("@Facility_ID", obj.facilityId);
                    objCmd.Parameters.AddWithValue("@Cur_Date", obj.CurDate);
                    objCmd.Parameters.AddWithValue("@Emp_ID", obj.EmpID);
                    objCmd.Parameters.AddWithValue("@Group_ID", obj.GroupID);
                    objCmd.Parameters.AddWithValue("@CMDType", 2);

                    if (!string.IsNullOrEmpty(obj.CustomerCode))
                        objCmd.Parameters.AddWithValue("@CustomerCode", obj.CustomerCode);
                    if (!string.IsNullOrEmpty(obj.CustomerCode2))
                        objCmd.Parameters.AddWithValue("@CustomerCode2", obj.CustomerCode2);

                    await objCnn.OpenAsync();
                    var reader = await objCmd.ExecuteReaderAsync();

                    var resultList = new List<AgedReceivablesMonthlyResultDto>();

                    while (await reader.ReadAsync())
                    {
                        decimal balance = reader.GetDecimal(reader.GetOrdinal("Balance"));
                        if (balance > 0)
                            continue;

                        decimal balanceDue = reader.GetDecimal(reader.GetOrdinal("BalanceDue"));
                        decimal notDue = reader.GetDecimal(reader.GetOrdinal("BlanceNotDue"));
                        decimal payment = reader.GetDecimal(reader.GetOrdinal("Payment"));
                        decimal a1_90 = reader.GetDecimal(reader.GetOrdinal("Amount_1_90"));
                        decimal a91_180 = reader.GetDecimal(reader.GetOrdinal("Amount_91_180"));
                        decimal a181_270 = reader.GetDecimal(reader.GetOrdinal("Amount_181_270"));
                        decimal aMoreThan270 = reader.GetDecimal(reader.GetOrdinal("Amount_morethan_270"));

                        decimal due = 0;
                        decimal remainingPayment = payment;

                        if (payment >= balanceDue)
                        {
                            due = 0;
                            notDue = balanceDue - payment - notDue;
                        }
                        else
                        {
                            due = balanceDue - payment;
                        }

                        if (remainingPayment >= aMoreThan270 && aMoreThan270 > 0)
                        {
                            remainingPayment -= aMoreThan270;
                            aMoreThan270 = 0;
                        }
                        else if (aMoreThan270 != 0)
                        {
                            aMoreThan270 -= remainingPayment;
                            remainingPayment = 0;
                        }

                        if (remainingPayment >= a181_270 && a181_270 > 0)
                        {
                            remainingPayment -= a181_270;
                            a181_270 = 0;
                        }
                        else if (a181_270 != 0)
                        {
                            a181_270 -= remainingPayment;
                            remainingPayment = 0;
                        }

                        if (remainingPayment >= a91_180 && a91_180 > 0)
                        {
                            remainingPayment -= a91_180;
                            a91_180 = 0;
                        }
                        else if (a91_180 != 0)
                        {
                            a91_180 -= remainingPayment;
                            remainingPayment = 0;
                        }

                        if (remainingPayment >= a1_90 && a1_90 > 0)
                        {
                            remainingPayment -= a1_90;
                            a1_90 = 0;
                        }
                        else if (a1_90 != 0)
                        {
                            a1_90 -= remainingPayment;
                            remainingPayment = 0;
                        }

                        if (remainingPayment >= notDue && notDue > 0)
                        {
                            remainingPayment -= notDue;
                            notDue = 0;
                        }
                        else if (notDue != 0)
                        {
                            notDue -= remainingPayment;
                            remainingPayment = 0;
                        }

                        resultList.Add(new AgedReceivablesMonthlyResultDto
                        {
                            CustomerCode = reader.IsDBNull(reader.GetOrdinal("CustomerCode")) ? null : reader.GetString(reader.GetOrdinal("CustomerCode")),
                            CustomerName = reader.IsDBNull(reader.GetOrdinal("CustomerName")) ? null : reader.GetString(reader.GetOrdinal("CustomerName")),
                            EmpName = reader.IsDBNull(reader.GetOrdinal("Emp_Name")) ? null : reader.GetString(reader.GetOrdinal("Emp_Name")),
                            Balance = balance,
                            BalanceDue = due,
                            BlanceNotDue = notDue,
                            Payment = payment,
                            AmountMoreThan270 = aMoreThan270,
                            Amount181_270 = a181_270,
                            Amount91_180 = a91_180,
                            Amount1_90 = a1_90,
                            Creditlimit = reader.GetDecimal(reader.GetOrdinal("Credit_limit")),
                            DuePeriodDays = reader.GetInt32(reader.GetOrdinal("Due_Period_Days"))
                        });
                    }

                    return resultList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching aged receivables: {ex.Message}", ex);
            }
        }


        #endregion ==================================  أعمار الديون - شهري


        #region ==========================================  مقارنة بالسنوات
        public async Task<IEnumerable<CompareyearsDtoResultDto>> GetBudgetEstimateCompareyears(CompareyearsDto obj)
        {
            try
            {
                decimal NetYear1 = 0;
                decimal NetYear2 = 0;
                decimal NetYear3 = 0;

                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "Acc_Budget_Estimate_Master_SP";

                    objCmd.Parameters.AddWithValue("@Fin_year", obj.Finyear);
                    objCmd.Parameters.AddWithValue("@Fin_year2", obj.Finyear2);
                    objCmd.Parameters.AddWithValue("@Fin_year3", obj.Finyear3);
                    objCmd.Parameters.AddWithValue("@Acc_groupID", obj.AccgroupID);
                    objCmd.Parameters.AddWithValue("@Facility_ID", obj.FacilityID);
                    objCmd.Parameters.AddWithValue("@Branch_ID", obj.BranchID);
                    objCmd.Parameters.AddWithValue("@CMDTYPE", 13);

                    if (!string.IsNullOrEmpty(obj.AccAccountCode) && !string.IsNullOrEmpty(obj.AccAccountCode2))
                        objCmd.Parameters.AddWithValue("@Acc_Account_Code", obj.AccAccountCode);
                    objCmd.Parameters.AddWithValue("@Acc_Account_Code2", obj.AccAccountCode2);

                    if (!string.IsNullOrEmpty(obj.CCCodeFrom) && !string.IsNullOrEmpty(obj.CCCodeTo))
                        objCmd.Parameters.AddWithValue("@CC_Code_From", obj.CCCodeFrom);
                    objCmd.Parameters.AddWithValue("@CC_Code_To", obj.CCCodeTo);
                    objCmd.CommandTimeout = 600;

                    await objCnn.OpenAsync();
                    using (var reader = await objCmd.ExecuteReaderAsync())
                    {
                        var resultList = new List<CompareyearsDtoResultDto>();

                        while (await reader.ReadAsync())
                        {
                            decimal net1 = reader["Net_Year1"] != DBNull.Value ? Convert.ToDecimal(reader["Net_Year1"]) : 0;
                            decimal net2 = reader["Net_Year2"] != DBNull.Value ? Convert.ToDecimal(reader["Net_Year2"]) : 0;
                            decimal net3 = reader["Net_Year3"] != DBNull.Value ? Convert.ToDecimal(reader["Net_Year3"]) : 0;

                            // عكس السالب لموجب
                            net1 = Math.Abs(net1);
                            net2 = Math.Abs(net2);
                            net3 = Math.Abs(net3);

                            // تجميع المجاميع
                            NetYear1 += net1;
                            NetYear2 += net2;
                            NetYear3 += net3;

                            var item = new CompareyearsDtoResultDto
                            {
                                AccAccountName = reader.IsDBNull(reader.GetOrdinal("Acc_Account_Name")) ? null : reader.GetString(reader.GetOrdinal("Acc_Account_Name")),
                                AccAccountCode = reader.IsDBNull(reader.GetOrdinal("Acc_Account_Code")) ? null : reader.GetString(reader.GetOrdinal("Acc_Account_Code")),
                                NetYear1 = net1,
                                NetYear2 = net2,
                                NetYear3 = net3
                            };

                            resultList.Add(item);
                        }

                        return (resultList);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching budget estimate comparison: {ex.Message}", ex);
            }
        }

        #endregion ================================== مقارنة بالسنوات


        #region ========================================== تقارير احصائي

        public async Task<IEnumerable<DashboardStatusResultDto>> GetDashboardData(DashboardRequestDto request, int CmdType)
        {
            try
            {
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "Acc_Dashboard_SP";

                    objCmd.Parameters.AddWithValue("@USER_ID", currentData.UserId);
                    objCmd.Parameters.AddWithValue("@Emp_ID", currentData.EmpId);
                    objCmd.Parameters.AddWithValue("@Branch_ID", currentData.BranchId);
                    objCmd.Parameters.AddWithValue("@BRANCHS_ID", currentData.Branches);
                    objCmd.Parameters.AddWithValue("@Curr_Date", request.CurrDate);
                    objCmd.Parameters.AddWithValue("@Start_Date", request.StartDate);
                    objCmd.Parameters.AddWithValue("@End_Date", request.EndDate);
                    objCmd.Parameters.AddWithValue("@CMDTYPE", CmdType);
                    objCmd.Parameters.AddWithValue("@Facility_ID", currentData.FacilityId);
                    objCmd.Parameters.AddWithValue("@Fin_Year", currentData.FinYear);

                    objCmd.CommandTimeout = 600;

                    await objCnn.OpenAsync();
                    using (var reader = await objCmd.ExecuteReaderAsync())
                    {
                        var resultList = new List<DashboardStatusResultDto>();

                        while (await reader.ReadAsync())
                        {
                            var item = new DashboardStatusResultDto
                            {
                                Cnt = reader.IsDBNull(reader.GetOrdinal("Cnt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Cnt")),
                                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                                Name2 = reader.IsDBNull(reader.GetOrdinal("Name2")) ? null : reader.GetString(reader.GetOrdinal("Name2")),
                                Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? null : reader.GetString(reader.GetOrdinal("Color")),
                                Icon = reader.IsDBNull(reader.GetOrdinal("Icon")) ? null : reader.GetString(reader.GetOrdinal("Icon")),
                                Url = reader.IsDBNull(reader.GetOrdinal("Url")) ? null : reader.GetString(reader.GetOrdinal("Url"))
                            };

                            resultList.Add(item);
                        }

                        return resultList;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching dashboard data: {ex.Message}", ex);
            }
        }


        public async Task<IEnumerable<DashboardEstimatedactualResultDto>> GetDashboardDataEstimatedactual(DashboardRequestDto request, int CmdType)
        {

            try
            {
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.CommandText = "Acc_Dashboard_SP";

                    objCmd.Parameters.AddWithValue("@USER_ID", currentData.UserId);
                    objCmd.Parameters.AddWithValue("@Emp_ID", currentData.EmpId);
                    objCmd.Parameters.AddWithValue("@Branch_ID", currentData.BranchId);
                    objCmd.Parameters.AddWithValue("@BRANCHS_ID", currentData.Branches);
                    objCmd.Parameters.AddWithValue("@Curr_Date", request.CurrDate);
                    objCmd.Parameters.AddWithValue("@Start_Date", request.StartDate);
                    objCmd.Parameters.AddWithValue("@End_Date", request.EndDate);
                    objCmd.Parameters.AddWithValue("@CMDTYPE", CmdType);
                    objCmd.Parameters.AddWithValue("@Facility_ID", currentData.FacilityId);
                    objCmd.Parameters.AddWithValue("@Fin_Year", currentData.FinYear);


                    objCmd.CommandTimeout = 600;

                    await objCnn.OpenAsync();
                    using (var reader = await objCmd.ExecuteReaderAsync())
                    {
                        var resultList = new List<DashboardEstimatedactualResultDto>();

                        while (await reader.ReadAsync())
                        {
                            var item = new DashboardEstimatedactualResultDto
                            {
                                AccAccountId = reader.IsDBNull(reader.GetOrdinal("Acc_Account_Id")) ? 0 : reader.GetInt64(reader.GetOrdinal("Acc_Account_Id")),
                                AccAccountCode = reader.IsDBNull(reader.GetOrdinal("Acc_Account_Code")) ? null : reader.GetString(reader.GetOrdinal("Acc_Account_Code")),
                                AccAccountName = reader.IsDBNull(reader.GetOrdinal("Acc_Account_Name")) ? null : reader.GetString(reader.GetOrdinal("Acc_Account_Name")),
                                Budget = reader.IsDBNull(reader.GetOrdinal("Budget")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Budget")),
                                Actual = reader.IsDBNull(reader.GetOrdinal("Actual")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Actual")),
                            };

                            resultList.Add(item);
                        }

                        return resultList;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching dashboard data: {ex.Message}", ex);
            }
        }


        #endregion ========================================== تقارير احصائي
        #endregion =====================  Stored Procedure in Acc system


    }
}
