using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Services.Main
{
    public class SysCustomerFileService : GenericQueryService<SysCustomerFile, SysCustomerFileDto, SysCustomerFile>, ISysCustomerFileService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;


        public SysCustomerFileService(IQueryRepository<SysCustomerFile> queryRepository, IMainRepositoryManager mainRepositoryManager, IMapper mapper, ICurrentData session) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.session = session;
        }
        public async Task<IResult<SysCustomerFileDto>> Add(SysCustomerFileDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysCustomerFileDto>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");

            try
            {

                var item = _mapper.Map<SysCustomerFile>(entity);
                var newEntity = await _mainRepositoryManager.SysCustomerFileRepository.AddAndReturn(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysCustomerFileDto>(newEntity);


                return await Result<SysCustomerFileDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {

                return await Result<SysCustomerFileDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }

        }

        public async Task<IResult<SysCustomerFileDto>> Update(SysCustomerFileDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysCustomerFileDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");

            var item = await _mainRepositoryManager.SysCustomerFileRepository.GetById(entity.Id);

            if (item == null) return await Result<SysCustomerFileDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");


            _mapper.Map(entity, item);

            _mainRepositoryManager.SysCustomerFileRepository.Update(item);

            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysCustomerFileDto>.SuccessAsync(_mapper.Map<SysCustomerFileDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
                return await Result<SysCustomerFileDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.SysCustomerFileRepository.GetById(Id);
            if (item == null) return Result<SysCustomerFileDto>.Fail($"--- there is no Data with this id: {Id}---");

            _mainRepositoryManager.SysCustomerFileRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysCustomerFileDto>.SuccessAsync(_mapper.Map<SysCustomerFileDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysCustomerFileDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.SysCustomerFileRepository.GetById(Id);
            if (item == null) return Result<SysCustomerFileDto>.Fail($"--- there is no Data with this id: {Id}---");

            _mainRepositoryManager.SysCustomerFileRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysCustomerFileDto>.SuccessAsync(_mapper.Map<SysCustomerFileDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysCustomerFileDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }
    }
}
