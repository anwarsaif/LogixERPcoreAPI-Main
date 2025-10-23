using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Services.Main
{
    public class SysScreenPermissionPropertyService : GenericQueryService<SysScreenPermissionProperty, SysScreenPermissionPropertyDto, SysScreenPermissionProperty>, ISysScreenPermissionPropertyService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;

        public SysScreenPermissionPropertyService(IQueryRepository<SysScreenPermissionProperty> queryRepository,
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

        public async Task<IResult<SysScreenPermissionPropertyDto>> Add(SysScreenPermissionPropertyDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysScreenPermissionPropertyDto>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");

            try
            {
                var newEntity = await _mainRepositoryManager.SysScreenPermissionPropertyRepository.AddAndReturn(_mapper.Map<SysScreenPermissionProperty>(entity));

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysScreenPermissionPropertyDto>(newEntity);

                return await Result<SysScreenPermissionPropertyDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {

                return await Result<SysScreenPermissionPropertyDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }

        }

        public async Task<IResult<SysScreenPermissionPropertyDto>> GetByProperty(long propertyId, long facilityId)
        {
            try
            {
                var item = await _mainRepositoryManager.SysScreenPermissionPropertyRepository.GetByProperty(propertyId, facilityId);

                if (item == null) return Result<SysScreenPermissionPropertyDto>.Fail($"--- there is no Data with this id: {propertyId}---");
                var newEntity = _mapper.Map<SysScreenPermissionPropertyDto>(item);
                return await Result<SysScreenPermissionPropertyDto>.SuccessAsync(newEntity, "");
            }
            catch (Exception exp)
            {
                return await Result<SysScreenPermissionPropertyDto>.FailAsync($"EXP in {this.GetType()} , Message: {exp.Message}");
            }
        }

        public async Task<IResult<SysScreenPermissionPropertiesVw>> GetByPropertyVw(long propertyId, long userId)
        {
            try
            {
                var item = await _mainRepositoryManager.SysScreenPermissionPropertyRepository.GetByPropertyVw(propertyId, userId);

                if (item == null) return Result<SysScreenPermissionPropertiesVw>.Fail($"--- there is no Data with this id: {propertyId}---");

                return await Result<SysScreenPermissionPropertiesVw>.SuccessAsync(item, "");
            }
            catch (Exception exp)
            {
                return await Result<SysScreenPermissionPropertiesVw>.FailAsync($"EXP in {this.GetType()} , Message: {exp.Message}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysScreenPermissionPropertyRepository.GetById(Id);
                if (item == null) return Result<SysScreenPermissionPropertyDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

                item.IsDeleted = true;
                _mainRepositoryManager.SysScreenPermissionPropertyRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysScreenPermissionPropertyDto>.SuccessAsync(_mapper.Map<SysScreenPermissionPropertyDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysScreenPermissionPropertyDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<SysPropertyVM>> Update(SysPropertyVM entity, CancellationToken cancellationToken = default)
        {
            try
            {
                if (entity.PermissionId > 0)
                {
                    var item = await _mainRepositoryManager.SysScreenPermissionPropertyRepository.GetById(entity.PermissionId);
                    if (item != null)
                    {
                        item.Allow = entity.Allow;
                        item.Value = entity.Value;

                        _mainRepositoryManager.SysScreenPermissionPropertyRepository.Update(item);
                        await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                    }
                }
                else
                {
                    SysScreenPermissionProperty newItem = new()
                    {
                        PropertyId = entity.PropertyId,
                        UserId = entity.UserId,
                        Allow = entity.Allow,
                        Value = entity.Value
                    };

                    var newEntity = await _mainRepositoryManager.SysScreenPermissionPropertyRepository.AddAndReturn(newItem);
                    await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                    entity.PermissionId = newEntity.Id;
                }

                return await Result<SysPropertyVM>.SuccessAsync(entity);
            }
            catch (Exception exp)
            {
                return await Result<SysPropertyVM>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }
    }
}
