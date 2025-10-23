using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Services.Main
{
    public class SysCustomerGroupAccountService : GenericQueryService<SysCustomerGroupAccount, SysCustomerGroupAccountDto, SysCustomerGroupAccountsVw>, ISysCustomerGroupAccountService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IAccRepositoryManager _accRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;

        public SysCustomerGroupAccountService(IQueryRepository<SysCustomerGroupAccount> queryRepository,
            IMapper mapper,
            IMainRepositoryManager mainRepositoryManager,
            IAccRepositoryManager accRepositoryManager,
            ICurrentData session,
            ILocalizationService localization) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._accRepositoryManager = accRepositoryManager;
            this._mapper = mapper;
            this.session = session;
            this.localization = localization;
        }

        public async Task<IResult<SysCustomerGroupAccountDto>> Add(SysCustomerGroupAccountDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysCustomerGroupAccountDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");

            try
            {
                //check account
                if (!string.IsNullOrEmpty(entity.AccAccountCode))
                {
                    var account = await _accRepositoryManager.AccAccountRepository.GetOne(a => a.AccAccountCode == entity.AccAccountCode
                    && a.IsDeleted == false && a.FacilityId == session.FacilityId);
                    if (account != null)
                        entity.AccountId = account.AccAccountId;
                    else
                        return await Result<SysCustomerGroupAccountDto>.FailAsync($"{localization.GetResource1("AccountNotExsists")}");
                }
                var item = _mapper.Map<SysCustomerGroupAccount>(entity);
                //item.CreatedOn = DateTime.Now;
                //item.CreatedBy = session.UserId;
                var newEntity = await _mainRepositoryManager.SysCustomerGroupAccountRepository.AddAndReturn(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysCustomerGroupAccountDto>(newEntity);

                return await Result<SysCustomerGroupAccountDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysCustomerGroupAccountDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysCustomerGroupAccountRepository.GetById(Id);
                if (item == null) return Result<SysCustomerGroupAccountDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");
                item.IsDeleted = true;
                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;

                _mainRepositoryManager.SysCustomerGroupAccountRepository.Update(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysCustomerGroupAccountDto>.SuccessAsync(_mapper.Map<SysCustomerGroupAccountDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysCustomerGroupAccountDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysCustomerGroupAccountRepository.GetById(Id);
                if (item == null) return Result<SysCustomerGroupAccountDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");
                item.IsDeleted = true;
                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;

                _mainRepositoryManager.SysCustomerGroupAccountRepository.Update(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysCustomerGroupAccountDto>.SuccessAsync(_mapper.Map<SysCustomerGroupAccountDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysCustomerGroupAccountDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<SysCustomerGroupAccountDto>> Update(SysCustomerGroupAccountDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysCustomerGroupAccountDto>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");

            try
            {
                var item = _mapper.Map<SysCustomerGroupAccount>(entity);
                item.ModifiedOn = DateTime.Now;
                item.ModifiedBy = session.UserId;
                _mainRepositoryManager.SysCustomerGroupAccountRepository.Update(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysCustomerGroupAccountDto>.SuccessAsync(entity, "item updated successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysCustomerGroupAccountDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }
    }
}
