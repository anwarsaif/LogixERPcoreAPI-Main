using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using System.Linq.Expressions;

namespace Logix.Application.Services.Main
{
    public class SysScreenWorkflowService : GenericQueryService<SysScreenWorkflow, SysScreenWorkflowDto, SysScreenWorkflow>, ISysScreenWorkflowService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;


        public SysScreenWorkflowService(IQueryRepository<SysScreenWorkflow> queryRepository, IMainRepositoryManager mainRepositoryManager, IMapper mapper,
            ICurrentData session, ILocalizationService localization) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.session = session;
            this.localization = localization;
        }

        public async Task<IResult<SysScreenWorkflowDto>> Add(SysScreenWorkflowDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysScreenWorkflowDto>.FailAsync($"{localization.GetResource1("AddNullEntity")}");

            try
            {
                entity.CreatedBy = session.UserId;
                entity.CreatedOn = DateTime.Now;
                var item = _mapper.Map<SysScreenWorkflow>(entity);
                var newEntity = await _mainRepositoryManager.SysScreenWorkflowRepository.AddAndReturn(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysScreenWorkflowDto>(newEntity);

                return await Result<SysScreenWorkflowDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysScreenWorkflowDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult<SysScreenWorkflowDto>> Update(SysScreenWorkflowDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysScreenWorkflowDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");

            var item = await _mainRepositoryManager.SysScreenWorkflowRepository.GetById(entity.Id);

            if (item == null) return await Result<SysScreenWorkflowDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");

            item.ModifiedBy = session.UserId;
            item.ModifiedOn = DateTime.Now;

            _mapper.Map(entity, item);

            _mainRepositoryManager.SysScreenWorkflowRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysScreenWorkflowDto>.SuccessAsync(_mapper.Map<SysScreenWorkflowDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
                return await Result<SysScreenWorkflowDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.SysScreenWorkflowRepository.GetById(Id);
            if (item == null) return Result<SysScreenWorkflowDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

            item.IsDeleted = true;
            item.ModifiedOn = DateTime.Now;
            item.ModifiedBy = session.UserId;

            _mainRepositoryManager.SysScreenWorkflowRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysScreenWorkflowDto>.SuccessAsync(_mapper.Map<SysScreenWorkflowDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysScreenWorkflowDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.SysScreenWorkflowRepository.GetById(Id);
            if (item == null) return Result<SysScreenWorkflowDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

            item.IsDeleted = true;
            item.ModifiedOn = DateTime.Now;
            item.ModifiedBy = session.UserId;

            _mainRepositoryManager.SysScreenWorkflowRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysScreenWorkflowDto>.SuccessAsync(_mapper.Map<SysScreenWorkflowDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysScreenWorkflowDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }


        public Task<IResult<IEnumerable<SysScreenWorkflow>>> GetAllVW(Expression<Func<SysScreenWorkflow, bool>> expression, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<SysScreenWorkflow>> GetOneVW(Expression<Func<SysScreenWorkflow, bool>> expression, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public async Task<IResult<IEnumerable<SysScreenWorkflowDto>>> GetScreenWorkflowByScreen(long screenId)
        {
            try
            {
                var items = await _mainRepositoryManager.SysScreenWorkflowRepository.GetScreenWorkflowByScreen(screenId);
                if (items == null)
                    return await Result<IEnumerable<SysScreenWorkflowDto>>.FailAsync($"ERROR in GetScreenWorkflowByScreen, Message: No Data Found");
                return await Result<IEnumerable<SysScreenWorkflowDto>>.SuccessAsync(items);
            }
            catch (Exception exp)
            {

                return await Result<IEnumerable<SysScreenWorkflowDto>>.FailAsync($"EXP in GetScreenWorkflowByScreen at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<IEnumerable<DynamicAttributeDto>>> GetAttributes(long screenId, long? appTypeId, int? stepId = null)
        {
            try
            {
                var items = await _mainRepositoryManager.SysScreenWorkflowRepository.GetAttributes(screenId, appTypeId, stepId);
                if (items == null)
                    return await Result<IEnumerable<DynamicAttributeDto>>.FailAsync($"ERROR in GetAttriutes, Message: No Data Found");
                return await Result<IEnumerable<DynamicAttributeDto>>.SuccessAsync(items);
            }
            catch (Exception exp)
            {

                return await Result<IEnumerable<DynamicAttributeDto>>.FailAsync($"EXP in GetAttriutes at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<IEnumerable<DynamicAttributeValueDto>>> GetAttributeValues(long screenId, long appId, long? appTypeId = null)
        {
            try
            {
                var items = await _mainRepositoryManager.SysScreenWorkflowRepository.GetAttributeValues(screenId, appId, appTypeId);
                if (items == null)
                    return await Result<IEnumerable<DynamicAttributeValueDto>>.FailAsync($"ERROR in GetAttriuteValues, Message: No Data Found");
                return await Result<IEnumerable<DynamicAttributeValueDto>>.SuccessAsync(items);
            }
            catch (Exception exp)
            {

                return await Result<IEnumerable<DynamicAttributeValueDto>>.FailAsync($"EXP in GetAttriuteValues at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<string?>> GetDefaultValue(string defaultValue, long? empId, string currDate, long? facilityId = 1, long? appId = null, long? finYear = null)
        {
            try
            {
                var item = await _mainRepositoryManager.SysScreenWorkflowRepository.GetDefaultValue(defaultValue, empId, currDate, facilityId, appId, finYear);
                if (string.IsNullOrEmpty(item))
                    return await Result<string>.FailAsync("No Default Value");
                return await Result<string>.SuccessAsync(item);
            }
            catch (Exception exp)
            {

                return await Result<string>.FailAsync($"EXP in GetDefaultValue at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

    }      
}
