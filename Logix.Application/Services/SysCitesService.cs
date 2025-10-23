using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices;
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
    public class SysCitesService : GenericQueryService<SysCites, SysCitesDto, SysCites>, ISysCitesService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        public SysCitesService(IQueryRepository<SysCites> queryRepository, IMainRepositoryManager mainRepositoryManager, IMapper mapper, ICurrentData session) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.session = session;
        }

        public async Task<IResult<SysCitesDto>> Add(SysCitesDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysCitesDto>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");

            try
            {
                // chk if city code exist
                var getCity = await _mainRepositoryManager.SysCitesRepository.GetAll(c => !string.IsNullOrEmpty(c.Code) && c.Code.Equals(entity.Code));
                if (getCity.Any())
                    return await Result<SysCitesDto>.FailAsync("رقم المنطقة موجود مسبقا");

                entity.CountryID ??= 0; entity.ParentID ??= 0;

                // if city has no parent => make it belong to country
                if (entity.ParentID == 0)
                    entity.ParentID = entity.CountryID;

                var item = _mapper.Map<SysCites>(entity);

                item.CreatedBy = session.UserId;
                item.CreatedOn = DateTime.Now;

                var newEntity = await _mainRepositoryManager.SysCitesRepository.AddAndReturn(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysCitesDto>(newEntity);
                return await Result<SysCitesDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysCitesDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult<SysCitesEditDto>> Update(SysCitesEditDto entity, CancellationToken cancellationToken = default)
        {
            try
            {
                if (entity == null) return await Result<SysCitesEditDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");

                var item = await _mainRepositoryManager.SysCitesRepository.GetById(entity.CityID);
                if (item == null) return await Result<SysCitesEditDto>.FailAsync($"--- there is no Data with this id: {entity.CityID}---");

                // chk if city code exist
                var getCity = await _mainRepositoryManager.SysCitesRepository.GetAll(c => c.CityID != entity.CityID
                    && !string.IsNullOrEmpty(c.Code) && c.Code.Equals(entity.Code));
                if (getCity.Any())
                    return await Result<SysCitesEditDto>.FailAsync("رقم المنطقة موجود مسبقا");

                entity.CountryID ??= 0; entity.ParentID ??= 0;

                // if city has no parent => make it belong to country
                if (entity.ParentID == 0)
                    entity.ParentID = entity.CountryID;

                if (entity.ParentID == entity.CityID)
                    return await Result<SysCitesEditDto>.FailAsync("لا يمكن أن تكون المنطقة منطقة رئيسية لنفسها");

                item.ModifiedOn = DateTime.Now;
                item.ModifiedBy = session.UserId;

                _mapper.Map(entity, item);
                _mainRepositoryManager.SysCitesRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysCitesEditDto>.SuccessAsync(_mapper.Map<SysCitesEditDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                return await Result<SysCitesEditDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysCitesRepository.GetById(Id);
                if (item == null) return Result<SysCitesDto>.Fail($"--- there is no Data with this id: {Id}---");

                // chk if city has childern
                var getChild = await _mainRepositoryManager.SysCitesRepository.GetAll(c => c.ParentID == Id && c.IsDeleted == false);
                if (getChild.Any())
                    return Result<SysCitesDto>.Fail("لا يمكن حذف هذه المنطقة وذلك لاحتوائها على مناطق أخرى");

                item.IsDeleted = true;
                item.ModifiedOn = DateTime.Now;
                item.ModifiedBy = session.UserId;

                _mainRepositoryManager.SysCitesRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                return await Result<SysCitesDto>.SuccessAsync(_mapper.Map<SysCitesDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysCitesDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }
    }
}