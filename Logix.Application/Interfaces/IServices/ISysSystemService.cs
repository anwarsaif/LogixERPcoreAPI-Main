using Logix.Application.DTOs.Main;
using Logix.Application.DTOs.RPT;
using Logix.Application.DTOs.TS;
using Logix.Application.Wrapper;
using Logix.Domain.ACC;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysSystemService : IGenericQueryService<SysSystemDto, SysSystem>
    {
        Task<IResult<SysSystemDto>> Add(SysSystemDto entity, CancellationToken cancellationToken = default);

        Task<IResult> Remove(int Id, CancellationToken cancellationToken = default);
        Task<IResult<SysSystemDto>> Update(SysSystemDto entity, CancellationToken cancellationToken = default);
        Task<IResult> ChangeActive(SysSystemDto entity, CancellationToken cancellationToken = default);

        //Task<IResult<IEnumerable<MainListDto>>> GetMainList(long userId, int sysId, int cmdType);
        //Task<IResult<IEnumerable<SubListDto>>> GetSubList(long userId, int sysId, int cmdType, int parent);
        Task<IResult<IEnumerable<SysBranchVw>>> GetSysBranchVw();
        Task<IResult<IEnumerable<AccFacilitiesVw>>> GetFacilities();
        Task<IResult<IEnumerable<AccFinancialYear>>> GetFinancialYears(long facilityId);
        Task<IResult<IEnumerable<AccFinancialYear>>> GetFinancialYears();
        Task<IResult<IEnumerable<AccFinancialYear>>> GetFinancialYearsAll(long facilityId);
        Task<IResult<IEnumerable<ScreenSearchDto>>> SearchScreens(string name);
        Task<IResult<IEnumerable<TsTasksVwDto>>> GetTasksByUser(long userId);

        Task<IResult<List<MainListDto>>> GetUserScreens(long userId, int sysId);
        Task<IResult<List<SysSystemDto>>> GetSystemsToShowInHome();
        Task<IResult<List<RptReportDto>>> GetCustomReportMenu(long systemId, string sysGroupId);

    }
}