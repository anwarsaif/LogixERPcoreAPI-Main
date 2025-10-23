using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Application.Services.Main
{
    public class SysExchangeRateService : GenericQueryService<SysExchangeRate, SysExchangeRateDto, SysExchangeRateListVW>, ISysExchangeRateService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;
        private readonly IQueryRepository<SysExchangeRate> myQueryRepository;

        public SysExchangeRateService(IQueryRepository<SysExchangeRate> queryRepository,
            IMainRepositoryManager mainRepositoryManager,
            IMapper mapper,
            ICurrentData session,
            ILocalizationService localization,
            IQueryRepository<SysExchangeRate> myQueryRepository) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.session = session;
            this.myQueryRepository = myQueryRepository;
            this.localization = localization;
        }

        public async Task<IResult<SysExchangeRateDto>> Add(SysExchangeRateDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysExchangeRateDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");

            try
            {
                var item = _mapper.Map<SysExchangeRate>(entity);
                item.CreatedBy = this.session.UserId;
                item.CreatedOn = DateTime.Now;

                var newEntity = await _mainRepositoryManager.SysExchangeRateRepository.AddAndReturn(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysExchangeRateDto>(newEntity);

                return await Result<SysExchangeRateDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {

                return await Result<SysExchangeRateDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult<SysExchangeRateEditDto>> Update(SysExchangeRateEditDto entity, CancellationToken cancellationToken = default)
        {
            try
            {
                if (entity == null) return await Result<SysExchangeRateEditDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");

                var item = await _mainRepositoryManager.SysExchangeRateRepository.GetById(entity.Id);

                if (item == null) return await Result<SysExchangeRateEditDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");

                _mapper.Map(entity, item);
                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;

                _mainRepositoryManager.SysExchangeRateRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysExchangeRateEditDto>.SuccessAsync(_mapper.Map<SysExchangeRateEditDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
                return await Result<SysExchangeRateEditDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.SysExchangeRateRepository.GetById(Id);
            if (item == null) return Result<SysExchangeRateDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

            item.ModifiedBy = session.UserId;
            item.ModifiedOn = DateTime.Now;
            item.IsDeleted = true;

            _mainRepositoryManager.SysExchangeRateRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysExchangeRateDto>.SuccessAsync(_mapper.Map<SysExchangeRateDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysExchangeRateDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.SysExchangeRateRepository.GetById(Id);
            if (item == null) return Result<SysExchangeRateDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

            item.ModifiedBy = session.UserId;
            item.ModifiedOn = DateTime.Now;
            item.IsDeleted = true;

            _mainRepositoryManager.SysExchangeRateRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysExchangeRateDto>.SuccessAsync(_mapper.Map<SysExchangeRateDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysExchangeRateDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<IEnumerable<SysExchangeRateVw>>> GetAllExRateVw(Expression<Func<SysExchangeRateVw, bool>> expression, CancellationToken cancellationToken = default)
        {
            try
            {
                var items = await myQueryRepository.GetAllFrom<SysExchangeRateVw>(expression);
                if (items == null) return await Result<IEnumerable<SysExchangeRateVw>>.FailAsync($"{localization.GetResource1("NoData")}");

                return await Result<IEnumerable<SysExchangeRateVw>>.SuccessAsync(items, "records retrieved");

            }
            catch (Exception exc)
            {
                return await Result<IEnumerable<SysExchangeRateVw>>.FailAsync($"EXP in {GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult<IEnumerable<SysExchangeRateVw>>> GetAllExRateVw(CancellationToken cancellationToken = default)
        {
            try
            {
                var items = await myQueryRepository.GetAllFrom<SysExchangeRateVw>();
                if (items == null) return await Result<IEnumerable<SysExchangeRateVw>>.FailAsync($"{localization.GetResource1("NoData")}");

                return await Result<IEnumerable<SysExchangeRateVw>>.SuccessAsync(items, "records retrieved");

            }
            catch (Exception exc)
            {
                return await Result<IEnumerable<SysExchangeRateVw>>.FailAsync($"EXP in {GetType()}, Meesage: {exc.Message}");
            }
        }
    }
}

