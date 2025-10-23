using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Services.Main
{
    public class SysSettingExportService : GenericQueryService<SysSettingExport, SysSettingExportDto, SysSettingExportVw>, ISysSettingExportService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;

        public SysSettingExportService(IQueryRepository<SysSettingExport> queryRepository,
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

        public async Task<IResult<SysSettingExportDto>> Add(SysSettingExportDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysSettingExportDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");
            try
            {
                var item = _mapper.Map<SysSettingExport>(entity);
                item.CreatedBy = session.UserId;
                item.CreatedOn = DateTime.Now;

                var newEntity = await _mainRepositoryManager.SysSettingExportRepository.AddAndReturn(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysSettingExportDto>(newEntity);

                return await Result<SysSettingExportDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysSettingExportDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysSettingExportRepository.GetById(Id);
                if (item == null) return Result<SysSettingExportDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                item.IsDeleted = true;

                _mainRepositoryManager.SysSettingExportRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysSettingExportDto>.SuccessAsync(_mapper.Map<SysSettingExportDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysSettingExportDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<SysSettingExportDto>> Update(SysSettingExportDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysSettingExportDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));
            try
            {
                var item = await _mainRepositoryManager.SysSettingExportRepository.GetById(entity.Id);
                if (item == null) return await Result<SysSettingExportDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

                entity.FacilityId = item.FacilityId;
                entity.SystemId = item.SystemId;
                entity.ScreenId = item.ScreenId;
                entity.IsDeleted = item.IsDeleted;
                _mapper.Map(entity, item);

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                _mainRepositoryManager.SysSettingExportRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysSettingExportDto>.SuccessAsync(_mapper.Map<SysSettingExportDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysSettingExportDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }
    }
}