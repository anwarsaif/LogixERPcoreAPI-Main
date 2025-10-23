using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.HR;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Application.Services.Main
{
    public class SysNotificationsMangService : GenericQueryService<SysNotificationsMang, SysNotificationsMangDto, SysNotificationsMangVw>, ISysNotificationsMangService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;

        public SysNotificationsMangService(IQueryRepository<SysNotificationsMang> queryRepository,
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

        public async Task<IResult<SysNotificationsMangDto>> Add(SysNotificationsMangDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysNotificationsMangDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");
            try
            {
                var item = _mapper.Map<SysNotificationsMang>(entity);
                item.CreatedBy = session.UserId;
                item.CreatedOn = DateTime.Now;
                var newEntity = await _mainRepositoryManager.SysNotificationsMangRepository.AddAndReturn(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysNotificationsMangDto>(newEntity);

                return await Result<SysNotificationsMangDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysNotificationsMangDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysNotificationsMangRepository.GetById(Id);
                if (item == null) return Result<SysNotificationsMangDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                item.IsDeleted = true;

                _mainRepositoryManager.SysNotificationsMangRepository.Update(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysNotificationsMangDto>.SuccessAsync(_mapper.Map<SysNotificationsMangDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysNotificationsMangDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<SysNotificationsMangEditDto>> Update(SysNotificationsMangEditDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysNotificationsMangEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

            try
            {
                var item = await _mainRepositoryManager.SysNotificationsMangRepository.GetById(entity.Id);

                if (item == null) return await Result<SysNotificationsMangEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

                _mapper.Map(entity, item);

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                _mainRepositoryManager.SysNotificationsMangRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysNotificationsMangEditDto>.SuccessAsync(_mapper.Map<SysNotificationsMangEditDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                Console.WriteLine($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
                return await Result<SysNotificationsMangEditDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }
        public async Task<IResult<List<SysNotificationsMangResultDto>>> GetNotificationsByUserAndGroupAsync()
        {
            try
            {
                var result = await _mainRepositoryManager.SysNotificationsMangRepository.GetNotificationsByUserAndGroupAsync();
                return await Result<List<SysNotificationsMangResultDto>>.SuccessAsync(result, "");
            }
            catch (Exception exp)
            {
                return await Result<List<SysNotificationsMangResultDto>>.FailAsync($"EXP in GetNotificationsByUserAndGroupAsync at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }

        }
    }
}
