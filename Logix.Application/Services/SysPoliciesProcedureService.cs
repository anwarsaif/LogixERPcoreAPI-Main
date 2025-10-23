using AutoMapper;
using Logix.Application.Common;
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
    public class SysPoliciesProcedureService : GenericQueryService<SysPoliciesProcedure, SysPoliciesProcedureDto, SysPoliciesProceduresVw>, ISysPoliciesProcedureService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;

        public SysPoliciesProcedureService(IQueryRepository<SysPoliciesProcedure> queryRepository,
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

        public async Task<IResult<SysPoliciesProcedureDto>> Add(SysPoliciesProcedureDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysPoliciesProcedureDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");

            try
            {
                entity.FacilityId = session.FacilityId;
                
                var item = _mapper.Map<SysPoliciesProcedure>(entity);
                item.CreatedBy = session.UserId;
                item.CreatedOn = DateTime.Now;
                var newEntity = await _mainRepositoryManager.SysPoliciesProcedureRepository.AddAndReturn(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysPoliciesProcedureDto>(newEntity);

                return await Result<SysPoliciesProcedureDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysPoliciesProcedureDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult<SysPoliciesProcedureEditDto>> Update(SysPoliciesProcedureEditDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysPoliciesProcedureEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

            try
            {
                var item = await _mainRepositoryManager.SysPoliciesProcedureRepository.GetById(entity.Id);

                if (item == null) return await Result<SysPoliciesProcedureEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

                _mapper.Map(entity, item);

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                _mainRepositoryManager.SysPoliciesProcedureRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysPoliciesProcedureEditDto>.SuccessAsync(_mapper.Map<SysPoliciesProcedureEditDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysPoliciesProcedureEditDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysPoliciesProcedureRepository.GetById(Id);
                if (item == null) return Result<SysPoliciesProcedureDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                item.IsDeleted = true;

                _mainRepositoryManager.SysPoliciesProcedureRepository.Update(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysPoliciesProcedureDto>.SuccessAsync(_mapper.Map<SysPoliciesProcedureDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysPoliciesProcedureDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysPoliciesProcedureRepository.GetById(Id);
                if (item == null) return Result<SysPoliciesProcedureDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                item.IsDeleted = true;

                _mainRepositoryManager.SysPoliciesProcedureRepository.Update(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysPoliciesProcedureDto>.SuccessAsync(_mapper.Map<SysPoliciesProcedureDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysPoliciesProcedureDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

    }
}
