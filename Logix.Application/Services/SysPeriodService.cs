using AutoMapper;
using DocumentFormat.OpenXml.Drawing;
using Logix.Application.Common;
using Logix.Application.DTOs.ACC;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.ACC;
using Logix.Domain.Main;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Application.Services.Main
{
    public class SysPeriodService : GenericQueryService<SysPeriod, SysPeriodDto, SysPeriod>, ISysPeriodService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;


        public SysPeriodService(IQueryRepository<SysPeriod> queryRepository, IMainRepositoryManager mainRepositoryManager, IMapper mapper,
            ICurrentData session, ILocalizationService localization) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.session = session;
            this.localization = localization;
        }

        public async Task<IResult<SysPeriodDto>> Add(SysPeriodDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysPeriodDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");
            try
            {
                var item = _mapper.Map<SysPeriod>(entity);
                item.FacilityId = session.FacilityId;

                var newEntity = await _mainRepositoryManager.SysPeriodRepository.AddAndReturn(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysPeriodDto>(newEntity);
                return await Result<SysPeriodDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysPeriodDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult<SysPeriodEditDto>> Update(SysPeriodEditDto entity, CancellationToken cancellationToken = default)
        {
            try
            {
                if (entity == null) return await Result<SysPeriodEditDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");

                var item = await _mainRepositoryManager.SysPeriodRepository.GetById(entity.Id);

                if (item == null) return await Result<SysPeriodEditDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");

                _mapper.Map(entity, item);

                _mainRepositoryManager.SysPeriodRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysPeriodEditDto>.SuccessAsync(_mapper.Map<SysPeriodEditDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                return await Result<SysPeriodEditDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysPeriodRepository.GetById(Id);
                if (item == null) return Result<SysPeriodDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

                item.IsDeleted = true;

                _mainRepositoryManager.SysPeriodRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysPeriodDto>.SuccessAsync(_mapper.Map<SysPeriodDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysPeriodDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<int>> IsDateRangeOverlap(long Id, string StartDate, string EndDate, long SystemId, long FacilityId, CancellationToken cancellationToken = default)
        {
            try
            {
                var items = await _mainRepositoryManager.SysPeriodRepository.GetAll(x => x.FacilityId == FacilityId && x.SystemId == SystemId
                && (Id == 0 || x.Id != Id) && x.IsDeleted == false);

                if (items.Any())
                {
                    DateTime sDate = DateTime.ParseExact(StartDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    DateTime eDate = DateTime.ParseExact(EndDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    var final = items.Where(x =>
                        (sDate >= DateTime.ParseExact(x.StartDate!, "yyyy/MM/dd", CultureInfo.InvariantCulture) && sDate <= DateTime.ParseExact(x.EndDate!, "yyyy/MM/dd", CultureInfo.InvariantCulture))
                        || (eDate >= DateTime.ParseExact(x.StartDate!, "yyyy/MM/dd", CultureInfo.InvariantCulture) && eDate <= DateTime.ParseExact(x.EndDate!, "yyyy/MM/dd", CultureInfo.InvariantCulture))

                        || (DateTime.ParseExact(x.StartDate!, "yyyy/MM/dd", CultureInfo.InvariantCulture) >= sDate && DateTime.ParseExact(x.StartDate!, "yyyy/MM/dd", CultureInfo.InvariantCulture) <= eDate)
                        || (DateTime.ParseExact(x.EndDate!, "yyyy/MM/dd", CultureInfo.InvariantCulture) >= sDate && DateTime.ParseExact(x.EndDate!, "yyyy/MM/dd", CultureInfo.InvariantCulture) <= eDate)
                        );

                    int count = final.Count();
                    return await Result<int>.SuccessAsync(count);
                }
                return await Result<int>.SuccessAsync(0);
            }
            catch (Exception exp)
            {
                return await Result<int>.FailAsync($"EXP, Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }
    }
}
