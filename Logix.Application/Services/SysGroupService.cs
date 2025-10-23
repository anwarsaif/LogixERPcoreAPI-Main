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
    public class SysGroupService : GenericQueryService<SysGroup, SysGroupDto, SysGroupVw>, ISysGroupService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IWFRepositoryManager _wfRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;

        public SysGroupService(IQueryRepository<SysGroup> queryRepository,
            IWFRepositoryManager wfRepositoryManager,
            IMapper mapper,
            IMainRepositoryManager mainRepositoryManager,
            ICurrentData session,
            ILocalizationService localization) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._wfRepositoryManager = wfRepositoryManager;
            this._mapper = mapper;
            this.session = session;
            this.localization = localization;
        }

        public async Task<IResult<SysGroupDto>> Add(SysGroupDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysGroupDto>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");

            try
            {
                entity.CreatedBy = session.UserId;
                entity.CreatedOn = DateTime.Now;
                entity.UserId = session.UserId;
                entity.FacilityId = session.FacilityId;

                var newEntity = await _mainRepositoryManager.SysGroupRepository.AddAndReturn(_mapper.Map<SysGroup>(entity));

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysGroupDto>(newEntity);

                return await Result<SysGroupDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {

                return await Result<SysGroupDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysGroupRepository.GetById(Id);
                if (item == null) return Result<SysGroupDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

                //check if there a user links with this group
                var users = await _mainRepositoryManager.SysUserRepository.GetAll(u => u.GroupsId != null && u.GroupsId.Equals(Id.ToString()) && u.IsDeleted == false);
                if (users.Any())
                {
                    return await Result<SysGroupDto>.FailAsync($"{localization.GetResource1("GroupLinkUsers")}");
                }

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                item.IsDeleted = true;

                _mainRepositoryManager.SysGroupRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                return await Result<SysGroupDto>.SuccessAsync(_mapper.Map<SysGroupDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysGroupDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysGroupRepository.GetById(Id);
                if (item == null) return Result<SysGroupDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

                //check if there a user links with this group
                var users = await _mainRepositoryManager.SysUserRepository.GetAll(u => u.GroupsId != null && u.GroupsId.Equals(Id.ToString()) && u.IsDeleted == false);
                if (users.Any())
                {
                    return await Result<SysGroupDto>.FailAsync($"{localization.GetResource1("GroupLinkUsers")}");
                }

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                item.IsDeleted = true;

                _mainRepositoryManager.SysGroupRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                return await Result<SysGroupDto>.SuccessAsync(_mapper.Map<SysGroupDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysGroupDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<SysGroupEditDto>> Update(SysGroupEditDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysGroupEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));
            try
            {
                var item = await _mainRepositoryManager.SysGroupRepository.GetById(entity.GroupId);
                if (item == null) return await Result<SysGroupEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

                _mapper.Map(entity, item);
                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                _mainRepositoryManager.SysGroupRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysGroupEditDto>.SuccessAsync(_mapper.Map<SysGroupEditDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysGroupEditDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<CopyGroupVM>> Copy(CopyGroupVM entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<CopyGroupVM>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");
            try
            {
                await _mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);
                //get old permissions for group, to delete them
                var oldPermissions = await _mainRepositoryManager.SysScreenPermissionRepository.GetAll(p => p.GroupId == entity.GroupId);
                foreach (var item in oldPermissions)
                {
                    _mainRepositoryManager.SysScreenPermissionRepository.Remove(item);
                    await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                }

                var copyingPermissions = await _mainRepositoryManager.SysScreenPermissionRepository.GetAll(p => p.GroupId == entity.GroupId2);
                foreach (var item in copyingPermissions)
                {
                    SysScreenPermissionDto newEntity = new SysScreenPermissionDto()
                    {
                        GroupId = entity.GroupId,
                        ScreenId = item.ScreenId,
                        ScreenShow = item.ScreenShow,
                        ScreenPrint = item.ScreenPrint,
                        ScreenAdd = item.ScreenAdd,
                        ScreenEdit = item.ScreenEdit,
                        ScreenDelete = item.ScreenDelete,
                        ScreenView = item.ScreenView,
                        ScreenExport = item.ScreenExport,
                        ScreenImport = item.ScreenImport,
                        ScreenApproval = item.ScreenApproval,
                        ScreenReject = item.ScreenReject,
                        UserId = session.UserId,
                    };
                    await _mainRepositoryManager.SysScreenPermissionRepository.Add(_mapper.Map<SysScreenPermission>(newEntity));
                    await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                }

                var automationForms = await _wfRepositoryManager.WfAppTypeRepository.GetAll(f => f.IsDeleted == false);
                foreach (var item in automationForms)
                {
                    if (!string.IsNullOrEmpty(item.SysGroupId))
                    {
                        item.SysGroupId = item.SysGroupId.Replace(" ", "");
                        var sysGrpIdArray = item.SysGroupId.Split(',');
                        //delete
                        if (sysGrpIdArray.Contains(entity.GroupId.ToString()))
                        {
                            sysGrpIdArray = Array.FindAll(sysGrpIdArray, x => x != entity.GroupId.ToString());

                            // Join the remaining numbers back into a string
                            string newSysGrpId = string.Join(",", sysGrpIdArray);
                            item.SysGroupId = newSysGrpId;
                        }

                        if (sysGrpIdArray.Contains(entity.GroupId2.ToString()))
                        {
                            item.SysGroupId += "," + entity.GroupId.ToString();
                        }
                    }

                    if (!string.IsNullOrEmpty(item.SysGroupQuery))
                    {
                        item.SysGroupQuery = item.SysGroupQuery.Replace(" ", ""); 
                        var sysGrpQueryArray = item.SysGroupQuery.Split(',');
                        //delete
                        if (sysGrpQueryArray.Contains(entity.GroupId.ToString()))
                        {
                            sysGrpQueryArray = Array.FindAll(sysGrpQueryArray, x => x != entity.GroupId.ToString());

                            // Join the remaining numbers back into a string
                            string newSysqQuery = string.Join(",", sysGrpQueryArray);
                            item.SysGroupQuery = newSysqQuery;
                        }

                        if (sysGrpQueryArray.Contains(entity.GroupId2.ToString()))
                        {
                            item.SysGroupQuery += "," + entity.GroupId.ToString();
                        }
                    }
                    _wfRepositoryManager.WfAppTypeRepository.Update(item);
                    await _wfRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                }

                await _mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);
                return await Result<CopyGroupVM>.SuccessAsync("item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<CopyGroupVM>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

		public async Task<IResult<List<SysGroupFilterDto>>> Search(SysGroupFilterDto filter, CancellationToken cancellationToken = default)
		{
			try
			{
				filter.SystemId ??= 0;
				var items = await _mainRepositoryManager.SysGroupRepository.GetAllVw(g => g.FacilityId == session.FacilityId
				&& g.IsDel == false
				&& (filter.SystemId == 0 || (g.SystemId == filter.SystemId))
				&& (string.IsNullOrEmpty(filter.GroupName) || (!string.IsNullOrEmpty(g.GroupName) && g.GroupName.Contains(filter.GroupName)))
				&& (string.IsNullOrEmpty(filter.GroupName2) || (!string.IsNullOrEmpty(g.GroupName2) && g.GroupName2.ToLower().Contains(filter.GroupName2)))
				);
				if (items != null)
				{
					var res = items;

					if (session.GroupId != 1)
					{
						var userGroup = await _mainRepositoryManager.SysGroupRepository.GetById(session.GroupId);
						if (userGroup != null && userGroup != null)
						{
							res = res.Where(g => g.SystemId > 0 && g.SystemId == userGroup.SystemId);
						}
						else
						{
							return await Result<List<SysGroupFilterDto>>.SuccessAsync(new List<SysGroupFilterDto>());
						}
					}
					var final = res.ToList();

					List<SysGroupFilterDto> results = new List<SysGroupFilterDto>();
					foreach (var item in final)
					{
						results.Add(new SysGroupFilterDto
						{
							GroupId = item.GroupId,
							GroupName = item.GroupName,
							GroupName2 = item.GroupName2,
							SystemId = item.SystemId,
							SystemName = item.SystemName,
							SystemName2 = item.SystemName2
						});
					}
					return await Result<List<SysGroupFilterDto>>.SuccessAsync(results);
				}
				return await Result<List<SysGroupFilterDto>>.SuccessAsync(new List<SysGroupFilterDto>());

			}
			catch (Exception ex)
			{
				return await Result<List<SysGroupFilterDto>>.FailAsync($"EXP in {this.GetType()}, Meesage: {ex.Message}");
			}
		}
	}
}
