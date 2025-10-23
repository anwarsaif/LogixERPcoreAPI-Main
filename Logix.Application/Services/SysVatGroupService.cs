using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Services.Main
{
    public class SysVatGroupService : GenericQueryService<SysVatGroup, SysVatGroupDto, SysVatGroupVw>, ISysVatGroupService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData currentData;
        private readonly ILocalizationService localization;
        private readonly IAccRepositoryManager accRepositoryManager;

        public SysVatGroupService(IQueryRepository<SysVatGroup> queryRepository, IMainRepositoryManager mainRepositoryManager, IMapper mapper, ICurrentData currentData, ILocalizationService localization, IAccRepositoryManager accRepositoryManager) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.currentData = currentData;
            this.localization = localization;
            this.accRepositoryManager = accRepositoryManager;
        }
        public async Task<IResult<SysVatGroupDto>> Add(SysVatGroupDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysVatGroupDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");

            try
            {
                long SalesVATAccountID = 0;
                long PurchasesVATAccountID = 0;

                if (!string.IsNullOrEmpty(entity.SalesVatAccountCode))
                {
                    var SalesVatAccount = await accRepositoryManager.AccAccountRepository
                        .GetAccountIdByCode(entity.SalesVatAccountCode, currentData.FacilityId);

                    if (SalesVatAccount != null)
                    {
                        SalesVATAccountID = SalesVatAccount;
                    }
                    else
                    {
                        return await Result<SysVatGroupDto>.WarningAsync(
                            localization.GetAccResource("AccAccountNotfind") + entity.SalesVatAccountCode);
                    }
                }

                if (!string.IsNullOrEmpty(entity.PurchasesVatAccountCode))
                {
                    var PurchasesVAT = await accRepositoryManager.AccAccountRepository
                         .GetAccountIdByCode(entity.PurchasesVatAccountCode, currentData.FacilityId);


                    if (PurchasesVAT != null)
                    {
                        PurchasesVATAccountID = PurchasesVAT;
                    }
                    else
                    {
                        return await Result<SysVatGroupDto>.WarningAsync(
                            localization.GetAccResource("AccAccountNotfind") + entity.PurchasesVatAccountCode);
                    }
                }

                entity.SalesVatAccountId = SalesVATAccountID;
                entity.PurchasesVatAccountId = PurchasesVATAccountID;
                entity.FacilityId = currentData.FacilityId;

                var item = _mapper.Map<SysVatGroup>(entity);
                var newEntity = await _mainRepositoryManager.SysVatGroupRepository.AddAndReturn(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysVatGroupDto>(newEntity);


                return await Result<SysVatGroupDto>.SuccessAsync(entityMap, localization.GetMessagesResource("success"));
            }
            catch (Exception exc)
            {

                return await Result<SysVatGroupDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }

        }

        public async Task<IResult<SysVatGroupEditDto>> Update(SysVatGroupEditDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysVatGroupEditDto>.FailAsync(localization.GetMessagesResource("UpdateNullEntity"));
            long SalesVATAccountID = 0;
            long PurchasesVATAccountID = 0;

            if (!string.IsNullOrEmpty(entity.SalesVatAccountCode))
            {
                var SalesVatAccount = await accRepositoryManager.AccAccountRepository
                    .GetAccountIdByCode(entity.SalesVatAccountCode, currentData.FacilityId);

                if (SalesVatAccount != null)
                {
                    SalesVATAccountID = SalesVatAccount;
                }
                else
                {
                    return await Result<SysVatGroupEditDto>.WarningAsync(
                        localization.GetAccResource("AccAccountNotfind") + entity.SalesVatAccountCode);
                }
            }

            if (!string.IsNullOrEmpty(entity.PurchasesVatAccountCode))
            {
                var PurchasesVAT = await accRepositoryManager.AccAccountRepository
                     .GetAccountIdByCode(entity.PurchasesVatAccountCode, currentData.FacilityId);


                if (PurchasesVAT != null)
                {
                    PurchasesVATAccountID = PurchasesVAT;
                }
                else
                {
                    return await Result<SysVatGroupEditDto>.WarningAsync(
                        localization.GetAccResource("AccAccountNotfind") + entity.PurchasesVatAccountCode);
                }
            }

            entity.SalesVatAccountId = SalesVATAccountID;
            entity.PurchasesVatAccountId = PurchasesVATAccountID;
            var item = await _mainRepositoryManager.SysVatGroupRepository.GetById(entity.VatId);

            if (item == null) return await Result<SysVatGroupEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));


            _mapper.Map(entity, item);

            _mainRepositoryManager.SysVatGroupRepository.Update(item);

            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysVatGroupEditDto>.SuccessAsync(_mapper.Map<SysVatGroupEditDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysVatGroupEditDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.SysVatGroupRepository.GetById(Id);
            if (item == null) return Result<SysVatGroupDto>.Fail(localization.GetMessagesResource("NoIdInUpdate"));
            item.IsDeleted = true;

            _mainRepositoryManager.SysVatGroupRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysVatGroupDto>.SuccessAsync(_mapper.Map<SysVatGroupDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysVatGroupDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.SysVatGroupRepository.GetById(Id);
            if (item == null) return Result<SysVatGroupDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");
            item.IsDeleted = true;
            _mainRepositoryManager.SysVatGroupRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysVatGroupDto>.SuccessAsync(_mapper.Map<SysVatGroupDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysVatGroupDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }
    }
}
