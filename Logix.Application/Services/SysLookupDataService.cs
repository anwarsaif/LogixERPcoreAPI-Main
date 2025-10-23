using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using System.Linq.Expressions;

namespace Logix.Application.Services.Main
{
    public class SysLookupDataService : GenericQueryService<SysLookupData, SysLookupDataDto, SysLookupDataVw>, ISysLookupDataService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IAccRepositoryManager _accRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;

        public SysLookupDataService(IQueryRepository<SysLookupData> queryRepository,
            IMainRepositoryManager mainRepositoryManager,
            IAccRepositoryManager accRepositoryManager,
            IMapper mapper,
            ICurrentData session,
            ILocalizationService localization) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._accRepositoryManager = accRepositoryManager;
            this._mapper = mapper;
            this.session = session;
            this.localization = localization;
        }

        public async Task<IResult<SysLookupDataDto>> Add(SysLookupDataDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysLookupDataDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");

            try
            {
                if (!string.IsNullOrEmpty(entity.AccountCode))
                {
                    var account = await _accRepositoryManager.AccAccountRepository.GetOne(a => a.AccAccountCode == entity.AccountCode && a.FacilityId == session.FacilityId && a.IsDeleted == false);
                    if (account == null)
                        return await Result<SysLookupDataDto>.FailAsync($"{localization.GetResource1("AccountNotExsists")}");
                    entity.AccAccountId = account.AccAccountId;
                }

                if (!string.IsNullOrEmpty(entity.CostCenterCode))
                {
                    var costCenter = await _accRepositoryManager.AccCostCenterRepository.GetOne(c => c.CostCenterCode == entity.CostCenterCode && c.FacilityId == session.FacilityId && c.IsDeleted == false);
                    if (costCenter == null)
                        return await Result<SysLookupDataDto>.FailAsync($"{localization.GetResource1("CostCenterNotExsists")}");
                    entity.CcId = costCenter.CcId;
                }

                var allCodes = await _mainRepositoryManager.SysLookupDataRepository.GetAll(c => c.CatagoriesId == entity.CatagoriesId, c => c.Code);
                long maxCode = allCodes.Max() ?? 0;

                entity.Code = maxCode + 1;
                entity.SortNo = Convert.ToInt32(maxCode + 1);
                entity.UserId = session.UserId;

                var newEntity = await _mainRepositoryManager.SysLookupDataRepository.AddAndReturn(_mapper.Map<SysLookupData>(entity));

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysLookupDataDto>(newEntity);

                return await Result<SysLookupDataDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {

                return await Result<SysLookupDataDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }

        }

        public async Task<IResult<SysLookupDataDto>> Update(SysLookupDataDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysLookupDataDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

            try
            {
                if (!string.IsNullOrEmpty(entity.AccountCode))
                {
                    var account = await _accRepositoryManager.AccAccountRepository.GetOne(a => a.AccAccountCode == entity.AccountCode && a.FacilityId == session.FacilityId && a.IsDeleted == false);
                    if (account == null)
                        return await Result<SysLookupDataDto>.FailAsync($"{localization.GetResource1("AccountNotExsists")}");
                    entity.AccAccountId = account.AccAccountId;
                }
                else { entity.AccAccountId = 0; }

                if (!string.IsNullOrEmpty(entity.CostCenterCode))
                {
                    var costCenter = await _accRepositoryManager.AccCostCenterRepository.GetOne(c => c.CostCenterCode == entity.CostCenterCode && c.FacilityId == session.FacilityId && c.IsDeleted == false);
                    if (costCenter == null)
                        return await Result<SysLookupDataDto>.FailAsync($"{localization.GetResource1("CostCenterNotExsists")}");
                    entity.CcId = costCenter.CcId;
                }
                else { entity.CcId = 0; }
                entity.UserId = session.UserId;

                var item = await _mainRepositoryManager.SysLookupDataRepository.GetById(entity.Id);

                if (item == null) return await Result<SysLookupDataDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");

                _mapper.Map(entity, item);

                _mainRepositoryManager.SysLookupDataRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysLookupDataDto>.SuccessAsync(_mapper.Map<SysLookupDataDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                Console.WriteLine($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
                return await Result<SysLookupDataDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<IEnumerable<DDItem>>> GetDataByCategory(int categoryId, int lang = 0)
        {
            lang = lang == 0 ? session.Language : lang;
            try
            {
                var allItems = await _mainRepositoryManager.SysLookupDataRepository.GetDataByCategory(categoryId);
                allItems = allItems.Where(d => d.Isdel == false).OrderBy(d => d.SortNo).ToList();
                if (allItems != null && allItems.Any())
                {
                    var ddItems = new List<DDItem>();
                    foreach (var item in allItems)
                    {
                        var di = new DDItem { Code = item.Code, Id = item.Id, ColorId = item.ColorId, Icon = item.Icon };
                        switch (lang)
                        {
                            case 1: di.Name = item.Name; break;
                            case 2: di.Name = item.Name2 ?? item.Name; break;
                            default: di.Name = item.Name; break;
                        }
                        ddItems.Add(di);
                    }

                    return await Result<IEnumerable<DDItem>>.SuccessAsync(ddItems);
                }

                return await Result<IEnumerable<DDItem>>.FailAsync($"No Data Items with category No: {categoryId}");
            }
            catch (Exception exp)
            {
                return await Result<IEnumerable<DDItem>>.FailAsync($"EXP in {this.GetType()} , Message: {exp.Message}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.SysLookupDataRepository.GetById(Id);
            if (item == null) return Result<SysLookupDataDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");
            item.Isdel = true;

            _mainRepositoryManager.SysLookupDataRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysLookupDataDto>.SuccessAsync(_mapper.Map<SysLookupDataDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysLookupDataDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.SysLookupDataRepository.GetById(Id);
            if (item == null) return Result<SysLookupDataDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");
            item.Isdel = true;

            _mainRepositoryManager.SysLookupDataRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysLookupDataDto>.SuccessAsync(_mapper.Map<SysLookupDataDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysLookupDataDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

    }
}
