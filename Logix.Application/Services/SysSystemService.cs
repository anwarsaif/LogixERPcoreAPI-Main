using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.DTOs.RPT;
using Logix.Application.DTOs.TS;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.ACC;
using Logix.Domain.Main;
using System.Globalization;

namespace Logix.Application.Services.Main
{
    public class SysSystemService : GenericQueryService<SysSystem, SysSystemDto, SysSystem>, ISysSystemService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData currentData;

        public SysSystemService(IQueryRepository<SysSystem> queryRepository, IMainRepositoryManager mainRepositoryManager, IMapper mapper, ICurrentData currentData) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.currentData = currentData;

        }
        public Task<IResult<SysSystemDto>> Add(SysSystemDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> ChangeActive(SysSystemDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        //public async Task<IResult<IEnumerable<MainListDto>>> GetMainList(long userId, int sysId, int cmdType)
        //{
        //    var items = await _mainRepositoryManager.SysSystemRepository.GetMainList(userId, sysId, cmdType);

        //   // var itemMap = _mapper.Map<IEnumerable<SysSystemDto>>(items);
        //    if (items == null) return await Result<IEnumerable<MainListDto>>.FailAsync("No Lists Found");
        //    return await Result<IEnumerable<MainListDto>>.SuccessAsync(items, "Main List records retrieved");
        //}
        public async Task<IResult<IEnumerable<ScreenSearchDto>>> SearchScreens(string name)
        {
            var items = await _mainRepositoryManager.SysSystemRepository.SearchScreen(name, currentData.GroupId, currentData.Language);

            // var itemMap = _mapper.Map<IEnumerable<SysSystemDto>>(items);
            if (items == null) return await Result<IEnumerable<ScreenSearchDto>>.FailAsync("No Screens Found");
            return await Result<IEnumerable<ScreenSearchDto>>.SuccessAsync(items, "");
        }


        //public async Task<IResult<IEnumerable<SubListDto>>> GetSubList(long userId, int sysId, int cmdType, int parent)
        //{
        //    var items = await _mainRepositoryManager.SysSystemRepository.GetSubList(userId, sysId, cmdType, parent);

        //    // var itemMap = _mapper.Map<IEnumerable<SysSystemDto>>(items);
        //    if (items == null) return await Result<IEnumerable<SubListDto>>.FailAsync("No Lists Found");
        //    foreach(var item in items)
        //    {
        //        if(!string.IsNullOrEmpty(item.SCREEN_URL))
        //        {
        //            item.SCREEN_URL = item.SCREEN_URL.StartsWith("/") ? item.SCREEN_URL : item.Folder + "/" + item.SCREEN_URL ;
        //            item.SCREEN_URL = $"{currentData.OldBaseUrl}{item.SCREEN_URL}";
        //        }
        //        if (item.IsCore==false)
        //        {
        //            item.URL = item.SCREEN_URL;
        //        }
        //    }
        //    return await Result<IEnumerable<SubListDto>>.SuccessAsync(items, "");
        //}

        public async Task<IResult<IEnumerable<SysBranchVw>>> GetSysBranchVw()
        {
            var items = await _mainRepositoryManager.SysSystemRepository.GetSysBranchVw();
            if (items == null) return await Result<IEnumerable<SysBranchVw>>.FailAsync("No Data Found");
            return await Result<IEnumerable<SysBranchVw>>.SuccessAsync(items, "records retrieved");
        }
        public async Task<IResult<IEnumerable<TsTasksVwDto>>> GetTasksByUser(long userId)
        {
            try
            {
                var items = await _mainRepositoryManager.SysSystemRepository.GetTasksByUser(currentData.UserId);
                if (items == null) return await Result<IEnumerable<TsTasksVwDto>>.FailAsync("No Data Found");
                var tasks = _mapper.Map<IEnumerable<TsTasksVwDto>>(items);
                tasks = tasks.Where(x => x.AssigneeToUserId.Split(',').Contains(userId.ToString()));
                foreach (var task in tasks)
                {
                    if (!string.IsNullOrEmpty(task.SendDate) && !string.IsNullOrEmpty(task.DueDate))
                    {
                        var send = DateTime.ParseExact(task.SendDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                        var due = DateTime.ParseExact(task.DueDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                        int daysDifference = (due - send).Days;
                        int daysRemains = (due - DateTime.Now).Days;
                        task.CntDays = daysDifference;
                        task.RemainingDays = daysRemains;
                        task.Persent = (int)Math.Round((task.RemainingDays * 100) / (double)task.CntDays, 2);
                    }

                }
                return await Result<IEnumerable<TsTasksVwDto>>.SuccessAsync(tasks, "records retrieved");
            }
            catch (Exception ex)
            {
                return await Result<IEnumerable<TsTasksVwDto>>.FailAsync($"EXP: {ex.Message}");
            }
        }

        public async Task<IResult<IEnumerable<AccFacilitiesVw>>> GetFacilities()
        {
            var items = await _mainRepositoryManager.SysSystemRepository.GetFacilities();
            if (items == null) return await Result<IEnumerable<AccFacilitiesVw>>.FailAsync("No Data Found");
            return await Result<IEnumerable<AccFacilitiesVw>>.SuccessAsync(items, "records retrieved");
        }

        public async Task<IResult<IEnumerable<AccFinancialYear>>> GetFinancialYears(long facilityId)
        {
            var items = await _mainRepositoryManager.SysSystemRepository.GetFinancialYears(facilityId);
            if (items == null) return await Result<IEnumerable<AccFinancialYear>>.FailAsync("No Data Found");
            return await Result<IEnumerable<AccFinancialYear>>.SuccessAsync(items.Where(f => f.IsDeleted == false), "");
        }

        public async Task<IResult<IEnumerable<AccFinancialYear>>> GetFinancialYears()
        {
            var items = await _mainRepositoryManager.SysSystemRepository.GetFinancialYears();
            if (items == null) return await Result<IEnumerable<AccFinancialYear>>.FailAsync("No Data Found");
            return await Result<IEnumerable<AccFinancialYear>>.SuccessAsync(items.Where(f => f.IsDeleted == false), "");
        }
        public async Task<IResult<IEnumerable<AccFinancialYear>>> GetFinancialYearsAll(long facilityId)
        {
            var items = await _mainRepositoryManager.SysSystemRepository.GetFinancialYearsAll(facilityId);
            if (items == null) return await Result<IEnumerable<AccFinancialYear>>.FailAsync("No Data Found");
            return await Result<IEnumerable<AccFinancialYear>>.SuccessAsync(items.Where(f => f.IsDeleted == false), "");
        }
        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<SysSystemDto>> Update(SysSystemDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public async Task<IResult<List<MainListDto>>> GetUserScreens(long userId, int sysId)
        {
            var items = await _mainRepositoryManager.SysSystemRepository.GetUserScreens(userId, sysId);

            if (items == null) return await Result<List<MainListDto>>.FailAsync("No Lists Found");
            return await Result<List<MainListDto>>.SuccessAsync(items, "UserScreens List records retrieved");
        }
        public async Task<IResult<List<SysSystemDto>>> GetSystemsToShowInHome()
        {
            var items = await _mainRepositoryManager.SysSystemRepository.GetSystemsToShowInHome();

            return await Result<List<SysSystemDto>>.SuccessAsync(items, "SystemList records retrieved");
        }

        public async Task<IResult<List<RptReportDto>>> GetCustomReportMenu(long systemId, string sysGroupId)
        {
            var items = await _mainRepositoryManager.SysSystemRepository.GetCustomReportMenu(systemId, sysGroupId);

            if (items == null) return await Result<List<RptReportDto>>.FailAsync("No Lists Found");
            return await Result<List<RptReportDto>>.SuccessAsync(items, "UserScreens List records retrieved");
        }


    }
}
