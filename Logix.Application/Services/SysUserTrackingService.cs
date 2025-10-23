using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Application.Services.Main
{
    public class SysUserTrackingService : GenericQueryService<SysUserTracking, SysUserTrackingDto, SysUserTrackingVw>, ISysUserTrackingService
    {
		private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;

        public SysUserTrackingService(IQueryRepository<SysUserTracking> queryRepository,
            IMapper mapper,
            IMainRepositoryManager mainRepositoryManager,
            ICurrentData session,
            ILocalizationService localization) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.session = session;
            this.localization = localization;
        }

        public async Task<IResult<SysUserTrackingDto>> Add(SysUserTrackingDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysUserTrackingDto>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");
            try
            {
                entity.UserId = Convert.ToInt32(session.UserId);
                entity.EmpId = session.EmpId;
                entity.CreatedOn = DateTime.Now;
                var newEntity = await _mainRepositoryManager.SysUserTrackingRepository.AddAndReturn(_mapper.Map<SysUserTracking>(entity));
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysUserTrackingDto>(newEntity);
                return await Result<SysUserTrackingDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysUserTrackingDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

		public async Task<IResult<List<SysUserTrackingVm>>> GetUserTrackingRp(SysUserTrackingFilterDto filter, CancellationToken cancellationToken = default)
		{
			try
			{
				var getReport = await _mainRepositoryManager.SysUserTrackingRepository.GetAllVw(a =>
					(string.IsNullOrEmpty(filter.ActivityDateFrom) || (a.CreatedOn != null && a.CreatedOn.Value.Date >= DateTime.ParseExact(filter.ActivityDateFrom, "yyyy/MM/dd", CultureInfo.InvariantCulture)))
					&& (string.IsNullOrEmpty(filter.ActivityDateTo) || (a.CreatedOn != null && a.CreatedOn.Value.Date <= DateTime.ParseExact(filter.ActivityDateTo, "yyyy/MM/dd", CultureInfo.InvariantCulture)))
					&& (filter.UserId == null || (a.UserId != null && a.UserId == filter.UserId))
				);

				if (getReport != null)
				{
					var res = getReport.ToList();

					List<SysUserTrackingVm> results = new();
					foreach (var item in res)
					{
						SysUserTrackingVm result = new SysUserTrackingVm()
						{
							Url = item.Url,
							UserFullName = item.UserFullname,
							Date = item.CreatedOn != null ? item.CreatedOn.Value.ToString("yyyy/MM/dd hh:mm:ss tt", CultureInfo.InvariantCulture) : "",
						};
						results.Add(result);
					}
					return await Result<List<SysUserTrackingVm>>.SuccessAsync(results);
				}

				return await Result<List<SysUserTrackingVm>>.SuccessAsync(new List<SysUserTrackingVm>());
			}
			catch (Exception ex)
			{
				return await Result<List<SysUserTrackingVm>>.FailAsync($"EXP in {this.GetType()}, Meesage: {ex.Message}");
			}
		}

		public Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<SysUserTracking>> Update(SysUserTracking entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}