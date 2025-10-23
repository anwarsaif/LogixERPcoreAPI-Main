using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Application.Services.Main
{
    public class SysAnnouncementService : GenericQueryService<SysAnnouncement, SysAnnouncementDto, SysAnnouncementVw>, ISysAnnouncementService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;

        private readonly IMapper _mapper;
        private readonly ICurrentData _session;

        public SysAnnouncementService(IQueryRepository<SysAnnouncement> queryRepository, IMainRepositoryManager mainRepositoryManager, IMapper mapper, ICurrentData session) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this._session = session;
        }

        public async Task<IResult<SysAnnouncementDto>> Add(SysAnnouncementDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysAnnouncementDto>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");
            try
            {
                var item = _mapper.Map<SysAnnouncement>(entity);
                item.GroupsId = "0";
                item.FacilityId = _session.FacilityId;
                item.CreatedBy = _session.UserId;
                item.UsersId = _session.UserId.ToString();
                item.PublishDate = DateHelper.GetCurrentDateTime().ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);

                var newEntity = await _mainRepositoryManager.SysAnnouncementRepository.AddAndReturn(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysAnnouncementDto>(newEntity);
                return await Result<SysAnnouncementDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysAnnouncementDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult<SysAnnouncementEditDto>> Update(SysAnnouncementEditDto entity, CancellationToken cancellationToken = default)
        {
            try
            {
                if (entity == null) return await Result<SysAnnouncementEditDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");
                var item = await _mainRepositoryManager.SysAnnouncementRepository.GetById(entity.Id);

                if (item == null) return await Result<SysAnnouncementEditDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");
                _mapper.Map(entity, item);
                item.ModifiedBy = _session.UserId;
                item.ModifiedOn = DateTime.Now;

                _mainRepositoryManager.SysAnnouncementRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysAnnouncementEditDto>.SuccessAsync(_mapper.Map<SysAnnouncementEditDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                return await Result<SysAnnouncementEditDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysAnnouncementRepository.GetById(Id);
                if (item == null) return await Result<SysAnnouncementDto>.FailAsync($"--- there is no Data with this id: {Id}---");
                
                item.IsDeleted = true;
                item.ModifiedOn = DateTime.Now;
                item.ModifiedBy = _session.UserId;
                
                _mainRepositoryManager.SysAnnouncementRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                return await Result<SysAnnouncementDto>.SuccessAsync(_mapper.Map<SysAnnouncementDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysAnnouncementDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysAnnouncementRepository.GetById(Id);
                if (item == null) return await Result<SysAnnouncementDto>.FailAsync($"--- there is no Data with this id: {Id}---");

                item.IsDeleted = true;
                item.ModifiedOn = DateTime.Now;
                item.ModifiedBy = _session.UserId;

                _mainRepositoryManager.SysAnnouncementRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                return await Result<SysAnnouncementDto>.SuccessAsync(_mapper.Map<SysAnnouncementDto>(item), " record removed");
            }
            catch (Exception exp)
            {
                return await Result<SysAnnouncementDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        //============== Cusome Function only in this Service =======================================
        public async Task<IResult<IEnumerable<SysAnnouncementLocationVw>>> GetSysAnnouncementLocationVw(CancellationToken cancellationToken = default)
        {
            try
            {
                var items = await _mainRepositoryManager.SysAnnouncementRepository.GeSysAnnouncementLocationVw();
                if (items == null) return await Result<IEnumerable<SysAnnouncementLocationVw>>.FailAsync("No Data Found");
                return await Result<IEnumerable<SysAnnouncementLocationVw>>.SuccessAsync(items.Where(d => d.Isdel == false).OrderBy(d => d.SortNo).ToList(), "records retrieved");
            }
            catch (Exception exp)
            {
                return await Result<IEnumerable<SysAnnouncementLocationVw>>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }
    }
}
