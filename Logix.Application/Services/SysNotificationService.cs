using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Services.Main
{
    public class SysNotificationService : GenericQueryService<SysNotification, SysNotificationDto, SysNotificationsVw>, ISysNotificationService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;

        public SysNotificationService(IQueryRepository<SysNotification> queryRepository,
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

        public async Task<IResult<SysNotificationDto>> Add(SysNotificationDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysNotificationDto>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");
            try
            {
                entity.CreatedOn = DateTime.Now;
                entity.CreatedBy = session.UserId;
                var newEntity = await _mainRepositoryManager.SysNotificationRepository.AddAndReturn(_mapper.Map<SysNotification>(entity));
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysNotificationDto>(newEntity);
                return await Result<SysNotificationDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysNotificationDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
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

        public Task<IResult<SysNotificationDto>> Update(SysNotificationDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public async Task<IResult<IEnumerable<SysNotificationsVw>>> GetTopVw(CancellationToken cancellationToken = default)
        {
            try
            {
                var items = await _mainRepositoryManager.SysNotificationRepository.GetTop(n => n.UserId == session.UserId && n.IsRead == false);
                return await Result<IEnumerable<SysNotificationsVw>>.SuccessAsync(items, "records retrieved");
            }
            catch (Exception exc)
            {
                return await Result<IEnumerable<SysNotificationsVw>>.FailAsync($"EXP in {GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult> ReadNotification(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysNotificationRepository.GetById(Id);
                if (item == null) return await Result.FailAsync($"{localization.GetMessagesResource("NoIdInUpdate")}");

                item.IsRead = true;
                item.ReadDate = DateTime.Now;
                _mainRepositoryManager.SysNotificationRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysNotificationDto>.SuccessAsync(_mapper.Map<SysNotificationDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> ReadAllNotifications(string Ids, CancellationToken cancellationToken = default)
        {
            try
            {
                var idsArr = Ids.Split(',');
                var items = await _mainRepositoryManager.SysNotificationRepository.GetAll(n => idsArr.Contains(n.Id.ToString()));
                //if (item == null) return await Result.FailAsync($"{localization.GetMessagesResource("NoIdInUpdate")}");

                await _mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);
                foreach (var item in items)
                {
                    item.IsRead = true;
                    item.ReadDate = DateTime.Now;
                    _mainRepositoryManager.SysNotificationRepository.Update(item);
                    await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                }
                await _mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);

                return await Result<SysNotificationDto>.SuccessAsync(localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        //public SysNotificationService(IMainRepositoryManager mainRepositoryManager, IMapper mapper)
        //{
        //    this._mainRepositoryManager = mainRepositoryManager;
        //    this._mapper = mapper;
        //}
        //    public async Task<IResult<SysNotificationDto>> Add(SysNotificationDto entity, CancellationToken cancellationToken = default)
        //    {
        //        if (entity == null) return await Result<SysNotificationDto>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");

        //        try
        //        {
        //            //entity.CreatedOn = DateTime.Now;
        //            //entity.CreatedBy = session.UserId;
        //            var newEntity = await _mainRepositoryManager.SysNotificationRepository.AddAndReturn(_mapper.Map<SysNotification>(entity));

        //            await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

        //            var entityMap = _mapper.Map<SysNotificationDto>(newEntity);

        //            return await Result<SysNotificationDto>.SuccessAsync(entityMap, "item added successfully");
        //        }
        //        catch (Exception exc)
        //        {

        //            return await Result<SysNotificationDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
        //        }

        //    }

        //    public Task<IResult> ChangeActive(SysNotificationDto entity, CancellationToken cancellationToken = default)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public async Task<IResult<IEnumerable<SysNotificationDto>>> GetAll(Expression<Func<SysNotificationDto, bool>> expression, CancellationToken cancellationToken = default)
        //    {
        //        try
        //        {
        //            var mapExpr = _mapper.Map<Expression<Func<SysNotification, bool>>>(expression);
        //            var items = await _mainRepositoryManager.SysNotificationRepository.GetAll(mapExpr);
        //            var itemMap = _mapper.Map<IEnumerable<SysNotificationDto>>(items);
        //            //if (items == null || items.Any() == false) return await Result<IEnumerable<SysNotificationDto>>.FailAsync("No Data");
        //            return await Result<IEnumerable<SysNotificationDto>>.SuccessAsync(itemMap);
        //        }
        //        catch (Exception exp)
        //        {
        //            return await Result<IEnumerable<SysNotificationDto>>.FailAsync($"EXP in {this.GetType()} , Message: {exp.Message}");
        //        }
        //    }

        //    public Task<IResult<DtResult<SysNotificationDto>>> GetAll(DtRequest dtRequest, Expression<Func<SysNotificationDto, bool>> expression, CancellationToken cancellationToken = default)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public async Task<IResult<IEnumerable<SysNotificationDto>>> GetAll(CancellationToken cancellationToken = default)
        //    {
        //        var items = await _mainRepositoryManager.SysNotificationRepository.GetAll();

        //        var itemMap = _mapper.Map<IEnumerable<SysNotificationDto>>(items);
        //        if (items == null) return await Result<IEnumerable<SysNotificationDto>>.FailAsync("No Data Found");
        //        return await Result<IEnumerable<SysNotificationDto>>.SuccessAsync(itemMap, "records retrieved");
        //    }

        //    public Task<IResult<DtResult<SysNotificationDto>>> GetAll(DtRequest dtRequest, CancellationToken cancellationToken = default)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public async Task<IResult<SysNotificationDto>> GetById(int Id, CancellationToken cancellationToken = default)
        //    {
        //        try
        //        {
        //            var item = await _mainRepositoryManager.SysNotificationRepository.GetById(Id);
        //            if (item == null) return Result<SysNotificationDto>.Fail($"--- there is no Data with this id: {Id}---");
        //            var newEntity = _mapper.Map<SysNotificationDto>(item);
        //            return await Result<SysNotificationDto>.SuccessAsync(newEntity, "");

        //        }
        //        catch (Exception ex)
        //        {
        //            return Result<SysNotificationDto>.Fail($"Exp in get data by Id: {ex.Message} --- {(ex.InnerException != null ? "InnerExp: " + ex.InnerException.Message : "no inner")} .");
        //        }
        //    }

        //    public async Task<IResult<SysNotificationDto>> GetById(long Id, CancellationToken cancellationToken = default)
        //    {
        //        try
        //        {
        //            var item = await _mainRepositoryManager.SysNotificationRepository.GetById(Id);
        //            if (item == null) return Result<SysNotificationDto>.Fail($"--- there is no Data with this id: {Id}---");
        //            var newEntity = _mapper.Map<SysNotificationDto>(item);
        //            return await Result<SysNotificationDto>.SuccessAsync(newEntity, "");

        //        }
        //        catch (Exception ex)
        //        {
        //            return Result<SysNotificationDto>.Fail($"Exp in get data by Id: {ex.Message} --- {(ex.InnerException != null ? "InnerExp: " + ex.InnerException.Message : "no inner")} .");
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

        //    public async Task<IResult<SysNotificationDto>> Update(SysNotificationDto entity, CancellationToken cancellationToken = default)
        //    {
        //        if (entity == null) return await Result<SysNotificationDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");

        //        var item = await _mainRepositoryManager.SysNotificationRepository.GetById(entity.Id);

        //        if (item == null) return await Result<SysNotificationDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");
        //        _mapper.Map(entity, item);
        //        _mainRepositoryManager.SysNotificationRepository.Update(item);

        //        try
        //        {
        //            await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

        //            return await Result<SysNotificationDto>.SuccessAsync(_mapper.Map<SysNotificationDto>(item), "Item updated successfully");
        //        }
        //        catch (Exception exp)
        //        {
        //            Console.WriteLine($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
        //            return await Result<SysNotificationDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
        //        }
        //    }
        //
    }
}
