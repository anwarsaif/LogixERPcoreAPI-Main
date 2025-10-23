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
    public class SysLookupCategoryService : GenericQueryService<SysLookupCategory, SysLookupCategoryDto, SysLookupCategory>, ISysLookupCategoryService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;

        public SysLookupCategoryService(IQueryRepository<SysLookupCategory> queryRepository,
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

        public async Task<IResult<SysLookupCategoryDto>> Add(SysLookupCategoryDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysLookupCategoryDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");

            try
            {
                if (entity.CatagoriesId <= 0)
                    return await Result<SysLookupCategoryDto>.FailAsync($"يجب اخال رقم صحيح للتصنيف");

                var idExist = await _mainRepositoryManager.SysLookupCategoryRepository.GetOne(c => c.CatagoriesId == entity.CatagoriesId);
                if (idExist != null)
                    return await Result<SysLookupCategoryDto>.FailAsync($"رقم التصنيف موجود مسبقا");

                entity.UserId = session.UserId;

                var newEntity = await _mainRepositoryManager.SysLookupCategoryRepository.AddAndReturn(_mapper.Map<SysLookupCategory>(entity));

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysLookupCategoryDto>(newEntity);

                return await Result<SysLookupCategoryDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysLookupCategoryDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.SysLookupCategoryRepository.GetById(Id);
            if (item == null) return Result<SysLookupCategoryDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

            if (item.IsDeletable == false)
                return Result<SysLookupCategoryDto>.Fail("لا يمكنك حذف هذا العنصر");

            item.Isdel = true;

            _mainRepositoryManager.SysLookupCategoryRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysLookupCategoryDto>.SuccessAsync(_mapper.Map<SysLookupCategoryDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysLookupCategoryDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.SysLookupCategoryRepository.GetById(Id);
            if (item == null) return Result<SysLookupCategoryDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

            if (item.IsDeletable == false)
                return Result<SysLookupCategoryDto>.Fail("لا يمكنك حذف هذا العنصر");

            item.Isdel = true;

            _mainRepositoryManager.SysLookupCategoryRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysLookupCategoryDto>.SuccessAsync(_mapper.Map<SysLookupCategoryDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysLookupCategoryDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<SysLookupCategoryEditDto>> Update(SysLookupCategoryEditDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysLookupCategoryEditDto>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");

            try
            {
                var item = await _mainRepositoryManager.SysLookupCategoryRepository.GetById(entity.CatagoriesId);
                if (item == null) return await Result<SysLookupCategoryEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));
                _mapper.Map(entity, item);
                _mainRepositoryManager.SysLookupCategoryRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysLookupCategoryEditDto>.SuccessAsync(_mapper.Map<SysLookupCategoryEditDto>(item), "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysLookupCategoryEditDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

    }

    ////previous way
    //public class SysLookupCategoryService : GenericQueryService<SysLookupCategory, SysLookupCategoryDto, SysLookupCategory>, ISysLookupCategoryService
    //{
    //    private readonly IMainRepositoryManager _mainRepositoryManager;
    //    private readonly IMapper _mapper;
    //    private readonly ICurrentData session;
    //    private readonly ILocalizationService localization;

    //    public SysLookupCategoryService(IMainRepositoryManager mainRepositoryManager,
    //        IMapper mapper, 
    //        ICurrentData session,
    //        ILocalizationService localization)
    //    {
    //        this._mainRepositoryManager = mainRepositoryManager;
    //        this._mapper = mapper;
    //        this.session = session;
    //        this.localization = localization;
    //    }
    //    public async Task<IResult<SysLookupCategoryDto>> Add(SysLookupCategoryDto entity, CancellationToken cancellationToken = default)
    //    {
    //        if (entity == null) return await Result<SysLookupCategoryDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");

    //        try
    //        {
    //            entity.UserId = session.UserId;

    //            var newEntity = await _mainRepositoryManager.SysLookupCategoryRepository.AddAndReturn(_mapper.Map<SysLookupCategory>(entity));

    //            await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

    //            var entityMap = _mapper.Map<SysLookupCategoryDto>(newEntity);

    //            return await Result<SysLookupCategoryDto>.SuccessAsync(entityMap, "item added successfully");
    //        }
    //        catch (Exception exc)
    //        {

    //            return await Result<SysLookupCategoryDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
    //        }

    //    }

    //    public Task<IResult> ChangeActive(SysLookupCategoryDto entity, CancellationToken cancellationToken = default)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async Task<IResult<IEnumerable<SysLookupCategoryDto>>> GetAll(Expression<Func<SysLookupCategoryDto, bool>> expression, CancellationToken cancellationToken = default)
    //    {
    //        try
    //        {
    //            var mapExpr = _mapper.Map<Expression<Func<SysLookupCategory, bool>>>(expression);
    //            var items = await _mainRepositoryManager.SysLookupCategoryRepository.GetAll(mapExpr);
    //            var itemMap = _mapper.Map<IEnumerable<SysLookupCategoryDto>>(items);

    //            return await Result<IEnumerable<SysLookupCategoryDto>>.SuccessAsync(itemMap);
    //        }
    //        catch (Exception exp)
    //        {
    //            return await Result<IEnumerable<SysLookupCategoryDto>>.FailAsync($"EXP in {this.GetType()} , Message: {exp.Message}");
    //        }
    //    }

    //    public Task<IResult<DtResult<SysLookupCategoryDto>>> GetAll(DtRequest dtRequest, Expression<Func<SysLookupCategoryDto, bool>> expression, CancellationToken cancellationToken = default)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async Task<IResult<IEnumerable<SysLookupCategoryDto>>> GetAll(CancellationToken cancellationToken = default)
    //    {
    //        var items = await _mainRepositoryManager.SysLookupCategoryRepository.GetAll();

    //        var itemMap = _mapper.Map<IEnumerable<SysLookupCategoryDto>>(items);
    //        if (items == null) return await Result<IEnumerable<SysLookupCategoryDto>>.FailAsync($"{localization.GetResource1("NoData")}");
    //        return await Result<IEnumerable<SysLookupCategoryDto>>.SuccessAsync(itemMap, "records retrieved");
    //    }

    //    public Task<IResult<DtResult<SysLookupCategoryDto>>> GetAll(DtRequest dtRequest, CancellationToken cancellationToken = default)
    //    {
    //        throw new NotImplementedException();
    //    }


    //    public async Task<IResult<SysLookupCategoryDto>> GetById(int Id, CancellationToken cancellationToken = default)
    //    {
    //        try
    //        {
    //            var item = await _mainRepositoryManager.SysLookupCategoryRepository.GetById(Id);
    //            if (item == null) return Result<SysLookupCategoryDto>.Fail($"--- there is no Data with this id: {Id}---");
    //            var newEntity = _mapper.Map<SysLookupCategoryDto>(item);
    //            return await Result<SysLookupCategoryDto>.SuccessAsync(newEntity, "");

    //        }
    //        catch (Exception ex)
    //        {
    //            return Result<SysLookupCategoryDto>.Fail($"Exp in get data by Id: {ex.Message} --- {(ex.InnerException != null ? "InnerExp: " + ex.InnerException.Message : "no inner")} .");
    //        }
    //    }
    //    public async Task<IResult<SysLookupCategoryEditDto>> GetForUpdate(int Id, CancellationToken cancellationToken = default)
    //    {
    //        try
    //        {
    //            var item = await _mainRepositoryManager.SysLookupCategoryRepository.GetById(Id);
    //            if (item == null) return Result<SysLookupCategoryEditDto>.Fail($"--- there is no Data with this id: {Id}---");
    //            var newEntity = _mapper.Map<SysLookupCategoryEditDto>(item);
    //            return await Result<SysLookupCategoryEditDto>.SuccessAsync(newEntity, "");

    //        }
    //        catch (Exception ex)
    //        {
    //            return Result<SysLookupCategoryEditDto>.Fail($"Exp in get data by Id: {ex.Message} --- {(ex.InnerException != null ? "InnerExp: " + ex.InnerException.Message : "no inner")} .");
    //        }
    //    }

    //    public async Task<IResult<SysLookupCategoryDto>> GetById(long Id, CancellationToken cancellationToken = default)
    //    {
    //        try
    //        {
    //            var item = await _mainRepositoryManager.SysLookupCategoryRepository.GetById(Id);
    //            if (item == null) return Result<SysLookupCategoryDto>.Fail($"--- there is no Data with this id: {Id}---");
    //            var newEntity = _mapper.Map<SysLookupCategoryDto>(item);
    //            return await Result<SysLookupCategoryDto>.SuccessAsync(newEntity, "");

    //        }
    //        catch (Exception ex)
    //        {
    //            return Result<SysLookupCategoryDto>.Fail($"Exp in get data by Id: {ex.Message} --- {(ex.InnerException != null ? "InnerExp: " + ex.InnerException.Message : "no inner")} .");
    //        }
    //    }

    //    public async Task<IResult<SysLookupCategoryEditDto>> Update(SysLookupCategoryEditDto entity, CancellationToken cancellationToken = default)
    //    {
    //        if (entity == null) return await Result<SysLookupCategoryEditDto>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");

    //        try
    //        {
    //            var item = await _mainRepositoryManager.SysLookupCategoryRepository.GetById(entity.CatagoriesId);
    //            _mapper.Map(entity, item);
    //            _mainRepositoryManager.SysLookupCategoryRepository.Update(item);
    //            await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);



    //            return await Result<SysLookupCategoryEditDto>.SuccessAsync(_mapper.Map<SysLookupCategoryEditDto>(item), "item added successfully");
    //        }
    //        catch (Exception exc)
    //        {

    //            return await Result<SysLookupCategoryEditDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
    //        }
    //    }
    //    public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
    //    {
    //        var item = await _mainRepositoryManager.SysLookupCategoryRepository.GetById(Id);
    //        if (item == null) return Result<SysLookupCategoryDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

    //        if (item.IsDeletable == false)
    //            return Result<SysLookupCategoryDto>.Fail("لا يمكنك حذف هذا العنصر");

    //        item.Isdel = true;

    //        _mainRepositoryManager.SysLookupCategoryRepository.Update(item);
    //        try
    //        {
    //            await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

    //            return await Result<SysLookupCategoryDto>.SuccessAsync(_mapper.Map<SysLookupCategoryDto>(item), " record removed");
    //        }
    //        catch (Exception exp)
    //        {
    //            return await Result<SysLookupCategoryDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
    //        }
    //    }

    //    public Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<IResult<SysLookupCategoryDto>> Update(SysLookupCategoryDto entity, CancellationToken cancellationToken = default)
    //    {
    //        throw new NotImplementedException();
    //    }


    //}

}
