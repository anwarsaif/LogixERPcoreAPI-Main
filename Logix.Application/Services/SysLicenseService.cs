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
    public class SysLicenseService : GenericQueryService<SysLicense, SysLicenseDto, SysLicensesVw>, ISysLicenseService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;

        public SysLicenseService(IQueryRepository<SysLicense> queryRepository,
            IMapper mapper,
            IMainRepositoryManager mainRepositoryManager,
            ICurrentData session,
            ILocalizationService localization
            ) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.session = session;
            this.localization = localization;
        }

        public async Task<IResult<SysLicenseDto>> Add(SysLicenseDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysLicenseDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");

            try
            {
                //entity.CreatedOn = DateTime.Now;
                var item = _mapper.Map<SysLicense>(entity);
                var newEntity = await _mainRepositoryManager.SysLicenseRepository.AddAndReturn(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysLicenseDto>(newEntity);

                return await Result<SysLicenseDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {

                return await Result<SysLicenseDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.SysLicenseRepository.GetById(Id);
            if (item == null) return Result<SysLicenseDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");
            item.IsDeleted = true;
            item.ModifiedOn = DateTime.Now;
            item.ModifiedBy = session.UserId;
            _mainRepositoryManager.SysLicenseRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysLicenseDto>.SuccessAsync(_mapper.Map<SysLicenseDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysLicenseDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            var item = await _mainRepositoryManager.SysLicenseRepository.GetById(Id);
            if (item == null) return Result<SysLicenseDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");
            item.IsDeleted = true;
            item.ModifiedOn = DateTime.Now;
            item.ModifiedBy = session.UserId;
            _mainRepositoryManager.SysLicenseRepository.Update(item);
            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysLicenseDto>.SuccessAsync(_mapper.Map<SysLicenseDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysLicenseDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<SysLicenseDto>> Update(SysLicenseDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysLicenseDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");

            var item = await _mainRepositoryManager.SysLicenseRepository.GetById(entity.Id);

            if (item == null) return await Result<SysLicenseDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");

            entity.CreatedBy = item.CreatedBy;
            entity.CreatedOn = item.CreatedOn;
            entity.FacilityId = item.FacilityId;

            item.ModifiedOn = DateTime.Now;
            item.ModifiedBy = session.UserId;

            _mapper.Map(entity, item);

            _mainRepositoryManager.SysLicenseRepository.Update(item);

            try
            {
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysLicenseDto>.SuccessAsync(_mapper.Map<SysLicenseDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
                return await Result<SysLicenseDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }
    }
    }
