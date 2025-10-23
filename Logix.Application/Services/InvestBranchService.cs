using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Interfaces.IServices.WA;
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
    public class InvestBranchService : GenericQueryService<InvestBranch, InvestBranchDto, InvestBranchVw>, IInvestBranchService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;

        private readonly IMapper _mapper;
        private readonly ICurrentData _session;
        private readonly IWhatsappBusinessService waService;
        private readonly ILocalizationService localization;

        public InvestBranchService(IQueryRepository<InvestBranch> queryRepository, IMainRepositoryManager mainRepositoryManager, IMapper mapper, ICurrentData session, IWhatsappBusinessService waService, ILocalizationService localization) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;

            this._session = session;
            this.waService = waService;

            this.localization = localization;
        }

        public async Task<IResult<InvestBranchDto>> Add(InvestBranchDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<InvestBranchDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");

            try
            {
                var chkAllow = await AdditionAllowed(entity.FacilityId ?? 0);
                if (!chkAllow)
                    return await Result<InvestBranchDto>.FailAsync("لقد وصلت الى الحد الاقصى من الفروع");

                if (entity.Auto || string.IsNullOrEmpty(entity.BranchCode))
                {
                    //make branch code = max(BranchCode)+1
                    var allBranches = await _mainRepositoryManager.InvestBranchRepository.GetAll();
                    allBranches = allBranches.Where(b => b.FacilityId == entity.FacilityId);
                    var maxBranchCode = Convert.ToInt32(allBranches.Max(b => b.BranchCode)) + 1;
                    entity.BranchCode = maxBranchCode.ToString();
                }
                else
                {
                    //check if code is already exist
                    var branch = await _mainRepositoryManager.InvestBranchRepository.GetOne(b => b.BranchCode == entity.BranchCode && b.FacilityId == entity.FacilityId);
                    if (branch != null)
                        return await Result<InvestBranchDto>.FailAsync($"{localization.GetResource1("NumberExists")}");
                }
                entity.UserId = _session.UserId;

                var item = _mapper.Map<InvestBranch>(entity);
                var newEntity = await _mainRepositoryManager.InvestBranchRepository.AddAndReturn(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<InvestBranchDto>(newEntity);

                return await Result<InvestBranchDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<InvestBranchDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }

        }

        public async Task<IResult<InvestBranchDto>> Update(InvestBranchDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<InvestBranchDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");

            var item = await _mainRepositoryManager.InvestBranchRepository.GetById(entity.BranchId);

            if (item == null) return await Result<InvestBranchDto>.FailAsync($"--- there is no Data with this id: {entity.BranchId}---");

            if (item.FacilityId != entity.FacilityId)
            {
                var chkAllow = await AdditionAllowed(entity.FacilityId ?? 0);
                if (!chkAllow)
                    return await Result<InvestBranchDto>.FailAsync("لقد وصلت الى الحد الاقصى من الفروع");
            }

            if (string.IsNullOrEmpty(entity.BranchCode))
            {
                entity.BranchCode = item.BranchCode;
            }
            else
            {
                var branch = await _mainRepositoryManager.InvestBranchRepository.GetOne(b => b.BranchCode == entity.BranchCode && b.BranchId != entity.BranchId && b.FacilityId == entity.FacilityId);
                if (branch != null)
                    return await Result<InvestBranchDto>.FailAsync($"{localization.GetResource1("NumberExists")}");
            }
            entity.UserId = _session.UserId;

            _mapper.Map(entity, item);

            _mainRepositoryManager.InvestBranchRepository.Update(item);

            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<InvestBranchDto>.SuccessAsync(_mapper.Map<InvestBranchDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
                return await Result<InvestBranchDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.InvestBranchRepository.GetById(Id);
            if (item == null) return Result<InvestBranchDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");
            item.Isdel = true;
            //item.ModifiedOn = DateTime.Now;
            item.UserId = _session.UserId;
            _mainRepositoryManager.InvestBranchRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<InvestBranchDto>.SuccessAsync(_mapper.Map<InvestBranchDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<InvestBranchDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.InvestBranchRepository.GetById(Id);
            if (item == null) return Result<InvestBranchDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");
            item.Isdel = true;
            //item.ModifiedOn = DateTime.Now;
            item.UserId = _session.UserId;
            _mainRepositoryManager.InvestBranchRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<InvestBranchDto>.SuccessAsync(_mapper.Map<InvestBranchDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<InvestBranchDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }


        private async Task<bool> AdditionAllowed(long Facility_ID)
        {
            try
            {
                var chkExist = await _mainRepositoryManager.SysPackagesPropertyValueRepository.Property_exists(Facility_ID, 2);
                if (!chkExist)
                    return true;
                else if (await GetCountBRANCH(Facility_ID) < await _mainRepositoryManager.SysPackagesPropertyValueRepository.Get_Property_Values(Facility_ID, 2))
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        private async Task<long> GetCountBRANCH(long Facility_ID)
        {
            try
            {
                var getBranches = await _mainRepositoryManager.InvestBranchRepository.GetAll(x => x.FacilityId, x => x.Isdel == false && x.FacilityId == Facility_ID);
                return getBranches.Count();
            }
            catch
            {
                return 0;
            }
        }
    }
}
