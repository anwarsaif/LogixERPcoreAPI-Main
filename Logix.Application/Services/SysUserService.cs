using System.Data;
using System.Globalization;
using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Helpers;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Logix.Application.Services.Main
{
    public class SysUserService : GenericQueryService<SysUser, SysUserDto, SysUserVw>, ISysUserService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;
        private readonly ISendSmsHelper sendSmsHelper;
        private readonly ISysConfigurationAppHelper sysConfigurationAppHelper;
        private readonly IWorkflowHelper workflowHelper;
        private readonly IHrRepositoryManager _hrRepositoryManager;

        public SysUserService(IQueryRepository<SysUser> queryRepository,
            IMapper mapper,
            IMainRepositoryManager mainRepositoryManager,
            ICurrentData session,
            ILocalizationService localization,
            ISendSmsHelper sendSmsHelper, ISysConfigurationAppHelper sysConfigurationAppHelper, IWorkflowHelper workflowHelper, IHrRepositoryManager hrRepositoryManager
            ) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.session = session;
            this.localization = localization;
            this.sendSmsHelper = sendSmsHelper;
            this.sysConfigurationAppHelper = sysConfigurationAppHelper;
            this.workflowHelper = workflowHelper;
            this._hrRepositoryManager = hrRepositoryManager;
        }

        public async Task<IResult<SysUserDto>> Add(SysUserDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysUserDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");
            try
            {
                entity.CreatedBy = session.UserId;

                //string add = await _mainRepositoryManager.SysUserRepository.AddByProcedure(entity);
                string add = await _mainRepositoryManager.StoredProceduresRepository.AddUserByProcedure(entity);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var newEntity = await _mainRepositoryManager.SysUserRepository.GetOne(u => u.Id == Convert.ToInt64(add));
                var entityMap = _mapper.Map<SysUserDto>(newEntity);

                return await Result<SysUserDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysUserDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var item = await _mainRepositoryManager.SysUserRepository.GetById(Id);
                if (item == null) return Result<SysUserDto>.Fail($"{localization.GetMessagesResource("NoIdInDelete")}");

                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                item.IsDeleted = true;
                item.Isdel = true;

                _mainRepositoryManager.SysUserRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysUserDto>.SuccessAsync(_mapper.Map<SysUserDto>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysUserDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<SysUserEditDto>> Update(SysUserEditDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysUserEditDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));
            try
            {
                entity.ModifiedBy = session.UserId;

                //string add = await _mainRepositoryManager.SysUserRepository.EditByProcedure(entity);
                string add = await _mainRepositoryManager.StoredProceduresRepository.EditUserByProcedure(entity);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var newEntity = await _mainRepositoryManager.SysUserRepository.GetOne(u => u.Id == entity.Id);
                var entityMap = _mapper.Map<SysUserEditDto>(newEntity);

                return await Result<SysUserEditDto>.SuccessAsync(entityMap);
            }
            catch (Exception exp)
            {
                return await Result<SysUserEditDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }


        public async Task<IResult<SysUserLoginTimeDto>> UpdateUserLoginTime(SysUserLoginTimeDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysUserLoginTimeDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));
            try
            {
                var item = await _mainRepositoryManager.SysUserRepository.GetById(entity.Id);

                if (item == null) return await Result<SysUserLoginTimeDto>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

                item.Ips = entity.Ips;
                if (!string.IsNullOrEmpty(entity.TimeFrom))
                    item.TimeFrom = TimeSpan.ParseExact(entity.TimeFrom, "hh\\:mm", CultureInfo.InvariantCulture);
                else
                    item.TimeFrom = null;

                if (!string.IsNullOrEmpty(entity.TimeTo))
                    item.TimeTo = TimeSpan.ParseExact(entity.TimeTo, "hh\\:mm", CultureInfo.InvariantCulture);
                else
                    item.TimeTo = null;

                _mainRepositoryManager.SysUserRepository.Update(item);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<SysUserLoginTimeDto>.SuccessAsync(entity, localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<SysUserLoginTimeDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<DataTable>> GetUserMailBox(int ReferralsToUserId, int ReferralsToDepId, CancellationToken cancellationToken = default)
        {
            try
            {
                var items = await _mainRepositoryManager.SysUserRepository.GetUserMailBox(ReferralsToUserId, ReferralsToDepId);
                return await Result<DataTable>.SuccessAsync(items, localization.GetMessagesResource("success"));
            }
            catch (Exception exp)
            {
                return await Result<DataTable>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }


        public async Task<string?> GetUserPosting(long userId)
        {
            var AccTransfer = await _mainRepositoryManager.SysUserRepository.GetUserPosting(userId);
            return AccTransfer;
        }

        public async Task<string> GetDecryptUserPassword(long userId)
        {
            try
            {
                var password = await _mainRepositoryManager.StoredProceduresRepository.GetDecryptUserPassword(userId);
                return password;
            }
            catch
            {
                return "";
            }
        }

        public async Task<IResult<long>> GetPackageId()
        {
            try
            {
                long packageId = 0;
                var items = await _mainRepositoryManager.SysPackageRepository.GetAll(x => x.Id);
                if (items.Any())
                    packageId = items.FirstOrDefault();

                return await Result<long>.SuccessAsync(packageId);
            }
            catch
            {
                return await Result<long>.SuccessAsync(0);
            }
        }

        public async Task<IResult> ActivateTwoFactor(string UsersId, int TwoFactorType, CancellationToken cancellationToken = default)
        {
            try
            {
                var usersIdList = UsersId.Split(',');
                var getUsers = await _mainRepositoryManager.SysUserRepository.GetAll(x => usersIdList.Contains(x.Id.ToString()) && x.IsDeleted == false);
                foreach (var user in getUsers)
                {
                    user.TwoFactor = true;
                    user.TwoFactorType = TwoFactorType;
                }

                _mainRepositoryManager.SysUserRepository.UpdateAll(getUsers);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result.SuccessAsync();
            }
            catch (Exception exp)
            {
                return await Result.FailAsync(exp.Message);
            }
        }

        public async Task<IResult> DeactivateTwoFactor(string UsersId, CancellationToken cancellationToken = default)
        {
            try
            {
                var usersIdList = UsersId.Split(',');
                var getUsers = await _mainRepositoryManager.SysUserRepository.GetAll(x => usersIdList.Contains(x.Id.ToString()) && x.IsDeleted == false);
                foreach (var user in getUsers)
                {
                    user.TwoFactor = false;
                }

                _mainRepositoryManager.SysUserRepository.UpdateAll(getUsers);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result.SuccessAsync();
            }
            catch (Exception exp)
            {
                return await Result.FailAsync(exp.Message);
            }

        }


        public async Task<IResult<string>> CreateUserRequst(CreateUserRequstDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<string>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");
            try
            {
                long FacilityId = 1;
                entity.AppTypeId ??= 0;
                long? appId = 0;
                var NewUser = new SysUserDto();
                string GroupID = "0";

                // getEmpDataIf Exist Before
                var GetEmployee = await _mainRepositoryManager.InvestEmployeeRepository.GetOne(e => e.IdNo == entity.IdNo && e.BirthDate == entity.BirthDate && e.Isdel == false && e.IsDeleted == false && e.StatusId == 1);

                if (GetEmployee == null)
                    return await Result<string>.SuccessAsync("تاكد من البيانات المدخلة  الهوية و تاريخ الميلاد ");

                if (GetEmployee.FacilityId > 0)
                {
                    FacilityId = (long)GetEmployee.FacilityId;
                }

                // Check If USer Exists
                var USerExists = await _mainRepositoryManager.SysUserRepository.GetAll(e => e.UserName == entity.UserName && e.Isdel == false && e.IsDeleted == false);

                if (USerExists.Count() > 0)
                    return await Result<string>.SuccessAsync("اسم المستخدم موجود مسبقا");

                // Check User Existing  هل يوجد مستخدم سابق للموظف
                var CheckUserExisting = await _mainRepositoryManager.SysUserRepository.GetAll(e => e.EmpId == GetEmployee.Id && e.Isdel == false && e.IsDeleted == false);

                if (CheckUserExisting.Count() > 0)
                    return await Result<string>.SuccessAsync("يوجد مستخدم سابق للموظف");



                var GroupsId = await sysConfigurationAppHelper.GetValue(105, FacilityId);

                if (!string.IsNullOrEmpty(GroupsId))
                {
                    GroupID = GroupsId;
                }
                NewUser.UserName = entity.UserName;
                NewUser.UserFullname = GetEmployee.EmpName;
                NewUser.Email = GetEmployee.Email;
                NewUser.StringUserPassword = entity.StringUserPassword;
                NewUser.UserTypeId = 1;
                NewUser.IsDeleted = false;
                NewUser.Isdel = false;
                NewUser.FacilityId = GetEmployee.FacilityId;
                NewUser.EmpId = (int?)GetEmployee.Id;
                NewUser.GroupsId = GroupID;
                NewUser.UserPkId = GetEmployee.BranchId;
                NewUser.UserType2Id = 0;
                NewUser.ProjectsId = "";
                NewUser.SupId = 0;
                NewUser.CusId = 0;
                NewUser.SalesType = 0;
                NewUser.Isupdate = false;
                NewUser.CandId = 0;


                string add = await _mainRepositoryManager.StoredProceduresRepository.AddUserByProcedure(NewUser);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                // Begin Of Transaction
                await _mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);

                //  Disable_UserByID
                var AddedUser = await _mainRepositoryManager.SysUserRepository.GetOne(u => u.Id == Convert.ToInt64(add));
                if (AddedUser != null)
                {
                    AddedUser.Enable = 2;
                    AddedUser.ModifiedOn = DateHelper.GetCurrentDateTime();
                }
                //  ارسال الى سير العمل

                var GetApp_ID = await workflowHelper.Send(GetEmployee.Id, 1813, entity.AppTypeId);
                appId = GetApp_ID;

                // Add To SysCreateUserRequst Table
                var AgreementList = await sysConfigurationAppHelper.GetValue(226, 1);
                var newSysCreateUserRequst = new SysCreateUserRequst
                {
                    IsDeleted = false,
                    AppId = (int?)appId,
                    Approve = entity.CheckApprove,
                    EmpId = (int?)GetEmployee.Id,
                    Files = entity.FileUrl,
                    Email = GetEmployee.Email,
                    CreatedOn = DateHelper.GetCurrentDateTime(),
                    Password = entity.StringUserPassword,
                    Mobile = GetEmployee.Mobile,
                    UserName = entity.UserName,
                    AgreementList = AgreementList
                };
                await _mainRepositoryManager.SysCreateUserRequstRepository.Add(newSysCreateUserRequst);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);



                await _mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);
                // End Of Transaction
                return await Result<string>.SuccessAsync("تم ارسال الطلب بنجاح");
            }
            catch (Exception exc)
            {
                return await Result<string>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult<string>> DisableUserByUserName(string UserName, int language = 1, CancellationToken cancellationToken = default)
        {
            try
            {

                // Check User Existing  هل يوجد مستخدم 
                var CheckUser = await _mainRepositoryManager.SysUserRepository.GetOne(e => e.UserName == UserName && e.Isdel == false && e.IsDeleted == false && e.UserTypeId == 1);

                if (CheckUser == null)
                {
                    var message = language == 1 ? "  غير مصرح " : "Incorrect username";
                    return await Result<string>.FailAsync(message);
                }
                CheckUser.Enable = 0;
                CheckUser.ModifiedBy = 1;
                CheckUser.ModifiedOn = DateHelper.GetCurrentDateTime();
                _mainRepositoryManager.SysUserRepository.Update(CheckUser);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                var Successmessage = language == 1 ? "تم تعطيل المستخدم بسبب تجاوز الحد المسموح  لعدد مرات تسجيل الدخول " : "User disabled because logon limit exceeded";

                return await Result<string>.SuccessAsync(Successmessage);
            }
            catch (Exception exc)
            {
                return await Result<string>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<long?> CheckPassword(long userId, string? password)
        {
            long? UserCount = await _mainRepositoryManager.SysUserRepository.CheckPassword(userId, password);
            return UserCount ?? 0;

        }

        public async Task<bool> ChangePassword(long userId, string? newPassword, string email)
        {
            var UserCount = await _mainRepositoryManager.SysUserRepository.ChangePassword(userId, newPassword, email);
            return UserCount;
        }

        public async Task<bool> UpdateOTP(string otp, long userId)
        {
            var UserCount = await _mainRepositoryManager.SysUserRepository.UpdateOTP(otp, userId);
            return UserCount;
        }


        public async Task<IResult<bool>> ApproveAgreement(CancellationToken cancellationToken = default)
        {
            try
            {
                var getUser = await _mainRepositoryManager.SysUserRepository.GetOne(x => x.Id == session.UserId && x.Isdel == false && x.IsDeleted == false);

                if (getUser == null)
                {
                    return await Result<bool>.FailAsync("the user Not Found");

                }
                getUser.IsAgree = true;
                _mainRepositoryManager.SysUserRepository.Update(getUser);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<bool>.SuccessAsync(true, "");
            }
            catch (Exception exp)
            {
                return await Result<bool>.FailAsync(exp.Message);
            }
        }

        public async Task<IResult> UpdateTwoFactor(long userId, CancellationToken cancellationToken = default)
        {
            try
            {

                var getUsers = await _mainRepositoryManager.SysUserRepository.GetOne(x => x.Id == userId && x.IsDeleted == false);

                getUsers.TwoFactor = false;
                _mainRepositoryManager.SysUserRepository.Update(getUsers);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result.SuccessAsync();
            }
            catch (Exception exp)
            {
                return await Result.FailAsync(exp.Message);
            }
        }

        #region ================================اضافة مستخدم من الأكتيف ديركتوري===========================================


        public async Task<IResult<string>> AddActiveDirectoryUser(ActiveDirectoryUserAddDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<string>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");
            try
            {

                var NewUser = new SysUserDto();

                //check If Email Exist
                var getemployeeByEmail = await _hrRepositoryManager.HrEmployeeRepository.GetAllVw(e => e.Email == entity.Email && e.Isdel == false && e.IsDeleted == false);

                if (getemployeeByEmail.Any())
                    return await Result<string>.FailAsync("هذا الموظف موجود مسبقا");


                // Check If USer Exists
                var USerExists = await _mainRepositoryManager.SysUserRepository.GetAll(e => e.UserName == entity.userName && e.Isdel == false && e.IsDeleted == false);

                if (USerExists.Any())
                    return await Result<string>.FailAsync("اسم المستخدم موجود مسبقا");

                var GroupsId = entity.GroupId.ToString();
                //  add Employee
                var Employee = await InsertHREmp(entity.empName, entity.Email, GroupsId);

                NewUser.UserName = entity.userName;
                NewUser.UserFullname = entity.empName;
                NewUser.Email = entity.Email;
                NewUser.StringUserPassword = entity.password;
                NewUser.IsDeleted = false;
                NewUser.Isdel = false;
                NewUser.EmpId = (int?)Employee.Id;
                NewUser.GroupsId = GroupsId;
                NewUser.ProjectsId = "";
                /*===================================Default Values==============================*/
                NewUser.UserTypeId = 1;
                NewUser.UserPkId = 1;
                NewUser.FacilityId = (int)session.FacilityId;
                NewUser.SupId = 0;
                NewUser.CusId = 0;
                NewUser.UserType2Id = 0;
                NewUser.SalesType = 0;
                NewUser.Isupdate = false;
                NewUser.CandId = 0;

                string add = await _mainRepositoryManager.StoredProceduresRepository.AddUserByProcedure(NewUser);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                return await Result<string>.SuccessAsync($"{localization.GetResource1("AddSuccess")}");
            }
            catch (Exception exc)
            {
                return await Result<string>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        private async Task<InvestEmployee> InsertHREmp(string empName, string email, string groupId, CancellationToken cancellationToken = default)
        {
            var NumberingByContarctType = await sysConfigurationAppHelper.GetValue(161, 1);

            // Fetching employees based on conditions
            var investEmployees = await _mainRepositoryManager.InvestEmployeeRepository.GetAll(e => e.IsSub == false &&
                (NumberingByContarctType == "1" ? e.ContractTypeId == 0 : true));

            // Calculate new employee ID
            long maxEmpId = Convert.ToInt64(investEmployees.Max(b => Convert.ToInt64(b.EmpId))) + 1;
            var newEmpId = maxEmpId.ToString();

            var newEmp = new Domain.Main.InvestEmployee
            {
                EmpId = newEmpId,
                EmpName = empName,
                EmpName2 = empName,
                BranchId = 1,
                JobCatagoriesId = 0,
                StatusId = 10,
                CheckDevice = false,
                CheckDeviceActive = false,
                FacilityId = (int)session.FacilityId,
                Email = email,
                EmpCode2 = "",
                IdNo = "",
                CreatedBy = session.UserId,
                CreatedOn = DateHelper.GetCurrentDateTime(),



                GosiSalary = 0,
                BankId = 0,
                GosiBiscSalary = 0,
                GosiHouseAllowance = 0,
                GosiAllowanceCommission = 0,
                GosiOtherAllowances = 0,
                GosiRateFacility = 0,
                GosiType = 0,
                Vacation2DaysYear = 0,
                JobId = 0,
                TrialStatusId = 0,
                SalaryGroupId = 0,
                AttendanceType = 0,
                CcId = 0,
                SponsorsId = 0,
                QualificationId = 0,
                SpecializationId = 0,
                Manager2Id = 0,
                Manager3Id = 0,
                IsJoinedGosiafterJuly32024 = false,
                UserId = 0,
                JobType = 0,
                Salary = 0
            };

            var addedEmployee = await _mainRepositoryManager.InvestEmployeeRepository.AddAndReturn(newEmp);
            await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

            return addedEmployee;
        }



        #endregion


        public async Task<IResult<List<SysUserFilterDto>>> Search(SysUserFilterDto filter, CancellationToken cancellationToken = default)
        {
            try
            {
                var branchsIds = session.Branches.Split(',');
                var items = await _mainRepositoryManager.SysUserRepository.GetAllVw(u => u.IsDeleted == false
                && u.UserTypeId == 1
                && (filter.Enable == null || (u.Enable != null && u.Enable == filter.Enable))
                && (string.IsNullOrEmpty(filter.GroupsId) || (!string.IsNullOrEmpty(u.GroupsId) && u.GroupsId.Equals(filter.GroupsId)))
                && (string.IsNullOrEmpty(filter.UserFullname) || (!string.IsNullOrEmpty(u.UserFullname) && u.UserFullname.Contains(filter.UserFullname)))
                && (string.IsNullOrEmpty(filter.EmpCode) || (!string.IsNullOrEmpty(u.EmpCode) && u.EmpCode.Equals(filter.EmpCode)))
                && (filter.FacilityId == null || (u.FacilityId != null && u.FacilityId == filter.FacilityId))
                && (filter.UserTypeId == null || (u.UserTypeId != null && u.UserTypeId == filter.UserTypeId))
                && (string.IsNullOrEmpty(filter.UserName) || (!string.IsNullOrEmpty(u.UserName) && u.UserName.Contains(filter.UserName)))
                );
                if (items != null)
                {
                    var res = items.AsQueryable();

                    if (filter.BranchId > 0)
                        res = res.Where(u => u.UserPkId == filter.BranchId);
                    else
                        res = res.Where(u => branchsIds.Contains(u.UserPkId.ToString()));

                    var final = res.ToList();
                    List<SysUserFilterDto> results = new List<SysUserFilterDto>();
                    foreach (var item in final)
                    {
                        if (session.GroupId != 1)
                        {
                            var getSysId = await _mainRepositoryManager.SysGroupRepository.GetOne(g => g.SystemId, g => g.GroupId == session.GroupId);
                            if (getSysId != null)
                            {
                                var getSysIdByUsrGrp = await _mainRepositoryManager.SysGroupRepository.GetOne(g => g.SystemId, g => g.GroupId == Convert.ToInt32(item.GroupsId));
                                if (getSysIdByUsrGrp != null)
                                {
                                    if (getSysId != getSysIdByUsrGrp)
                                        continue;
                                }
                            }
                        }

                        string lastLgn = "";
                        if (item.LastLogin != null)
                        {
                            DateTime currentDate = DateTime.Now;
                            // difference between dates
                            TimeSpan difference = currentDate - item.LastLogin.Value;

                            // difference in days, hours and minutes
                            lastLgn = $"{difference.Days} يوم - {difference.Hours} ساعة - {difference.Minutes} دقيقة";
                        }
                        SysUserFilterDto result = new SysUserFilterDto()
                        {
                            Id = item.UserId,
                            UserFullname = item.UserFullname,
                            UserEmail = item.UserEmail,
                            UserName = item.UserName,
                            BraName = item.BraName,
                            BraName2 = item.BraName2,
                            EnableChkbx = item.Enable == 1 ? true : false,
                            GroupsId = item.GroupsId,
                            GroupName = item.GroupName,
                            lastLogin = lastLgn
                        };
                        results.Add(result);
                    }
                    return await Result<List<SysUserFilterDto>>.SuccessAsync(results);
                }
                return await Result<List<SysUserFilterDto>>.SuccessAsync(new List<SysUserFilterDto>());
            }
            catch (Exception ex)
            {
                return await Result<List<SysUserFilterDto>>.FailAsync($"EXP in {this.GetType()}, Meesage: {ex.Message}");
            }
        }

        public async Task<IResult<List<SysUsersLoginsVm>>> GetUsersLoginsRpt(SysUsersLoginsVm filter, CancellationToken cancellationToken = default)
        {
            try
            {
                filter.UserId ??= 0; filter.GroupId ??= 0;

                var userVw = await _mainRepositoryManager.SysUserRepository.GetAllVw(u => u.Isdel == false
                && (filter.UserId == 0 || (u.UserId == filter.UserId))
                && (filter.GroupId == 0 || (!string.IsNullOrEmpty(u.GroupsId) && u.GroupsId == filter.GroupId.ToString()))
                );
                if (userVw.Any())
                {
                    if (!string.IsNullOrEmpty(filter.StartDate) && !string.IsNullOrEmpty(filter.EndDate))
                    {
                        DateTime startDate = DateTime.ParseExact(filter.StartDate ?? "", "yyyy/MM/dd", CultureInfo.InvariantCulture);
                        DateTime endDate = DateTime.ParseExact(filter.EndDate ?? "", "yyyy/MM/dd", CultureInfo.InvariantCulture);

                        userVw = userVw.Where(r => r.CreatedOn != null
                        && r.CreatedOn.Value.Date >= startDate && r.CreatedOn.Value.Date <= endDate);
                    }

                    var empVw = await _hrRepositoryManager.HrEmployeeRepository.GetAllVw(x => string.IsNullOrEmpty(filter.EmpCode)
                    || x.EmpId == filter.EmpCode);

                    var allLogTimes = await _mainRepositoryManager.SysUserLogTimeRepository.GetAll();

                    List<SysUsersLoginsVm> results = new();
                    foreach (var item in userVw)
                    {
                        var employee = empVw.Where(x => x.Id == item.EmpId).FirstOrDefault();
                        if (!string.IsNullOrEmpty(filter.EmpCode) && employee == null)
                            continue;
                        else
                            employee ??= new();

                        var firstLogin = allLogTimes.Where(x => x.UserId == item.UserId).Min(x => x.LoginTime);
                        var lastLogin = allLogTimes.Where(x => x.UserId == item.UserId).Max(x => x.LoginTime);

                        SysUsersLoginsVm result = new()
                        {
                            UserName = item.UserName,
                            EmpName = employee.EmpName,
                            EmpName2 = employee.EmpName2,
                            UserId = item.UserId,
                            FirstLogin = firstLogin != null ? firstLogin.Value.ToString("dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "",
                            LastLogin = lastLogin != null ? lastLogin.Value.ToString("dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "",
                            EmpCode = employee.EmpId,
                            IdNo = employee.IdNo,
                            BraName = item.BraName,
                            BraName2 = item.BraName2,
                            DepName = employee.DepName,
                            DepName2 = employee.DepName2,
                            LocationName = employee.LocationName,
                            LocationName2 = employee.LocationName2,
                        };
                        results.Add(result);
                    }

                    return await Result<List<SysUsersLoginsVm>>.SuccessAsync(results);
                }
                return await Result<List<SysUsersLoginsVm>>.SuccessAsync(new List<SysUsersLoginsVm>());
            }
            catch (Exception ex)
            {
                return await Result<List<SysUsersLoginsVm>>.FailAsync($"EXP in {this.GetType()}, Meesage: {ex.Message}");
            }
        }

        public async Task<IResult<List<SysUsersLoginsVm>>> GetUsersPermissionsRpt(SysUsersLoginsVm filter, CancellationToken cancellationToken = default)
        {
            try
            {
                filter.UserId ??= 0; filter.GroupId ??= 0;

                var userVw = await _mainRepositoryManager.SysUserRepository.GetAllVw(u => u.Isdel == false
                && (filter.UserId == 0 || (u.UserId == filter.UserId))
                && (filter.GroupId == 0 || (!string.IsNullOrEmpty(u.GroupsId) && u.GroupsId == filter.GroupId.ToString()))
                );
                if (userVw.Any())
                {
                    if (!string.IsNullOrEmpty(filter.StartDate) && !string.IsNullOrEmpty(filter.EndDate))
                    {
                        DateTime startDate = DateTime.ParseExact(filter.StartDate ?? "", "yyyy/MM/dd", CultureInfo.InvariantCulture);
                        DateTime endDate = DateTime.ParseExact(filter.EndDate ?? "", "yyyy/MM/dd", CultureInfo.InvariantCulture);

                        userVw = userVw.Where(r => r.CreatedOn != null
                        && r.CreatedOn.Value.Date >= startDate && r.CreatedOn.Value.Date <= endDate);
                    }

                    var empVw = await _hrRepositoryManager.HrEmployeeRepository.GetAllVw(x => string.IsNullOrEmpty(filter.EmpCode)
                    || x.EmpId == filter.EmpCode);

                    List<SysUsersLoginsVm> results = new();
                    foreach (var item in userVw)
                    {
                        var employee = empVw.Where(x => x.Id == item.EmpId).FirstOrDefault();
                        if (!string.IsNullOrEmpty(filter.EmpCode) && employee == null)
                            continue;
                        else
                            employee ??= new();

                        SysUsersLoginsVm result = new()
                        {
                            UserName = item.UserName,
                            EmpName = employee.EmpName,
                            EmpName2 = employee.EmpName2,
                            UserId = item.UserId,
                            EmpCode = employee.EmpId,
                            IdNo = employee.IdNo,
                            BraName = item.BraName,
                            BraName2 = item.BraName2,
                            DepName = employee.DepName,
                            DepName2 = employee.DepName2,
                            LocationName = employee.LocationName,
                            LocationName2 = employee.LocationName2,

                            CreatedOn = item.CreatedOn != null ? item.CreatedOn.Value.ToString("dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "",
                            ModifiedOn = item.ModifiedOn != null ? item.ModifiedOn.Value.ToString("dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : ""
                        };
                        result.NotEnableModifiedOn = item.Enable == 0 ? result.ModifiedOn : "";
                        results.Add(result);
                    }

                    return await Result<List<SysUsersLoginsVm>>.SuccessAsync(results);
                }
                return await Result<List<SysUsersLoginsVm>>.SuccessAsync(new List<SysUsersLoginsVm>());
            }
            catch (Exception ex)
            {
                return await Result<List<SysUsersLoginsVm>>.FailAsync($"EXP in {this.GetType()}, Meesage: {ex.Message}");
            }
        }
    }
}
