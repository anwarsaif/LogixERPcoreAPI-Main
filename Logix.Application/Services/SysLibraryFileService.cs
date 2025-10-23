using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.DTOs.PM;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using Microsoft.Extensions.Configuration;

namespace Logix.Application.Services.Main
{
    public class SysLibraryFileService : GenericQueryService<SysLibraryFile, SysLibraryFileDto, SysLibraryFilesVw>, ISysLibraryFileService
    {
        private readonly IMainRepositoryManager mainRepositoryManager;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;
        private readonly IPMRepositoryManager pMRepositoryManager;


        public SysLibraryFileService(IQueryRepository<SysLibraryFile> queryRepository, IMapper mapper, IMainRepositoryManager mainRepositoryManager, IConfiguration configuration, ICurrentData session, ILocalizationService localization, IPMRepositoryManager pMRepositoryManager) : base(queryRepository, mapper)
        {
            this.mainRepositoryManager = mainRepositoryManager;
            this.mapper = mapper;
            this.configuration = configuration;
            this.session = session;
            this.localization = localization;
            this.pMRepositoryManager = pMRepositoryManager; 
        }

        public async Task<IResult<SysLibraryFileDto>> Add(SysLibraryFileDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysLibraryFileDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");
            try
            {
                // check if Project Is Exist
                var Project = await pMRepositoryManager.PMProjectsRepository.GetOne(x => x.IsDeleted == false && x.Id == entity.ProjectId);
                if (Project == null) return await Result<SysLibraryFileDto>.FailAsync($"{localization.GetResource1("TheProjectNumberIsNotFoundInTheProjectList")}");
                var MappedEntity = mapper.Map<SysLibraryFile>(entity);
                MappedEntity.ProjectId = Project.Id;
                MappedEntity.CreatedBy = session.UserId;
                MappedEntity.CreatedOn = DateHelper.GetCurrentDateTime();
                MappedEntity.IsDeleted = false;
                var newEntity = await mainRepositoryManager.SysLibraryFileRepository.AddAndReturn(MappedEntity);
                await pMRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = mapper.Map<SysLibraryFileDto>(newEntity);

                return await Result<SysLibraryFileDto>.SuccessAsync(entityMap, localization.GetResource1("AddSuccess"));
            }
            catch (Exception)
            {
                return await Result<SysLibraryFileDto>.FailAsync(localization.GetResource1("ErrorOccurredDuring"));
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await mainRepositoryManager.SysLibraryFileRepository.GetOne(x => x.Id == Id);
                if (item == null) return Result<SysLibraryFileDto>.Fail($"{localization.GetMessagesResource("NoItemFoundToDelete")}");
                item.IsDeleted = true;
                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                mainRepositoryManager.SysLibraryFileRepository.Update(item);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                return await Result<SysLibraryFileDto>.SuccessAsync(mapper.Map<SysLibraryFileDto>(item), localization.GetResource1("DeleteSuccess"));
            }
            catch (Exception exp)
            {
                return await Result<SysLibraryFileDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await mainRepositoryManager.SysLibraryFileRepository.GetOne(x => x.Id == Id);
                if (item == null) return Result<SysLibraryFileDto>.Fail($"{localization.GetMessagesResource("NoItemFoundToDelete")}");
                item.IsDeleted = true;
                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                mainRepositoryManager.SysLibraryFileRepository.Update(item);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                return await Result<SysLibraryFileDto>.SuccessAsync(mapper.Map<SysLibraryFileDto>(item), localization.GetResource1("DeleteSuccess"));
            }
            catch (Exception exp)
            {
                return await Result<SysLibraryFileDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<SysLibraryFileEditDto>> Update(SysLibraryFileEditDto entity, CancellationToken cancellationToken = default)
        {
            try
            {

                var item = await mainRepositoryManager.SysLibraryFileRepository.GetById(entity.Id);
                if (item == null) return await Result<SysLibraryFileEditDto>.FailAsync($"{localization.GetMessagesResource("NoItemFoundToEdit")}");
                var Project = await pMRepositoryManager.PMProjectsRepository.GetOne(x => x.IsDeleted == false && x.Code == entity.ProjectCode);
                if (Project == null) return await Result<SysLibraryFileEditDto>.FailAsync($"{localization.GetResource1("TheProjectNumberIsNotFoundInTheProjectList")}");

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateHelper.GetCurrentDateTime();
                item.ProjectId = Project.Id;
                item.FileName = entity.FileName;
                item.FileDate = entity.FileDate;
                item.RefranceCode = entity.RefranceCode;
                item.FileType = entity.FileType;
                item.FileDescription = entity.FileDescription;
                item.SourceFile = entity.SourceFile;
                item.EndDateFile = entity.EndDateFile;
                item.FileUrl = entity.FileUrl;

                mainRepositoryManager.SysLibraryFileRepository.Update(item);
                await pMRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                var entityMap = mapper.Map<SysLibraryFileEditDto>(item);

                return await Result<SysLibraryFileEditDto>.SuccessAsync(entityMap, localization.GetResource1("UpdateSuccess"));

            }
            catch (Exception exc)
            {
                return await Result<SysLibraryFileEditDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message} && {localization.GetResource1("UpdateError")}");
            }
        }
    }
}
