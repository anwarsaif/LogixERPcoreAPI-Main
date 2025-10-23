using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Services.Main
{
    public class SysPropertyValueService : GenericQueryService<SysPropertyValue, SysPropertyValueDto, SysPropertyValuesVw>, ISysPropertyValueService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;

        public SysPropertyValueService(IQueryRepository<SysPropertyValue> queryRepository,
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

        public async Task<IResult<SysPropertyValueDto>> Add(SysPropertyValueDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysPropertyValueDto>.FailAsync(localization.GetMessagesResource("NoIdInAdd"));

            try
            {
                entity.FacilityId = session.FacilityId;
                var item = _mapper.Map<SysPropertyValue>(entity);
                var newEntity = await _mainRepositoryManager.SysPropertyValueRepository.AddAndReturn(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysPropertyValueDto>.SuccessAsync(_mapper.Map<SysPropertyValueDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysPropertyValueDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }


        public Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<SysPropertyValueDto>> Update(SysPropertyValueDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysPropertyValueDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

            try
            {
                var item = await _mainRepositoryManager.SysPropertyValueRepository.GetById(entity.Id);

                if (item == null) return await Result<SysPropertyValueDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

                _mapper.Map(entity, item);

                item.FacilityId = session.FacilityId;
                _mainRepositoryManager.SysPropertyValueRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysPropertyValueDto>.SuccessAsync(_mapper.Map<SysPropertyValueDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysPropertyValueDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> UpdatePropertyValue(long Id, string Value, CancellationToken cancellationToken = default)
        {
            if (Id == 0) return await Result<SysPropertyValueDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));
            try
            {
                var item = await _mainRepositoryManager.SysPropertyValueRepository.GetById(Id);
                if (item == null) return await Result<SysPropertyValueDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

                item.PropertyValue = Value;
                _mainRepositoryManager.SysPropertyValueRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result.SuccessAsync(localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<SysPropertyValueDto>> GetByProperty(long propertyId, long facilityId)
        {
            try
            {
                var item = await _mainRepositoryManager.SysPropertyValueRepository.GetByProperty(propertyId, facilityId);

                if (item == null) return Result<SysPropertyValueDto>.Fail($"--- there is no Data with this id: {propertyId}---");
                var newEntity = _mapper.Map<SysPropertyValueDto>(item);
                return await Result<SysPropertyValueDto>.SuccessAsync(newEntity, "");
                //return await Result<SysPropertyValueDto>.FailAsync($"No Data Items with property No: {propertyId}");
            }
            catch (Exception exp)
            {
                return await Result<SysPropertyValueDto>.FailAsync($"EXP in {this.GetType()} , Message: {exp.Message}");
            }
        }

        public async Task<IResult<SysPropertyValuesVw>> GetByPropertyVw(long propertyId, long facilityId)
        {
            try
            {
                var item = await _mainRepositoryManager.SysPropertyValueRepository.GetByPropertyVw(propertyId, facilityId);

                if (item == null) return Result<SysPropertyValuesVw>.Fail($"--- there is no Data with this id: {propertyId}---");

                return await Result<SysPropertyValuesVw>.SuccessAsync(item, "");
                // return await Result<SysPropertyValuesVw>.FailAsync($"No Data Items with property No: {propertyId}");
            }
            catch (Exception exp)
            {
                return await Result<SysPropertyValuesVw>.FailAsync($"EXP in {this.GetType()} , Message: {exp.Message}");
            }
        }

        public async Task<IResult<IEnumerable<SysPropertyValuesVw>>> GetAllVw()
        {
            try
            {
                var items = await _mainRepositoryManager.SysPropertyValueRepository.GetAllVw();
                if (items == null) return await Result<IEnumerable<SysPropertyValuesVw>>.FailAsync("No Data Found");
                return await Result<IEnumerable<SysPropertyValuesVw>>.SuccessAsync(items, "records retrieved");
            }
            catch (Exception exp)
            {
                return Result<IEnumerable<SysPropertyValuesVw>>.Fail($"Exp in get all of: {this.GetType}, Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")} .");
            }
        }

        public async Task<IResult> SetPropertyValue(long PropertyId, long FacilityId, string Value, CancellationToken cancellationToken = default)
        {
            if (PropertyId == 0) return await Result.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));
            try
            {
                var item = await _mainRepositoryManager.SysPropertyValueRepository.GetOne(x => x.PropertyId == PropertyId && x.FacilityId == FacilityId);
                if (item == null)
                {
                    var newItem = new SysPropertyValue
                    {
                        FacilityId = FacilityId,
                        PropertyId = PropertyId,
                        PropertyValue = Value
                    };
                    var newEntity = await _mainRepositoryManager.SysPropertyValueRepository.AddAndReturn(newItem);
                    await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                }
                else
                {
                    item.PropertyValue = Value;
                    _mainRepositoryManager.SysPropertyValueRepository.Update(item);
                    await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                }

                return await Result.SuccessAsync(localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result.FailAsync($"EXP, ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<List<SysPropertyValueFilterDto>>> Search(SysPropertyValueFilterDto filter, CancellationToken cancellationToken = default)
        {
            try
            {
                filter.PropertyId ??= 0; filter.SystemId ??= 0; filter.ClassificationsId ??= 0;
                long facilityId = session.FacilityId;

                // to get only properties of systems that the customer has
                var allSystems = await _mainRepositoryManager.SysSystemRepository.GetAll(s => s.SystemId, s => s.Isdel == false);
                if (allSystems != null)
                {
                    var properties = await _mainRepositoryManager.SysPropertyRepository.GetAllVw(p => allSystems.Contains(p.SystemId ?? 0)
                    && (filter.PropertyId == 0 || (p.Id == filter.PropertyId))
                    && (string.IsNullOrEmpty(filter.PropertyName) || (!string.IsNullOrEmpty(p.PropertyName) && p.PropertyName.Contains(filter.PropertyName)))
                    && (filter.SystemId == 0 || (p.SystemId == filter.SystemId))
                    && (filter.ClassificationsId == 0 || (p.ClassificationsId == filter.ClassificationsId))
                    && (filter.IsRequired == false || (p.IsRequired == filter.IsRequired))
                    );

                    if (properties != null)
                    {
                        List<SysPropertyValueFilterDto> final = new();

                        foreach (var item in properties)
                        {
                            long valueId = 0; string propertyValue = "";
                            var getValue = await _mainRepositoryManager.SysPropertyValueRepository.GetOne(x => x.PropertyId == item.Id && x.FacilityId == facilityId);
                            if (getValue != null && getValue.Id > 0)
                            {
                                valueId = getValue.Id;
                                propertyValue = getValue.PropertyValue ?? "";
                            }

                            final.Add(new SysPropertyValueFilterDto
                            {
                                Id = valueId,
                                PropertyCode = item.PropertyCode,
                                PropertyId = item.Id,
                                PropertyValue = propertyValue,
                                PropertyName = item.PropertyName,
                                SystemId = item.SystemId,
                                SystemName = item.SystemName,
                                SystemName2 = item.SystemName2,
                                Description = item.Description,
                            });
                        }

                        if (filter.IsEmptyValue)
                        {
                            final = final.Where(x => string.IsNullOrEmpty(x.PropertyValue)).ToList();
                        }

                        return await Result<List<SysPropertyValueFilterDto>>.SuccessAsync(final);
                    }
                    return await Result<List<SysPropertyValueFilterDto>>.SuccessAsync(new List<SysPropertyValueFilterDto>());
                }
                return await Result<List<SysPropertyValueFilterDto>>.SuccessAsync(new List<SysPropertyValueFilterDto>());

            }
            catch (Exception ex)
            {
                return await Result<List<SysPropertyValueFilterDto>>.FailAsync($"====== Exp, MESSAGE: {ex.Message}");
            }
        }
    }
}