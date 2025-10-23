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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Application.Services.Main
{
    public class SysCustomerTypeService : GenericQueryService<SysCustomerType, SysCustomerTypeDto, SysCustomerType>, ISysCustomerTypeService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;


        public SysCustomerTypeService(IQueryRepository<SysCustomerType> queryRepository, IMainRepositoryManager mainRepositoryManager, IMapper mapper, ICurrentData session) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.session = session;
        }
        public async Task<IResult<SysCustomerTypeDto>> Add(SysCustomerTypeDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysCustomerTypeDto>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");

            try
            {

                var item = _mapper.Map<SysCustomerType>(entity);
                var newEntity = await _mainRepositoryManager.SysCustomerTypeRepository.AddAndReturn(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysCustomerTypeDto>(newEntity);


                return await Result<SysCustomerTypeDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {

                return await Result<SysCustomerTypeDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }

        }

        public async Task<IResult<SysCustomerTypeDto>> Update(SysCustomerTypeDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysCustomerTypeDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");

            var item = await _mainRepositoryManager.SysCustomerTypeRepository.GetById(entity.TypeId);

            if (item == null) return await Result<SysCustomerTypeDto>.FailAsync($"--- there is no Data with this id: {entity.TypeId}---");


            _mapper.Map(entity, item);

            _mainRepositoryManager.SysCustomerTypeRepository.Update(item);

            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysCustomerTypeDto>.SuccessAsync(_mapper.Map<SysCustomerTypeDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
                return await Result<SysCustomerTypeDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.SysCustomerTypeRepository.GetById(Id);
            if (item == null) return Result<SysCustomerTypeDto>.Fail($"--- there is no Data with this id: {Id}---");

            _mainRepositoryManager.SysCustomerTypeRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysCustomerTypeDto>.SuccessAsync(_mapper.Map<SysCustomerTypeDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysCustomerTypeDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.SysCustomerTypeRepository.GetById(Id);
            if (item == null) return Result<SysCustomerTypeDto>.Fail($"--- there is no Data with this id: {Id}---");

            _mainRepositoryManager.SysCustomerTypeRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysCustomerTypeDto>.SuccessAsync(_mapper.Map<SysCustomerTypeDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysCustomerTypeDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }
    }
    }
