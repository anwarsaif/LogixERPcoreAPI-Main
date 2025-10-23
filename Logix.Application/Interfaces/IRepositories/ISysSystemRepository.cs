using Logix.Application.DTOs.Main;
using Logix.Application.DTOs.RPT;
using Logix.Domain.ACC;
using Logix.Domain.Main;
using Logix.Domain.TS;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysSystemRepository : IGenericRepository<SysSystem>
    {
        //Task<IEnumerable<MainListDto>> GetMainList(long userId, int sysId, int cmdType);
        //Task<IEnumerable<SubListDto>> GetSubList(long userId, int sysId, int cmdType, int parent);
        Task<IEnumerable<SysBranchVw>> GetSysBranchVw();
        Task<IEnumerable<AccFacilitiesVw>> GetFacilities();
        Task<IEnumerable<AccFinancialYear>> GetFinancialYears(long facilityId);
        Task<IEnumerable<AccFinancialYear>> GetFinancialYears();
        Task<IEnumerable<AccFinancialYear>> GetFinancialYearsAll(long facilityId);
        Task<IEnumerable<ScreenSearchDto>> SearchScreen(string name, int groupId, int language);
        Task<IEnumerable<TsTasksVw>> GetTasksByUser(long userId);


        Task<List<MainListDto>> GetUserScreens(long userId, int sysId);
        Task<List<SysSystemDto>> GetSystemsToShowInHome();
        Task<List<RptReportDto>> GetCustomReportMenu(long systemId, string sysGroupId);

    }
}
