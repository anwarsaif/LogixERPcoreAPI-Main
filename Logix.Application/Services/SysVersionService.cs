using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Services.Main
{
    public class SysVersionService : GenericQueryService<SysVersion, SysVersionDto, SysVersion>, ISysVersionService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;

        public SysVersionService(IQueryRepository<SysVersion> queryRepository,
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

        public async Task<IResult<SysVersionDto>> Add(SysVersionDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysVersionDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");

            try
            {
                entity.CreatedOn = DateTime.Now;

                var newEntity = await _mainRepositoryManager.SysVersionRepository.AddAndReturn(_mapper.Map<SysVersion>(entity));

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysVersionDto>(newEntity);

                return await Result<SysVersionDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysVersionDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysVersionRepository.GetById(Id);
                if (item == null) return Result<SysVersionDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

                _mainRepositoryManager.SysVersionRepository.Update(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysVersionDto>.SuccessAsync(_mapper.Map<SysVersionDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysVersionDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysVersionRepository.GetById(Id);
                if (item == null) return Result<SysVersionDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");
                _mainRepositoryManager.SysVersionRepository.Update(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysVersionDto>.SuccessAsync(_mapper.Map<SysVersionDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysVersionDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<SysVersionEditDto>> Update(SysVersionEditDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysVersionEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

            try
            {
                var item = await _mainRepositoryManager.SysVersionRepository.GetById(entity.Id);

                if (item == null) return await Result<SysVersionEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

                _mapper.Map(entity, item);

                _mainRepositoryManager.SysVersionRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysVersionEditDto>.SuccessAsync(_mapper.Map<SysVersionEditDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                Console.WriteLine($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
                return await Result<SysVersionEditDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }
    }
    }
