using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Interfaces.IServices.WA;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Services.Main
{
    public class SysMailServerService : GenericQueryService<SysMailServer, SysMailServerDto, SysMailServer>, ISysMailServerService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;

        private readonly IMapper _mapper;
        private readonly ICurrentData _session;
        private readonly IWhatsappBusinessService waService;
        private readonly ILocalizationService localization;

        public SysMailServerService(IQueryRepository<SysMailServer> queryRepository, IMainRepositoryManager mainRepositoryManager, IMapper mapper, ICurrentData session, IWhatsappBusinessService waService, ILocalizationService localization) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;

            this._session = session;
            this.waService = waService;

            this.localization = localization;
        }

        public async Task<IResult<SysMailServerDto>> Add(SysMailServerDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysMailServerDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");

            try
            {
               

                var item = _mapper.Map<SysMailServer>(entity);
                var newEntity = await _mainRepositoryManager.SysMailServerRepository.AddAndReturn(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysMailServerDto>(newEntity);

                return await Result<SysMailServerDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysMailServerDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }

        }

        public async Task<IResult<SysMailServerDto>> Update(SysMailServerDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysMailServerDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");

            var item = await _mainRepositoryManager.SysMailServerRepository.GetById(entity.Id);

            if (item == null) return await Result<SysMailServerDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");


            entity.UserId = _session.UserId;

            _mapper.Map(entity, item);

            _mainRepositoryManager.SysMailServerRepository.Update(item);

            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysMailServerDto>.SuccessAsync(_mapper.Map<SysMailServerDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
                return await Result<SysMailServerDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.SysMailServerRepository.GetById(Id);
            if (item == null) return Result<SysMailServerDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");
            item.IsDeleted = true;
            //item.ModifiedOn = DateTime.Now;
            item.UserId = _session.UserId;
            _mainRepositoryManager.SysMailServerRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysMailServerDto>.SuccessAsync(_mapper.Map<SysMailServerDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysMailServerDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.SysMailServerRepository.GetById(Id);
            if (item == null) return Result<SysMailServerDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");
            item.IsDeleted = true;
            //item.ModifiedOn = DateTime.Now;
            item.UserId = _session.UserId;
            _mainRepositoryManager.SysMailServerRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysMailServerDto>.SuccessAsync(_mapper.Map<SysMailServerDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysMailServerDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

    }
}
