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
    public class SysPropertyService : GenericQueryService<SysProperty, SysPropertyDto, SysPropertiesVw>, ISysPropertyService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;

        public SysPropertyService(IQueryRepository<SysProperty> queryRepository,
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

        public async Task<IResult<SysPropertyDto>> Add(SysPropertyDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysPropertyDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");

            try
            {
                var item = _mapper.Map<SysProperty>(entity);

                var newEntity = await _mainRepositoryManager.SysPropertyRepository.AddAndReturn(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysPropertyDto>(newEntity);

                return await Result<SysPropertyDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysPropertyDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
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

        public async Task<IResult<SysPropertyEditDto>> Update(SysPropertyEditDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysPropertyEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

            try
            {
                var item = await _mainRepositoryManager.SysPropertyRepository.GetById(entity.Id);

                if (item == null) return await Result<SysPropertyEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

                _mapper.Map(entity, item);
                _mainRepositoryManager.SysPropertyRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysPropertyEditDto>.SuccessAsync(_mapper.Map<SysPropertyEditDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysPropertyEditDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        //public SysPropertyService(IMainRepositoryManager mainRepositoryManager, IMapper mapper)
        //{
        //    this._mainRepositoryManager = mainRepositoryManager;
        //    this._mapper = mapper;
        //}

        //public async Task<IResult<IEnumerable<SysPropertiesVw>>> GetAllVw()
        //{
        //    try
        //    {
        //        var items = await _mainRepositoryManager.SysPropertyRepository.GetAllVw();
        //        if (items == null) return await Result<IEnumerable<SysPropertiesVw>>.FailAsync("No Data Found");
        //        return await Result<IEnumerable<SysPropertiesVw>>.SuccessAsync(items, "records retrieved");
        //    }
        //    catch (Exception exp)
        //    {
        //        return Result<IEnumerable<SysPropertiesVw>>.Fail($"Exp in get all of: {this.GetType().Name}, Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")} .");
        //    }
        //}

        //public async Task<IResult<SysPropertiesVw>> GetByIdVw(long propertyId)
        //{
        //    try
        //    {
        //        var item = await _mainRepositoryManager.SysPropertyRepository.GetByIdVw(propertyId);

        //        if (item == null) return Result<SysPropertiesVw>.Fail($"--- there is no Data with this id: {propertyId}---");
        //        var newEntity = _mapper.Map<SysPropertiesVw>(item);
        //        return await Result<SysPropertiesVw>.SuccessAsync(newEntity, "");
        //        return await Result<SysPropertiesVw>.FailAsync($"No Data Items with property No: {propertyId}");
        //    }
        //    catch (Exception exp)
        //    {
        //        return await Result<SysPropertiesVw>.FailAsync($"EXP in {this.GetType().Name} , Message: {exp.Message}");
        //    }
        //}
        //public async Task<IResult<SysPropertyDto>> Add(SysPropertyDto entity, CancellationToken cancellationToken = default)
        //{
        //    if (entity == null) return await Result<SysPropertyDto>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");

        //    try
        //    {
        //        var newEntity = await _mainRepositoryManager.SysPropertyRepository.AddAndReturn(_mapper.Map<SysProperty>(entity));

        //        await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

        //        var entityMap = _mapper.Map<SysPropertyDto>(newEntity);

        //        return await Result<SysPropertyDto>.SuccessAsync(entityMap, "item added successfully");
        //    }
        //    catch (Exception exc)
        //    {

        //        return await Result<SysPropertyDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
        //    }

        //}

        //public Task<IResult> ChangeActive(SysPropertyDto entity, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<IResult<IEnumerable<SysPropertyDto>>> GetAll(Expression<Func<SysPropertyDto, bool>> expression, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        var mapExpr = _mapper.Map<Expression<Func<SysProperty, bool>>>(expression);
        //        var items = await _mainRepositoryManager.SysPropertyRepository.GetAll(mapExpr);
        //        var itemMap = _mapper.Map<IEnumerable<SysPropertyDto>>(items);

        //        return await Result<IEnumerable<SysPropertyDto>>.SuccessAsync(itemMap);
        //    }
        //    catch (Exception exp)
        //    {
        //        return await Result<IEnumerable<SysPropertyDto>>.FailAsync($"EXP in {this.GetType()} , Message: {exp.Message}");
        //    }
        //}

        //public Task<IResult<DtResult<SysPropertyDto>>> GetAll(DtRequest dtRequest, Expression<Func<SysPropertyDto, bool>> expression, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<IResult<IEnumerable<SysPropertyDto>>> GetAll(CancellationToken cancellationToken = default)
        //{
        //    var items = await _mainRepositoryManager.SysPropertyRepository.GetAll();

        //    var itemMap = _mapper.Map<IEnumerable<SysPropertyDto>>(items);
        //    if (items == null) return await Result<IEnumerable<SysPropertyDto>>.FailAsync("No Data Found");
        //    return await Result<IEnumerable<SysPropertyDto>>.SuccessAsync(itemMap, "records retrieved");
        //}

        //public Task<IResult<DtResult<SysPropertyDto>>> GetAll(DtRequest dtRequest, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}


        //public async Task<IResult<SysPropertyDto>> GetById(int Id, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        var item = await _mainRepositoryManager.SysPropertyRepository.GetById(Id);
        //        if (item == null) return Result<SysPropertyDto>.Fail($"--- there is no Data with this id: {Id}---");
        //        var newEntity = _mapper.Map<SysPropertyDto>(item);
        //        return await Result<SysPropertyDto>.SuccessAsync(newEntity, "");

        //    }
        //    catch (Exception ex)
        //    {
        //        return Result<SysPropertyDto>.Fail($"Exp in get data by Id: {ex.Message} --- {(ex.InnerException != null ? "InnerExp: " + ex.InnerException.Message : "no inner")} .");
        //    }
        //}


        //public async Task<IResult<SysPropertyDto>> GetById(long Id, CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        var item = await _mainRepositoryManager.SysPropertyRepository.GetById(Id);
        //        if (item == null) return Result<SysPropertyDto>.Fail($"--- there is no Data with this id: {Id}---");
        //        var newEntity = _mapper.Map<SysPropertyDto>(item);
        //        return await Result<SysPropertyDto>.SuccessAsync(newEntity, "");

        //    }
        //    catch (Exception ex)
        //    {
        //        return Result<SysPropertyDto>.Fail($"Exp in get data by Id: {ex.Message} --- {(ex.InnerException != null ? "InnerExp: " + ex.InnerException.Message : "no inner")} .");
        //    }
        //}


        //public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IResult<SysPropertyDto>> Update(SysPropertyDto entity, CancellationToken cancellationToken = default)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
