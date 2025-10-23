using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Services.Main
{
    public class SysDynamicAttributeService : GenericQueryService<SysDynamicAttribute, SysDynamicAttributeDto, SysDynamicAttributesVw>, ISysDynamicAttributeService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;
        private readonly IQueryRepository<SysDynamicAttribute> myQueryRepository;

        public SysDynamicAttributeService(IQueryRepository<SysDynamicAttribute> queryRepository,
            IMapper mapper,
            IMainRepositoryManager mainRepositoryManager,
            ICurrentData session,
            ILocalizationService localization,
            IQueryRepository<SysDynamicAttribute> myQueryRepository) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.session = session;
            this.myQueryRepository = myQueryRepository;
            this.localization = localization;
        }

        public async Task<IResult<SysDynamicAttributeDto>> Add(SysDynamicAttributeDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysDynamicAttributeDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");
            try
            {
                var item = _mapper.Map<SysDynamicAttribute>(entity);
                item.CreatedBy = session.UserId;
                item.CreatedOn = DateTime.Now;

                var newEntity = await _mainRepositoryManager.SysDynamicAttributeRepository.AddAndReturn(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysDynamicAttributeDto>(newEntity);

                return await Result<SysDynamicAttributeDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysDynamicAttributeDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult<IEnumerable<SysDynamicAttributeDataTypeDto>>> GetAttributeTypes(CancellationToken cancellationToken = default)
        {
            try
            {
                var items = await myQueryRepository.GetAllFrom<SysDynamicAttributeDataType>();
                if (items == null) return await Result<IEnumerable<SysDynamicAttributeDataTypeDto>>.FailAsync($"{localization.GetResource1("NoData")}");

                var mapItems = _mapper.Map<IEnumerable<SysDynamicAttributeDataTypeDto>>(items);
                return await Result<IEnumerable<SysDynamicAttributeDataTypeDto>>.SuccessAsync(mapItems, "");
            }
            catch (Exception exc)
            {
                return await Result<IEnumerable<SysDynamicAttributeDataTypeDto>>.FailAsync($"EXP in {GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysDynamicAttributeRepository.GetOne(a => a.Id.Equals(Id));
                if (item == null) return Result<SysDynamicAttributeDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

                //item.ModifiedBy = session.UserId;
                //item.ModifiedOn = DateTime.Now;
                //item.IsDeleted = true;
                //_mainRepositoryManager.SysDynamicAttributeRepository.Update(item);

                await _mainRepositoryManager.SysDynamicAttributeRepository.RemoveUsingProcedure(Id);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysDynamicAttributeDto>.SuccessAsync(_mapper.Map<SysDynamicAttributeDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysDynamicAttributeDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<SysDynamicAttributeEditDto>> Update(SysDynamicAttributeEditDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysDynamicAttributeEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));
            try
            {
                var item = await _mainRepositoryManager.SysDynamicAttributeRepository.GetOne(a => a.DynamicAttributeId.Equals(entity.DynamicAttributeId));

                if (item == null) return await Result<SysDynamicAttributeEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

                _mapper.Map(entity, item);
                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;

                await _mainRepositoryManager.SysDynamicAttributeRepository.UpdateUsingProcedure(entity);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysDynamicAttributeEditDto>.SuccessAsync(_mapper.Map<SysDynamicAttributeEditDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysDynamicAttributeEditDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }
    }
}
