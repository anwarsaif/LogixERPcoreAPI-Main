using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Services.Main
{
    public class SysScreenPermissionService : GenericQueryService<SysScreenPermission, SysScreenPermissionDto, SysScreenPermissionVw>, ISysScreenPermissionService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;

        public SysScreenPermissionService(IQueryRepository<SysScreenPermission> queryRepository,
            IMapper mapper,
            IMainRepositoryManager mainRepositoryManager,
            ICurrentData session
            ) : base(queryRepository, mapper)
        {
            this._mapper = mapper;
            this._mainRepositoryManager = mainRepositoryManager;
            this.session = session;
        }

        public async Task<IResult<SysScreenPermissionDto>> Add(SysScreenPermissionDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysScreenPermissionDto>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");

            try
            {
                entity.UserId = session.UserId;
                var newEntity = await _mainRepositoryManager.SysScreenPermissionRepository.AddAndReturn(_mapper.Map<SysScreenPermission>(entity));

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysScreenPermissionDto>(newEntity);

                return await Result<SysScreenPermissionDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {

                return await Result<SysScreenPermissionDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult<SysScreenPermissionDto>> GetByScreenAndGroup(long screenId, int groupId, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysScreenPermissionRepository.GetByScreenAndGroup(screenId, groupId);
                if (item == null) return Result<SysScreenPermissionDto>.Fail($"--- there is no Data with this screen {screenId} and group: {groupId}---");
                var newEntity = _mapper.Map<SysScreenPermissionDto>(item);
                return await Result<SysScreenPermissionDto>.SuccessAsync(newEntity, "");

            }
            catch (Exception ex)
            {
                return Result<SysScreenPermissionDto>.Fail($"Exp in get screen permissions by Id: {ex.Message} --- {(ex.InnerException != null ? "InnerExp: " + ex.InnerException.Message : "no inner")} .");
            }
        }


        public Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<SysScreenPermissionDto>> Update(SysScreenPermissionDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<IEnumerable<SysScreenPermissionDtoVM>>> Update(IEnumerable<SysScreenPermissionDtoVM> entities, CancellationToken cancellationToken = default)
        {
            if (entities == null) return await Result<IEnumerable<SysScreenPermissionDtoVM>>.FailAsync($"Error in Add of: {this.GetType()}, the passed entity is NULL !!!.");

            try
            {
                foreach (var item in entities)
                {
                    //check if group is exist (get by screenId, groupId), so update
                    //else add
                    var permission = await _mainRepositoryManager.SysScreenPermissionRepository.GetOne(p => p.ScreenId == item.ScreenId && p.GroupId != null && p.GroupId == item.GroupId);
                    var obj = new SysScreenPermissionDto
                    {
                        ScreenId = item.ScreenId,
                        UserId = session.UserId,
                        GroupId = item.GroupId,
                        ScreenShow = item.ScreenShow,
                        ScreenAdd = item.ScreenAdd,
                        ScreenEdit = item.ScreenEdit,
                        ScreenDelete = item.ScreenDelete,
                        ScreenPrint = item.ScreenPrint,
                        ScreenView = item.ScreenView,
                        ScreenImport = item.ScreenImport,
                        ScreenExport = item.ScreenExport,
                        ScreenApproval = item.ScreenApproval,
                        ScreenReject = item.ScreenReject,
                    };

                    if (permission == null)
                    {
                        //Add
                        await _mainRepositoryManager.SysScreenPermissionRepository.Add(_mapper.Map<SysScreenPermission>(obj));
                        await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                    }
                    else
                    {
                        //Update
                        obj.PriveId = permission.PriveId;
                        _mapper.Map(obj, permission);
                        _mainRepositoryManager.SysScreenPermissionRepository.Update(permission);
                        await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                    }
                }

                return await Result<IEnumerable<SysScreenPermissionDtoVM>>.SuccessAsync(entities, "items updated successfully");
            }
            catch (Exception exc)
            {
                return await Result<IEnumerable<SysScreenPermissionDtoVM>>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult<List<UserPermissionSearchVm>>> GetUserPermissionReport(UserPermissionSearchVm entity, CancellationToken cancellationToken = default)
        {
            try
            {
                entity.UserId ??= 0; entity.SystemId ??= 0; entity.ScreenId ??= 0;
                var item = await _mainRepositoryManager.SysScreenPermissionRepository.GetUserPermissionReport(entity);
                return await Result<List<UserPermissionSearchVm>>.SuccessAsync(item);
            }
            catch (Exception ex)
            {
                return Result<List<UserPermissionSearchVm>>.Fail($"Exp in get screen permissions by Id: {ex.Message} --- {(ex.InnerException != null ? "InnerExp: " + ex.InnerException.Message : "no inner")} .");
            }
        }
        public async Task<PaginatedResult<List<UserPermissionSearchVm>>> GetUserPermissionReportAsync(UserPermissionSearchVm obj, int take = 10, int? lastSeenId = null)
        {
            try
            {
                obj.UserId ??= 0;
                obj.SystemId ??= 0;
                obj.ScreenId ??= 0;
                var result = await _mainRepositoryManager.SysScreenPermissionRepository.GetUserPermissionReportAsync(obj, take, lastSeenId);

                return result;
            }
            catch (Exception ex)
            {
                return PaginatedResult<List<UserPermissionSearchVm>>.Fail($"Exception in GetUserPermissionReportAsync: {ex.Message}" + (ex.InnerException != null ? $" --- InnerException: {ex.InnerException.Message}" : ""));
            }
        }


    }
}
