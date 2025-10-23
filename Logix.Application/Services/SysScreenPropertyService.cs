using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using System.Linq.Expressions;

namespace Logix.Application.Services.Main
{
    public class SysScreenPropertyService : GenericQueryService<SysScreenProperty, SysScreenPropertyDto, SysScreenProperty>, ISysScreenPropertyService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;

        public SysScreenPropertyService(IQueryRepository<SysScreenProperty> queryRepository,
            IMapper mapper,
            IMainRepositoryManager mainRepositoryManager) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
        }

        public async Task<IResult<SysScreenPropertyDto>> Add(SysScreenPropertyDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysScreenPropertyDto>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");

            try
            {
                var newEntity = await _mainRepositoryManager.SysScreenPropertyRepository.AddAndReturn(_mapper.Map<SysScreenProperty>(entity));

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysScreenPropertyDto>(newEntity);

                return await Result<SysScreenPropertyDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {

                return await Result<SysScreenPropertyDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
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

        public Task<IResult<SysScreenPropertyDto>> Update(SysScreenPropertyDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        //public SysScreenPropertyService(IMainRepositoryManager mainRepositoryManager, IMapper mapper)
        //{
        //    this._mainRepositoryManager = mainRepositoryManager;
        //    this._mapper = mapper;
        //}


        //    public Task<IResult> ChangeActive(SysScreenPropertyDto entity, CancellationToken cancellationToken = default)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public async Task<IResult<IEnumerable<SysScreenPropertyDto>>> GetAll(Expression<Func<SysScreenPropertyDto, bool>> expression, CancellationToken cancellationToken = default)
        //    {
        //        try
        //        {
        //            var mapExpr = _mapper.Map<Expression<Func<SysScreenProperty, bool>>>(expression);
        //            var items = await _mainRepositoryManager.SysScreenPropertyRepository.GetAll(mapExpr);
        //            var itemMap = _mapper.Map<IEnumerable<SysScreenPropertyDto>>(items);

        //            return await Result<IEnumerable<SysScreenPropertyDto>>.SuccessAsync(itemMap);
        //        }
        //        catch (Exception exp)
        //        {
        //            return await Result<IEnumerable<SysScreenPropertyDto>>.FailAsync($"EXP in {this.GetType()} , Message: {exp.Message}");
        //        }
        //    }

        //    public Task<IResult<DtResult<SysScreenPropertyDto>>> GetAll(DtRequest dtRequest, Expression<Func<SysScreenPropertyDto, bool>> expression, CancellationToken cancellationToken = default)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public async Task<IResult<IEnumerable<SysScreenPropertyDto>>> GetAll(CancellationToken cancellationToken = default)
        //    {
        //        var items = await _mainRepositoryManager.SysScreenPropertyRepository.GetAll();

        //        var itemMap = _mapper.Map<IEnumerable<SysScreenPropertyDto>>(items);
        //        if (items == null) return await Result<IEnumerable<SysScreenPropertyDto>>.FailAsync("No Data Found");
        //        return await Result<IEnumerable<SysScreenPropertyDto>>.SuccessAsync(itemMap, "records retrieved");
        //    }

        //    public Task<IResult<DtResult<SysScreenPropertyDto>>> GetAll(DtRequest dtRequest, CancellationToken cancellationToken = default)
        //    {
        //        throw new NotImplementedException();
        //    }


        //    public async Task<IResult<SysScreenPropertyDto>> GetById(int Id, CancellationToken cancellationToken = default)
        //    {
        //        try
        //        {
        //            var item = await _mainRepositoryManager.SysScreenPropertyRepository.GetById(Id);
        //            if (item == null) return Result<SysScreenPropertyDto>.Fail($"--- there is no Data with this id: {Id}---");
        //            var newEntity = _mapper.Map<SysScreenPropertyDto>(item);
        //            return await Result<SysScreenPropertyDto>.SuccessAsync(newEntity, "");

        //        }
        //        catch (Exception ex)
        //        {
        //            return Result<SysScreenPropertyDto>.Fail($"Exp in get data by Id: {ex.Message} --- {(ex.InnerException != null ? "InnerExp: " + ex.InnerException.Message : "no inner")} .");
        //        }
        //    }


        //    public async Task<IResult<SysScreenPropertyDto>> GetById(long Id, CancellationToken cancellationToken = default)
        //    {
        //        try
        //        {
        //            var item = await _mainRepositoryManager.SysScreenPropertyRepository.GetById(Id);
        //            if (item == null) return Result<SysScreenPropertyDto>.Fail($"--- there is no Data with this id: {Id}---");
        //            var newEntity = _mapper.Map<SysScreenPropertyDto>(item);
        //            return await Result<SysScreenPropertyDto>.SuccessAsync(newEntity, "");

        //        }
        //        catch (Exception ex)
        //        {
        //            return Result<SysScreenPropertyDto>.Fail($"Exp in get data by Id: {ex.Message} --- {(ex.InnerException != null ? "InnerExp: " + ex.InnerException.Message : "no inner")} .");
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

        //    public Task<IResult<SysScreenPropertyDto>> Update(SysScreenPropertyDto entity, CancellationToken cancellationToken = default)
        //    {
        //        throw new NotImplementedException();
        //    }
        //
    }
}
