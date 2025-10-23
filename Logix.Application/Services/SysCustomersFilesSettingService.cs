using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Services.Main
{
    public class SysCustomersFilesSettingService : GenericQueryService<SysCustomersFilesSetting, SysCustomersFilesSettingDto, SysCustomersFilesSettingsVw>, ISysCustomersFilesSettingService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData currentData;
        private readonly ILocalizationService localization;

        public SysCustomersFilesSettingService(IQueryRepository<SysCustomersFilesSetting> queryRepository,
            IMapper mapper,
            IMainRepositoryManager mainRepositoryManager,
            ICurrentData currentData,
            ILocalizationService localization) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.currentData = currentData;
            this.localization = localization;
        }

        public Task<IResult<SysCustomersFilesSettingDto>> Add(SysCustomersFilesSettingDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<SysCustomersFilesSettingDto>> Update(SysCustomersFilesSettingDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysCustomersFilesSettingDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

            try
            {
                if (entity.Id > 0)
                {
                    var item = await _mainRepositoryManager.SysCustomersFilesSettingRepository.GetById(entity.Id);

                    if (item == null) return await Result<SysCustomersFilesSettingDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

                    item.IsRequired = entity.IsRequired;
                    item.RequiresAuthorization = entity.RequiresAuthorization;
                    _mainRepositoryManager.SysCustomersFilesSettingRepository.Update(item);
                    await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                }
                else
                {
                    SysCustomersFilesSetting newItem = new()
                    {
                        FileTypeId = entity.FileTypeId,
                        CustomerTypeId = entity.CustomerTypeId,
                        IsRequired = entity.IsRequired,
                        RequiresAuthorization = entity.RequiresAuthorization,
                        FacilityId = Convert.ToInt32(currentData.FacilityId)
                    };
                    var newEntity = await _mainRepositoryManager.SysCustomersFilesSettingRepository.AddAndReturn(newItem);
                    await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                }

                return await Result<SysCustomersFilesSettingDto>.SuccessAsync(localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysCustomersFilesSettingDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }
    }
}
