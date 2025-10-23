using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Services.Main
{
    public class SysDepartmentService : GenericQueryService<SysDepartment, SysDepartmentDto, SysDepartmentVw>, ISysDepartmentService
    {

        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;
        private readonly IAccRepositoryManager _accRepositoryManager;
        private readonly IPMRepositoryManager _pmRepositoryManager;
        private readonly IHrRepositoryManager _hrRepositoryManager;


        public SysDepartmentService(IQueryRepository<SysDepartment> queryRepository,
            IMainRepositoryManager mainRepositoryManager,
            IMapper mapper,
            ICurrentData session,
            ILocalizationService localization,
            IAccRepositoryManager accRepositoryManager,
            IPMRepositoryManager pmRepositoryManager,
            IHrRepositoryManager hrRepositoryManager) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.session = session;
            this.localization = localization;
            this._accRepositoryManager = accRepositoryManager;
            this._pmRepositoryManager = pmRepositoryManager;
            this._hrRepositoryManager = hrRepositoryManager;
        }
        public async Task<IResult<SysDepartmentDto>> Add(SysDepartmentDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysDepartmentDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");

            try
            {
                entity.CatId ??= 0;
                entity.CityId ??= 0;
                entity.CcId ??= 0;
                entity.CcId2 ??= 0;
                entity.CcId3 ??= 0;
                entity.DepMangerId ??= 0;
                entity.EmpCode ??= "";
                entity.CcId4 ??= 0;
                entity.CcId5 ??= 0;
                entity.LevelNo ??= 0;
                entity.ProjectId ??= 0;
                entity.ProjectCode ??= "0";
                entity.CustomerId ??= 0;
                entity.BranchId ??= 0;
                entity.Fax ??= "";
                entity.Email ??= "";
                entity.FacilityId = session.FacilityId;
                entity.IsResidence = false;
                //get max code
                var allDepartments = await _mainRepositoryManager.SysDepartmentRepository.GetAll(d => d.TypeId == entity.TypeId && d.IsDeleted == false);
                if (allDepartments.Any())
                {
                    var maxDepartmentCode = allDepartments.Max(d => d.Code);
                    entity.Code = maxDepartmentCode + 1;
                }
                else
                    entity.Code = 1;

                //set CcId if CostCenterCode not nul
                if (!string.IsNullOrEmpty(entity.CostCenterCode))
                {
                    var costCenterId = await _accRepositoryManager.AccCostCenterRepository.getCostCenterByCode(entity.CostCenterCode, session.FacilityId);
                    if (costCenterId == 0)
                        return await Result<SysDepartmentDto>.FailAsync($"{localization.GetResource1("CostCenterNotExsists")}");
                    entity.CcId = costCenterId;
                }
                //العميل
                if (!string.IsNullOrEmpty(entity.CustomerCode))
                {
                    var CustomerId = await _mainRepositoryManager.SysCustomerRepository.GetCustomerId(entity.CustomerCode, 2);
                    if (CustomerId == 0)
                        return await Result<SysDepartmentDto>.FailAsync($"{localization.GetResource1("CustomerNotExsists")}");
                    entity.CustomerId = CustomerId;
                }

                //set DepMangerId if EmpCode not nul
                if (!string.IsNullOrEmpty(entity.EmpCode))
                {
                    var employeeId = await _hrRepositoryManager.HrEmployeeRepository.GetEmpId(session.FacilityId, entity.EmpCode);
                    if (employeeId == 0)
                        return await Result<SysDepartmentDto>.FailAsync($"{localization.GetHrResource("DeptLocationProjectDirector")}");
                    entity.DepMangerId = employeeId;
                }

                //set ProjectId if ProjectCode not nul
                if (!string.IsNullOrEmpty(entity.ProjectCode))
                {
                    var projectId = await _pmRepositoryManager.PMProjectsRepository.GetPMProjectsId(session.FacilityId, Convert.ToInt64(entity.ProjectCode));
                    if (projectId == 0)
                        return await Result<SysDepartmentDto>.FailAsync($"{localization.GetPMResource("ProjectNo")}");
                    entity.ProjectId = projectId;
                }

                entity.FacilityId = session.FacilityId;

                var item = _mapper.Map<SysDepartment>(entity);
                item.CreatedOn = DateHelper.GetCurrentDateTime();
                item.CreatedBy = session.UserId;
                item.Isdel = 0;
                item.IsDeleted = false;
                var newEntity = await _mainRepositoryManager.SysDepartmentRepository.AddAndReturn(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<SysDepartmentDto>(newEntity);

                return await Result<SysDepartmentDto>.SuccessAsync(entityMap, localization.GetMessagesResource("success"));
            }
            catch (Exception exc)
            {

                return await Result<SysDepartmentDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult<SysDepartmentEditDto>> Update(SysDepartmentEditDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysDepartmentEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));
            try
            {
                entity.CatId ??= 0;
                entity.CityId ??= 0;
                entity.CcId ??= 0;
                entity.CcId2 ??= 0;
                entity.CcId3 ??= 0;
                entity.DepMangerId ??= 0;
                entity.EmpCode ??= "";
                entity.CcId4 ??= 0;
                entity.CcId5 ??= 0;
                entity.LevelNo ??= 0;
                entity.ProjectId ??= 0;
                entity.ProjectCode ??= 0;
                entity.CustomerId ??= 0;
                entity.BranchId ??= 0;
                entity.Fax ??= "";
                entity.Email ??= "";
                entity.FacilityId = session.FacilityId;
                entity.IsResidence = false;

                var item = await _mainRepositoryManager.SysDepartmentRepository.GetById(entity.Id);
                if (item == null) return await Result<SysDepartmentEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));





                //set CcId if CostCenterCode not nul
                if (!string.IsNullOrEmpty(entity.CostCenterCode))
                {
                    var costCenterId = await _accRepositoryManager.AccCostCenterRepository.getCostCenterByCode(entity.CostCenterCode, session.FacilityId);
                    if (costCenterId == 0)
                        return await Result<SysDepartmentEditDto>.FailAsync($"{localization.GetResource1("CostCenterNotExsists")}");
                    entity.CcId = costCenterId;
                }


                //set DepMangerId if EmpCode not nul
                if (!string.IsNullOrEmpty(entity.EmpCode))
                {
                    var employeeId = await _hrRepositoryManager.HrEmployeeRepository.GetEmpId(session.FacilityId, entity.EmpCode);
                    if (employeeId == 0)
                        return await Result<SysDepartmentEditDto>.FailAsync($"{localization.GetHrResource("DeptLocationProjectDirector")}");
                    entity.DepMangerId = employeeId;
                }

                //set ProjectId if ProjectCode not nul
                if (entity.ProjectCode != 0)
                {
                    var projectId = await _pmRepositoryManager.PMProjectsRepository.GetPMProjectsId(session.FacilityId, entity.ProjectCode ?? 0);
                    if (projectId == 0)
                        return await Result<SysDepartmentEditDto>.FailAsync($"{localization.GetPMResource("ProjectNo")}");
                    entity.ProjectId = projectId;
                }
                item.TypeId = entity.TypeId;
                item.ParentId = entity.ParentId;
                item.StatusId = entity.StatusId;
                item.Name = entity.Name;
                item.Name2 = entity.Name2;
                item.Tel = entity.Tel;
                item.Fax = entity.Fax;
                item.Mobile = entity.Mobile;
                item.Email = entity.Email;
                item.CityId = entity.CityId;
                item.Note = entity.Note;
                item.IsShare = entity.IsShare;
                item.CcId = entity.CcId;
                item.DepMangerId = entity.DepMangerId ?? 0;
                item.ProjectId = entity.ProjectId;
                item.LevelNo = entity.LevelNo ?? 0;
                item.BranchId = entity.BranchId ?? 0;
                item.CustomerId = entity.CustomerId ?? 0;
                item.CcId = entity.CcId ?? 0;
                item.CcId2 = entity.CcId2 ?? 0;
                item.CcId3 = entity.CcId3 ?? 0;
                item.CcId4 = entity.CcId4 ?? 0;
                item.CcId5 = entity.CcId5 ?? 0;


                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateHelper.GetCurrentDateTime();
                _mainRepositoryManager.SysDepartmentRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                return await Result<SysDepartmentEditDto>.SuccessAsync(_mapper.Map<SysDepartmentEditDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysDepartmentEditDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }



        public async Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var GetDepartmentsEmployee = await _hrRepositoryManager.HrEmployeeRepository.GetAll(d => d.Isdel == false && d.IsDeleted == false && d.DeptId == Id);
                if (GetDepartmentsEmployee.Count() > 0) return Result<SysDepartmentDto>.Fail($"{localization.GetResource1("CantDeletedTheDepartment")}");

                var GetDepartmentsProjects = await _pmRepositoryManager.PMProjectsRepository.GetAll(d => d.IsDeleted == false && d.OwnerDeptId == Id);
                if (GetDepartmentsProjects.Count() > 0) return Result<SysDepartmentDto>.Fail($"{localization.GetResource1("CantDeletedTheDepartment")}");

                var item = await _mainRepositoryManager.SysDepartmentRepository.GetById(Id);
                if (item == null) return Result<SysDepartmentDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                item.IsDeleted = true;

                _mainRepositoryManager.SysDepartmentRepository.Update(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysDepartmentDto>.SuccessAsync(_mapper.Map<SysDepartmentDto>(item), localization.GetMessagesResource("DeletedSuccess"));
            }
            catch (Exception exp)
            {
                return await Result<SysDepartmentDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysDepartmentRepository.GetById(Id);
                if (item == null) return Result<SysDepartmentDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");
                var GetDepartmentsEmployee = await _hrRepositoryManager.HrEmployeeRepository.GetAll(d => d.Isdel == false && d.IsDeleted == false && d.DeptId == Id);
                if (GetDepartmentsEmployee.Count() > 0) return Result<SysDepartmentDto>.Fail($"{localization.GetResource1("CantDeletedTheDepartment")}");

                var GetDepartmentsProjects = await _pmRepositoryManager.PMProjectsRepository.GetAll(d => d.IsDeleted == false && d.OwnerDeptId == Id);
                if (GetDepartmentsProjects.Count() > 0) return Result<SysDepartmentDto>.Fail($"{localization.GetResource1("CantDeletedTheDepartment")}");
                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                item.IsDeleted = true;

                _mainRepositoryManager.SysDepartmentRepository.Update(item);

                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysDepartmentDto>.SuccessAsync(_mapper.Map<SysDepartmentDto>(item), localization.GetMessagesResource("DeletedSuccess"));
            }
            catch (Exception exp)
            {
                return await Result<SysDepartmentDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<List<string>>> GetchildDepartment(long DeptId, CancellationToken cancellationToken = default)
        {
            try
            {
                var getData = await _mainRepositoryManager.DbFunctionsRepository.HR_Get_childe_Department_Fn(DeptId);
                return await Result<List<string>>.SuccessAsync(getData, $"", 200);
                ;
            }
            catch (Exception exp)
            {

                return await Result<List<string>>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }
    }
}

