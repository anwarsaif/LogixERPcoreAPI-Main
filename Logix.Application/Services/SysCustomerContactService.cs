using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Services.Main
{
    public class SysCustomerContactService : GenericQueryService<SysCustomerContact, SysCustomerContactDto, SysCustomerContact>, ISysCustomerContactService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;


        public SysCustomerContactService(IQueryRepository<SysCustomerContact> queryRepository, IMainRepositoryManager mainRepositoryManager, IMapper mapper, ICurrentData session) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.session = session;
        }
        public async Task<IResult<SysCustomerContactDto>> Add(SysCustomerContactDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysCustomerContactDto>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");

            try
            {

                var item = _mapper.Map<SysCustomerContact>(entity);
                var newEntity = await _mainRepositoryManager.SysCustomerContactRepository.AddAndReturn(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysCustomerContactDto>(newEntity);


                return await Result<SysCustomerContactDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {

                return await Result<SysCustomerContactDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }

        }

        public async Task<IResult<SysCustomerContactDto>> Update(SysCustomerContactDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysCustomerContactDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");

            var item = await _mainRepositoryManager.SysCustomerContactRepository.GetById(entity.Id);

            if (item == null) return await Result<SysCustomerContactDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");


            _mapper.Map(entity, item);

            _mainRepositoryManager.SysCustomerContactRepository.Update(item);

            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysCustomerContactDto>.SuccessAsync(_mapper.Map<SysCustomerContactDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
                return await Result<SysCustomerContactDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.SysCustomerContactRepository.GetById(Id);
            if (item == null) return Result<SysCustomerContactDto>.Fail($"--- there is no Data with this id: {Id}---");
            item.IsDeleted = true;
            item.ModifiedBy = session.UserId;
            item.ModifiedOn = DateTime.Now;
            _mainRepositoryManager.SysCustomerContactRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysCustomerContactDto>.SuccessAsync(_mapper.Map<SysCustomerContactDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysCustomerContactDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.SysCustomerContactRepository.GetById(Id);
            if (item == null) return Result<SysCustomerContactDto>.Fail($"--- there is no Data with this id: {Id}---");
            item.IsDeleted = true;
            item.ModifiedBy = session.UserId;
            item.ModifiedOn = DateTime.Now;
            _mainRepositoryManager.SysCustomerContactRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysCustomerContactDto>.SuccessAsync(_mapper.Map<SysCustomerContactDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysCustomerContactDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }
    }
}
