using System.Linq.Expressions;
using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Services.Main
{
    public class SysCurrencyService : GenericQueryService<SysCurrency, SysCurrencyDto, SysCurrencyListVw>, ISysCurrencyService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IAccRepositoryManager _accRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ILocalizationService localization;


        public SysCurrencyService(IQueryRepository<SysCurrency> queryRepository,
            IMainRepositoryManager mainRepositoryManager,
            IAccRepositoryManager accRepositoryManager,
            IMapper mapper,
            ILocalizationService localization) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._accRepositoryManager = accRepositoryManager;
            this._mapper = mapper;
            this.localization = localization;
        }

        public async Task<IResult<SysCurrencyDto>> Add(SysCurrencyDto entity, CancellationToken cancellationToken = default)
        {
            try
            {
                if (entity == null) return await Result<SysCurrencyDto>.FailAsync($"{localization.GetResource1("AddNullEntity")}");

                var item = _mapper.Map<SysCurrency>(entity);
                var newEntity = await _mainRepositoryManager.SysCurrencyRepository.AddAndReturn(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                return await Result<SysCurrencyDto>.SuccessAsync(_mapper.Map<SysCurrencyDto>(newEntity), "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysCurrencyDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult<SysCurrencyEditDto>> Update(SysCurrencyEditDto entity, CancellationToken cancellationToken = default)
        {
            try
            {
                if (entity == null) return await Result<SysCurrencyEditDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");

                var item = await _mainRepositoryManager.SysCurrencyRepository.GetById(entity.Id);
                if (item == null) return await Result<SysCurrencyEditDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");

                _mapper.Map(entity, item);
                _mainRepositoryManager.SysCurrencyRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                return await Result<SysCurrencyEditDto>.SuccessAsync(_mapper.Map<SysCurrencyEditDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
                return await Result<SysCurrencyEditDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysCurrencyRepository.GetById(Id);
                if (item == null) return Result<SysCurrencyDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

                var journals = await _accRepositoryManager.AccJournalMasterRepository.GetAll(x => x.JId,
                    x => x.CurrencyId == Id && x.FlagDelete == false);
                if (journals.Any())
                    return Result<SysCurrencyDto>.Fail($"{localization.GetMainResource("CantDeletedTheCurrency")}");

                var accounts = await _accRepositoryManager.AccAccountRepository.GetAll(x => x.AccAccountId,
                    x => x.CurrencyId == Id && x.IsDeleted == false);
                if (accounts.Any())
                    return Result<SysCurrencyDto>.Fail($"{localization.GetMainResource("CantDeletedTheCurrency")}");

                item.IsDeleted = true;
                _mainRepositoryManager.SysCurrencyRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                return await Result<SysCurrencyDto>.SuccessAsync(_mapper.Map<SysCurrencyDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysCurrencyDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public Task<IResult<IEnumerable<SysCurrency>>> GetAllVW(Expression<Func<SysCurrency, bool>> expression, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<SysCurrency>> GetOneVW(Expression<Func<SysCurrency, bool>> expression, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
