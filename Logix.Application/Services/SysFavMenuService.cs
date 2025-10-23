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
    public class SysFavMenuService : GenericQueryService<SysFavMenu, SysFavMenuDto>, ISysFavMenuService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;

        public SysFavMenuService(IQueryRepository<SysFavMenu> queryRepository,
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

        public async Task<IResult<SysFavMenuDto>> Add(SysFavMenuDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysFavMenuDto>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");
            try
            {
                entity.UserId = session.UserId;
                var newEntity = await _mainRepositoryManager.SysFavMenuRepository.AddAndReturn(_mapper.Map<SysFavMenu>(entity));
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysFavMenuDto>(newEntity);
                return await Result<SysFavMenuDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysFavMenuDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysFavMenuRepository.GetById(Id);
                if (item == null) return Result<SysFavMenuDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

                _mainRepositoryManager.SysFavMenuRepository.Remove(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysFavMenuDto>.SuccessAsync(_mapper.Map<SysFavMenuDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysFavMenuDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<SysFavMenuDto>> Update(SysFavMenuDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysFavMenuDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));
            try
            {
                var item = await _mainRepositoryManager.SysFavMenuRepository.GetById(entity.Id);

                if (item == null) return await Result<SysFavMenuDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

                entity.Url = item.Url;
                entity.UserId = item.UserId;

                _mapper.Map(entity, item);
                _mainRepositoryManager.SysFavMenuRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysFavMenuDto>.SuccessAsync(_mapper.Map<SysFavMenuDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysFavMenuDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }
    }

    //public class SysFavMenuService : ISysFavMenuService
    //{
    //    private readonly IMainRepositoryManager _mainRepositoryManager;
    //    private readonly IMapper _mapper;

    //    public SysFavMenuService(IMainRepositoryManager mainRepositoryManager, IMapper mapper)
    //    {
    //        this._mainRepositoryManager = mainRepositoryManager;
    //        this._mapper = mapper;
    //    }
    //    public async Task<IResult<SysFavMenuDto>> Add(SysFavMenuDto entity, CancellationToken cancellationToken = default)
    //    {
    //        if (entity == null) return await Result<SysFavMenuDto>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");

    //        try
    //        {
    //            var newEntity = await _mainRepositoryManager.SysFavMenuRepository.AddAndReturn(_mapper.Map<SysFavMenu>(entity));

    //            await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

    //            var entityMap = _mapper.Map<SysFavMenuDto>(newEntity);

    //            return await Result<SysFavMenuDto>.SuccessAsync(entityMap, "item added successfully");
    //        }
    //        catch (Exception exc)
    //        {

    //            return await Result<SysFavMenuDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
    //        }

    //    }

    //    public Task<IResult> ChangeActive(SysFavMenuDto entity, CancellationToken cancellationToken = default)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async Task<IResult<IEnumerable<SysFavMenuDto>>> GetAll(Expression<Func<SysFavMenuDto, bool>> expression, CancellationToken cancellationToken = default)
    //    {
    //        try
    //        {
    //            var mapExpr = _mapper.Map<Expression<Func<SysFavMenu, bool>>>(expression);
    //            var items = await _mainRepositoryManager.SysFavMenuRepository.GetAll(mapExpr);
    //            var itemMap = _mapper.Map<IEnumerable<SysFavMenuDto>>(items);
    //            //if (items == null || items.Any() == false) return await Result<IEnumerable<SysFavMenuDto>>.FailAsync("No Data");
    //            return await Result<IEnumerable<SysFavMenuDto>>.SuccessAsync(itemMap);
    //        }
    //        catch (Exception exp)
    //        {
    //            return await Result<IEnumerable<SysFavMenuDto>>.FailAsync($"EXP in {this.GetType()}, Meesage: {exp.Message}");
    //        }
    //    }

    //    public Task<IResult<DtResult<SysFavMenuDto>>> GetAll(DtRequest dtRequest, Expression<Func<SysFavMenuDto, bool>> expression, CancellationToken cancellationToken = default)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async Task<IResult<IEnumerable<SysFavMenuDto>>> GetAll(CancellationToken cancellationToken = default)
    //    {
    //        var items = await _mainRepositoryManager.SysFavMenuRepository.GetAll();

    //        var itemMap = _mapper.Map<IEnumerable<SysFavMenuDto>>(items);
    //        if (items == null) return await Result<IEnumerable<SysFavMenuDto>>.FailAsync("No Data Found");
    //        return await Result<IEnumerable<SysFavMenuDto>>.SuccessAsync(itemMap, "records retrieved");
    //    }

    //    public Task<IResult<DtResult<SysFavMenuDto>>> GetAll(DtRequest dtRequest, CancellationToken cancellationToken = default)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async Task<IResult<SysFavMenuDto>> GetById(int Id, CancellationToken cancellationToken = default)
    //    {
    //        try
    //        {
    //            var item = await _mainRepositoryManager.SysFavMenuRepository.GetById(Id);
    //            if (item == null) return Result<SysFavMenuDto>.Fail($"--- there is no Data with this id: {Id}---");
    //            var newEntity = _mapper.Map<SysFavMenuDto>(item);
    //            return await Result<SysFavMenuDto>.SuccessAsync(newEntity, "");

    //        }
    //        catch (Exception ex)
    //        {
    //            return Result<SysFavMenuDto>.Fail($"Exp in get data by Id: {ex.Message} --- {(ex.InnerException != null ? "InnerExp: " + ex.InnerException.Message : "no inner")} .");
    //        }
    //    }


    //    public async Task<IResult<SysFavMenuDto>> GetById(long Id, CancellationToken cancellationToken = default)
    //    {
    //        try
    //        {
    //            var item = await _mainRepositoryManager.SysFavMenuRepository.GetById(Id);
    //            if (item == null) return Result<SysFavMenuDto>.Fail($"--- there is no Data with this id: {Id}---");
    //            var newEntity = _mapper.Map<SysFavMenuDto>(item);
    //            return await Result<SysFavMenuDto>.SuccessAsync(newEntity, "");

    //        }
    //        catch (Exception ex)
    //        {
    //            return Result<SysFavMenuDto>.Fail($"Exp in get data by Id: {ex.Message} --- {(ex.InnerException != null ? "InnerExp: " + ex.InnerException.Message : "no inner")} .");
    //        }
    //    }
    //    public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<IResult<SysFavMenuDto>> Update(SysFavMenuDto entity, CancellationToken cancellationToken = default)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
