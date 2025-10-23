using System.Data;
using Logix.Application.DTOs.ACC;
using Logix.Application.DTOs.HR;
using Logix.Application.DTOs.Main;
using Logix.Application.DTOs.WF;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface IStoredProceduresRepository
    {
        Task<IEnumerable<SendEmailDto>> WF_UsersEmail_App_Sp(long id);
        Task<IEnumerable<HrVacationBalanceALLFilterDto>> HR_VacationBalance_SP(HrVacationBalanceALLSendFilterDto filter);
        Task<HrVacationEmpBalanceDto> HR_Vacation_Balance(HrVacationEmpBalanceDto filter);
        Task<IEnumerable<HrVacationEmpBalanceDto>> HR_Vacation_Balance_Search(HrVacationEmpBalanceDto filter);
        Task<long> HR_Payroll_D_SP(HRPayrollDStoredProcedureDto entity);
        Task<IEnumerable<HRAttendanceReportDto>> HR_Attendance_Report_SP(HRAttendanceReportFilterDto filter);
        Task<IEnumerable<HRAttendanceTotalReportSPDto>> HR_Attendance_TotalReport_SP(HRAttendanceTotalReportSPFilterDto filter);
        Task<bool> HR_Attendance_SP_CmdType_1(HrAttendanceDto entity);

        Task<IEnumerable<HRAttendanceReport5Dto>> HR_Attendance_Report5_SP(HRAttendanceReport5FilterDto filter);
        Task<IEnumerable<HRAttendanceReport4Dto>> HR_Attendance_Report4_SP(HRAttendanceReport4FilterDto filter);

        Task<IEnumerable<HRAddMultiAttendanceDto>> AttendanceSearchForMultiAdd(HRAddMultiAttendanceFilterDto filter);
        Task<long> HR_Sick_leave_Sp(long EmpID, long CmdType, long? VacationID);

        int Absence_NotAttendance(AbsenceNotAttendanceDto entity);
        Task<IEnumerable<HRPayrollManuallCreateSpDto>> HR_Payroll_Create2_Sp(HRPayrollCreate2SpFilterDto filter);
        Task<IEnumerable<HRPayrollCreate2SpDto>> HR_Payroll_Create_Sp(HRPayrollCreateSpFilterDto filter);
        Task<IEnumerable<HRPreparationSalariesLoanDto>> HR_Preparation_Salaries_Loan_SP(HRPreparationSalariesLoanFilterDto filter);
        Task<IEnumerable<EmployeeGosiDto>> HR_RPEmployee_Sp(EmployeeGosiSearchtDto filter);

        Task<HrEmployeeLeaveResultDto> GetEmployeeLeaveData(HrLeaveGetDataDto obj);

        Task<IEnumerable<HRAttendanceTotalReportDto>> HR_Attendance_TotalReportManagerial_SP(HRAttendanceTotalReportFilterDto filter);
        Task<IEnumerable<HrEmployeeIncremenResultDto>> HR_Increments_During_Time_SP(IncrementsBothFilterDto filter);
        Task<IEnumerable<HRAttendanceTotalReportNewSPDto>> HR_Attendance_TotalReportNew_SP(HRAttendanceTotalReportFilterDto filter);
        Task<IEnumerable<HRAttendanceReport6SP>> HR_Attendance_Report6_SP(HRAttendanceReport6FilterSP filter);

        Task<HREmpClearanceSpDto> HR_Emp_Clearance_Sp(long EmpId, string LastWorkingDate);

        Task<int> HR_Reaset_Attendance_SP(HrAttendanceResetDto entity);
        Task<IEnumerable<HRPayrollAdvancedResultDto>> HR_Payroll_Create_Advanced_Sp(HRPayrollCreateSpFilterDto filter);

        Task<IEnumerable<HRApprovalAbsencesReportDto>> HRApprovalAbsencesReport(HRApprovalAbsencesReportFilterDto filter);

        Task<DataTable> HR_Payroll_Clearance_Sp(string EmpCode, string EmpName);





        #region Stored Procedure in main system
        Task<string> GetDecryptUserPassword(long id);
        Task<string> AddUserByProcedure(SysUserDto obj);
        Task<string> EditUserByProcedure(SysUserEditDto obj);

        Task<byte[]> EncryptUserPassword(string PassWord);

        #endregion

        Task<DataTable> GetSalaryData_WF(long EmpId, long FacilityId, string CurrentDate);
        Task<DataTable> GetDues_WF(long EmpId, string CurrentDate);



        #region Stored Procedure in Acc system

        Task<IEnumerable<TrialBalanceSheetDtoResult>> GetTrialBalanceSheet(TrialBalanceSheetDto obj);
        Task<IEnumerable<IncomeStatementMonthResultDto>> GetIncomeStatementMonth(IncomeStatementMonthtDto obj);
        Task<IEnumerable<ProfitandLossResultDto>> GetProfitandLoss(ProfitandLossDto obj);
        Task<IEnumerable<CashFlowsResultDto>> GetCashFlows(CashFlowsDto obj);
        Task<IEnumerable<AgedReceivablesResultDto>> GetAgedReceivables(AgedReceivablesDto obj);
        Task<IEnumerable<AgedReceivablesMonthlyResultDto>> GetAgedReceivablesMonthly(AgedReceivablesMonthlyDto obj);
        Task<IEnumerable<CompareyearsDtoResultDto>> GetBudgetEstimateCompareyears(CompareyearsDto obj);
        Task<IEnumerable<DashboardStatusResultDto>> GetDashboardData(DashboardRequestDto obj, int CmdType);
        Task<IEnumerable<DashboardEstimatedactualResultDto>> GetDashboardDataEstimatedactual(DashboardRequestDto obj, int CmdType);

        #endregion  Stored Procedure in Acc system


    }

}
