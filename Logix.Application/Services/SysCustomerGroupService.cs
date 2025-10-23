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
    public class SysCustomerGroupService : GenericQueryService<SysCustomerGroup, SysCustomerGroupDto, SysCustomerGroup>, ISysCustomerGroupService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;

        public SysCustomerGroupService(IQueryRepository<SysCustomerGroup> queryRepository,
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

        public async Task<IResult<SysCustomerGroupDto>> Add(SysCustomerGroupDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysCustomerGroupDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");

            try
            {
                var item = _mapper.Map<SysCustomerGroup>(entity);
                item.CreatedBy = session.UserId;
                item.CreatedOn = DateTime.Now;
                item.FacilityId = Convert.ToInt32(session.FacilityId);

                var newEntity = await _mainRepositoryManager.SysCustomerGroupRepository.AddAndReturn(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysCustomerGroupDto>(newEntity);

                return await Result<SysCustomerGroupDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {

                return await Result<SysCustomerGroupDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysCustomerGroupRepository.GetById(Id);
                if (item == null) return Result<SysCustomerGroupDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

                //check if there are any customer belong to this group
                var customers = await _mainRepositoryManager.SysCustomerRepository.GetAll(c => c.GroupId == Id && c.IsDeleted == false);
                if (customers.Any())
                    return await Result<SysCustomerGroupDto>.FailAsync("لا يمكن حذف هذا التصنيف وذلك لارتباطه بالعملاء");

                item.IsDeleted = true;
                item.ModifiedOn = DateTime.Now;
                item.ModifiedBy = session.UserId;
                _mainRepositoryManager.SysCustomerGroupRepository.Update(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysCustomerGroupDto>.SuccessAsync(_mapper.Map<SysCustomerGroupDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysCustomerGroupDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<SysCustomerGroupEditDto>> Update(SysCustomerGroupEditDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysCustomerGroupEditDto>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");

            try
            {
                var item = await _mainRepositoryManager.SysCustomerGroupRepository.GetById(entity.Id);

                if (item == null) return await Result<SysCustomerGroupEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));
                _mapper.Map(entity, item);

                item.ModifiedOn = DateTime.Now;
                item.ModifiedBy = session.UserId;
                _mainRepositoryManager.SysCustomerGroupRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysCustomerGroupEditDto>.SuccessAsync(_mapper.Map<SysCustomerGroupEditDto>(item), "item updated successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysCustomerGroupEditDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }
    }
}