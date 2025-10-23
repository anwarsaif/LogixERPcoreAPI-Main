namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface IDbFunctionsRepository
    {
        Task<int> DateDiff_day2(string SDate, string EDate);
        Task<decimal> ApplyPolicies(long Facility_ID, long Policie_ID, long Emp_ID);
        Task<List<string>> HR_Get_childe_Department_Fn(long DepId);
        Task<string> Steps_User_Fn(long userId, int appTypeId, int type);
        Task<decimal> HR_Ticket_Balance_Fn(string CurrentDate, long EmpId);
        Task<decimal> HR_Visa_Balance_Fn(string CurrentDate, long EmpId);
    }
}
