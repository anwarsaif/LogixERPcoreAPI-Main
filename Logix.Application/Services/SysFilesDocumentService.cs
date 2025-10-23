using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Services.Main
{
    public class SysFilesDocumentService : GenericQueryService<SysFilesDocument, SysFilesDocumentDto, SysFilesDocumentVw>, ISysFilesDocumentService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;

        public SysFilesDocumentService(IQueryRepository<SysFilesDocument> queryRepository,
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

        public async Task<IResult<SysFilesDocumentDto>> Add(SysFilesDocumentDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysFilesDocumentDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");
            try
            {
                var item = _mapper.Map<SysFilesDocument>(entity);

                var newEntity = await _mainRepositoryManager.SysFilesDocumentRepository.AddAndReturn(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysFilesDocumentDto>(newEntity);

                return await Result<SysFilesDocumentDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysFilesDocumentDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysFilesDocumentRepository.GetById(Id);
                if (item == null) return Result<SysFilesDocumentDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

                item.IsDeleted = true;
                _mainRepositoryManager.SysFilesDocumentRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysFilesDocumentDto>.SuccessAsync(_mapper.Map<SysFilesDocumentDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysFilesDocumentDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<SysFilesDocumentDto>> Update(SysFilesDocumentDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysFilesDocumentDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));
            try
            {
                var item = await _mainRepositoryManager.SysFilesDocumentRepository.GetById(entity.Id);
                if (item == null) return await Result<SysFilesDocumentDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

                entity.FacilityId = item.FacilityId;
                _mapper.Map(entity, item);

                _mainRepositoryManager.SysFilesDocumentRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysFilesDocumentDto>.SuccessAsync(_mapper.Map<SysFilesDocumentDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysFilesDocumentDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }
    }
}