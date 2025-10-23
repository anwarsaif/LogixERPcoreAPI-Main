using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Application.Services.Main
{
    public class SysRecordWebHookAuthService : GenericQueryService<SysRecordWebhookAuth, SysRecordWebhookAuthDto, SysRecordWebhookAuthVw>, ISysRecordWebHookAuthService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration configuration;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;
        public SysRecordWebHookAuthService(IQueryRepository<SysRecordWebhookAuth> queryRepository,
            IMapper mapper,
            IMainRepositoryManager mainRepositoryManager,
            IConfiguration configuration,
            ICurrentData session,
            ILocalizationService localization) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.configuration = configuration;
            this.session = session;
            this.localization = localization;
        }

        public async Task<IResult<SysRecordWebhookAuthDto>> Add(SysRecordWebhookAuthDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysRecordWebhookAuthDto>.FailAsync(localization.GetMessagesResource("AddNullEntity"));

            try
            {
                var item = _mapper.Map<SysRecordWebhookAuth>(entity);

                item.WebHookAuthId = entity.WebHookAuthId;
                item.ErrorReason = entity.ErrorReason;
                item.ErrorCode = entity.ErrorCode;
                item.Data = entity.Data;
                item.IsSended = entity.IsSended;
                item.CreatedBy = session.UserId;
                item.CreatedOn = DateTime.Now;

                var newEntity = await _mainRepositoryManager.SysRecordWebHookAuthRepository.AddAndReturn(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysRecordWebhookAuthDto>.SuccessAsync(_mapper.Map<SysRecordWebhookAuthDto>(newEntity), localization.GetResource1("AddSuccess"));
            }
            catch (Exception exc)
            {
                return await Result<SysRecordWebhookAuthDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult<SysRecordWebhookAuthEditDto>> Update(SysRecordWebhookAuthEditDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysRecordWebhookAuthEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

            try
            {
                var item = await _mainRepositoryManager.SysRecordWebHookAuthRepository.GetById(entity.Id);

                if (item == null) return await Result<SysRecordWebhookAuthEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

                _mapper.Map(entity, item);

                item.ErrorReason = entity.ErrorReason;
                item.ErrorCode = entity.ErrorCode;
                item.IsSended = entity.IsSended;
                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;

                _mainRepositoryManager.SysRecordWebHookAuthRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysRecordWebhookAuthEditDto>.SuccessAsync(_mapper.Map<SysRecordWebhookAuthEditDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysRecordWebhookAuthEditDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysRecordWebHookAuthRepository.GetById(Id);
                if (item == null)
                    return Result<SysRecordWebhookAuthDto>.Fail(localization.GetMessagesResource("InCorrectId"));

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                item.IsDeleted = true;
                _mainRepositoryManager.SysRecordWebHookAuthRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysRecordWebhookAuthDto>.SuccessAsync(_mapper.Map<SysRecordWebhookAuthDto>(item), localization.GetMessagesResource("DeletedSuccess"));
            }
            catch (Exception exp)
            {
                return await Result<SysRecordWebhookAuthDto>.FailAsync($"EXP in Remove at ({this.GetType()}), Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

    }
}
