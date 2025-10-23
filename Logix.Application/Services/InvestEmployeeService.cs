using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.HR;
using Logix.Application.DTOs.HR.EmployeeDto;
using Logix.Application.DTOs.Main;
using Logix.Application.Helpers;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.HR;
using Logix.Domain.Main;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using InvestEmployee = Logix.Domain.Main.InvestEmployee;

namespace Logix.Application.Services.Main
{
    public class InvestEmployeeService : GenericQueryService<InvestEmployee, InvestEmployeeDto, InvestEmployeeVvw>, IInvestEmployeeService
    {
        private readonly IMainRepositoryManager mainRepositoryManager;
        private readonly IPMRepositoryManager pmRepositoryManager;
        private readonly IHrRepositoryManager hrRepositoryManager;
        private readonly IAccRepositoryManager accRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;
        private readonly ISysConfigurationAppHelper configurationHelper;

        public InvestEmployeeService(IQueryRepository<InvestEmployee> queryRepository,
            IMapper mapper,
            IMainRepositoryManager mainRepositoryManager,
            IPMRepositoryManager pmRepositoryManager,
            IHrRepositoryManager hrRepositoryManager,
            IAccRepositoryManager accRepositoryManager,
            ICurrentData session,
            ISysConfigurationAppHelper configurationHelper,
            ILocalizationService localization) : base(queryRepository, mapper)
        {
            this.mainRepositoryManager = mainRepositoryManager;
            _mapper = mapper;
            this.pmRepositoryManager = pmRepositoryManager;
            this.hrRepositoryManager = hrRepositoryManager;
            this.accRepositoryManager = accRepositoryManager;
            this.session = session;
            this.localization = localization;
            this.configurationHelper = configurationHelper;
        }

        public async Task<IResult<InvestEmployeeDto>> Add(InvestEmployeeDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<InvestEmployeeDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");

            try
            {
                //chek if not auto numbering
                if (entity.AutoNumbering == false && string.IsNullOrEmpty(entity.EmpId))
                {
                    return await Result<InvestEmployeeDto>.FailAsync(localization.GetResource1("EmployeeIsNumber"));
                }


                //Auto numbering
                if (string.IsNullOrEmpty(entity.EmpId))
                {
                    var investEmployees = await mainRepositoryManager.InvestEmployeeRepository.GetAll(e => e.IsSub == false);
                    long maxEmpId = Convert.ToInt64(investEmployees.Max(b => Convert.ToInt64(b.EmpId))) + 1;
                    entity.EmpId = maxEmpId.ToString();
                }
                else
                {
                    var investEmployees = await mainRepositoryManager.InvestEmployeeRepository.GetAll(e => e.EmpId == entity.EmpId && e.IsDeleted == false);
                    if (investEmployees.Any())
                    {
                        return await Result<InvestEmployeeDto>.FailAsync("رقم الموظف موجود مسبقا");
                    }
                }
                entity.FacilityId = (int)session.FacilityId;
                entity.CreatedBy = session.UserId;
                entity.CreatedOn = DateTime.Now;
                entity.JobType = 2;
                entity.JobCatagoriesId = 0;
                entity.StatusId = 1;


                var newEntity = await mainRepositoryManager.InvestEmployeeRepository.AddAndReturn(_mapper.Map<InvestEmployee>(entity));

                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<InvestEmployeeDto>(newEntity);

                return await Result<InvestEmployeeDto>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<InvestEmployeeDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }

        }

        public async Task<IResult<InvestEmployeeDto>> Add(InvestEmployeeAddDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<InvestEmployeeDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");

            try
            {
                long managerId = 0;
                long Manager2Id = 0;
                long Manager3Id = 0;
                //Auto numbering
                if (string.IsNullOrEmpty(entity.EmpId))
                {
                    var GetEmployees = await mainRepositoryManager.InvestEmployeeRepository.GetAll(e => e.IsSub == false);
                    long maxEmpId = Convert.ToInt64(GetEmployees.Max(b => Convert.ToInt64(b.EmpId))) + 1;
                    entity.EmpId = maxEmpId.ToString();
                }
                var investEmployees = await mainRepositoryManager.InvestEmployeeRepository.GetAll(e => e.EmpId == entity.EmpId && e.IsDeleted == false);
                if (investEmployees.Count() > 0)
                {
                    return await Result<InvestEmployeeDto>.FailAsync("رقم الموظف موجود مسبقا");
                }

                if (entity.AutoNumbering == true && entity.GroupId <= 0)
                {
                    return await Result<InvestEmployeeDto>.FailAsync(localization.GetMessagesResource("Selectgrouppermissions"));


                }
                string ccIdCheck = await configurationHelper.GetValue(228, session.FacilityId);
                if (ccIdCheck == "1")
                {
                    if (string.IsNullOrEmpty(entity.CostCenterCode))
                    {
                        return await Result<InvestEmployeeDto>.FailAsync("قم بإدخال مركز التكلفة ");
                    }

                }
                string empName2 = await configurationHelper.GetValue(270, session.FacilityId);
                if (empName2 == "1")
                {
                    if (string.IsNullOrEmpty(entity.EmpName2))
                    {
                        return await Result<InvestEmployeeDto>.FailAsync("قم بإدخال اسم الموظف بالانجليزي  ");
                    }
                    if (string.IsNullOrEmpty(entity.ManagerId))
                    {
                        return await Result<InvestEmployeeDto>.FailAsync("قم بإدخال مدير المباشر ");

                    }

                    if (!(entity.Manager2Id > 0))
                    {
                        return await Result<InvestEmployeeDto>.FailAsync("قم بإدخال مدير اداري 1 ");

                    }
                    if (!(entity.Manager3Id > 0))
                    {
                        return await Result<InvestEmployeeDto>.FailAsync("قم بإدخال مدير اداري 2 ");

                    }
                    if (string.IsNullOrEmpty(entity.Email))
                    {
                        return await Result<InvestEmployeeDto>.FailAsync("قم بإدخال البريد الالكتروني ");
                    }
                    if (string.IsNullOrEmpty(entity.Email2))
                    {
                        return await Result<InvestEmployeeDto>.FailAsync("قم بإدخال البريد الالكتروني الشخصي");
                    }
                    if (!(entity.AttendanceType > 0))
                    {
                        return await Result<InvestEmployeeDto>.FailAsync("قم بتحديد نوع التحضير ");

                    }
                    if (!(entity.QualificationId > 0))
                    {
                        return await Result<InvestEmployeeDto>.FailAsync("قم بتحديد المؤهل ");
                    }
                    if (string.IsNullOrEmpty(entity.BirthDate))
                    {
                        return await Result<InvestEmployeeDto>.FailAsync("قم إدخال تاريخ الميلاد ");
                    }
                    if (!(entity.ReligionId > 0))
                    {
                        return await Result<InvestEmployeeDto>.FailAsync("قم بتحديد الديانة ");
                    }
                }


                await mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);
                if (!string.IsNullOrEmpty(entity.ManagerId))
                {
                    var checkMangerId = await mainRepositoryManager.InvestEmployeeRepository.GetOne(m => m.EmpId == entity.ManagerId && m.Isdel == false && m.IsDeleted == false);
                    if (checkMangerId == null)
                    {
                        return await Result<InvestEmployeeDto>.FailAsync(" المدير المباشر  غير موجود في قائمة الموظفين");
                    }
                    else
                    {
                        managerId = checkMangerId.Id;
                    }
                    if (checkMangerId.StatusId == 2)
                    {
                        return await Result<InvestEmployeeDto>.FailAsync(localization.GetHrResource("EmpNotActive"));
                    }
                }
                if (string.IsNullOrEmpty(entity.Doappointment))
                {

                    return await Result<InvestEmployeeDto>.FailAsync("قم إدخال تاريخ التعيين  ");
                }
                //يتبقى لنا هنا معرفة التاريخ الصحيح حسب سياسة التوظيف
                var DoappointmentDate = DateHelper.StringToDate(entity.Doappointment);
                if (DoappointmentDate.Year <= 1900 || DoappointmentDate.Year >= 2100 || DoappointmentDate.Month < 1 || DoappointmentDate.Month > 12 || DoappointmentDate.Day < 1 || DoappointmentDate.Day > 31)
                {
                    return await Result<InvestEmployeeDto>.FailAsync("تاريخ التعيين غير صالح");

                }
                if (string.IsNullOrEmpty(entity.ContractExpiryDate))
                {

                    return await Result<InvestEmployeeDto>.FailAsync("قم بإدخال تاريخ انتهاء العقد   ");
                }
                //يتبقى لنا هنا معرفة التاريخ الصحيح حسب سياسة التوظيف
                var ContractExpiryDateDate = DateHelper.StringToDate(entity.ContractExpiryDate);
                if (ContractExpiryDateDate.Year <= 1900 || ContractExpiryDateDate.Year >= 2100 || ContractExpiryDateDate.Month < 1 || ContractExpiryDateDate.Month > 12 || ContractExpiryDateDate.Day < 1 || ContractExpiryDateDate.Day > 31)
                {
                    return await Result<InvestEmployeeDto>.FailAsync("تاريخ انتهاء العقد غير صالح");

                }
                //ChkByID_NO
                if (string.IsNullOrEmpty(entity.IdNo))
                {
                    return await Result<InvestEmployeeDto>.FailAsync("قم بادخال رقم الهوية  ");

                }

                var ChkByIDNO = await hrRepositoryManager.HrEmployeeRepository.GetAll(e => e.IdNo == entity.IdNo && e.StatusId != 2 && e.IsDeleted == false);
                if (ChkByIDNO.Count() > 0)
                {
                    return await Result<InvestEmployeeDto>.FailAsync("رقم الهوية للموظف موجودة مسبقاً");
                }
                //ChkByIBAN
                //if (string.IsNullOrEmpty(entity.Iban))
                //{
                //    return await Result<InvestEmployeeDto>.FailAsync("قم بادخال رقم الأيبان  ");

                //}

                var ChkByIBAN = await hrRepositoryManager.HrEmployeeRepository.GetAll(e => e.Iban == entity.Iban && e.StatusId != 2 && e.IsDeleted == false);
                if (ChkByIDNO.Count() > 0)
                {
                    return await Result<InvestEmployeeDto>.FailAsync("رقم الأيبان للموظف موجودة مسبقاً");
                }

                if (!(entity.Location > 0))
                {
                    return await Result<InvestEmployeeDto>.FailAsync("قم باختيار الموقع ");
                }
                if (!(entity.JobCatagoriesId > 0))
                {
                    return await Result<InvestEmployeeDto>.FailAsync("قم باختيار نوع الوظيفة ");
                }
                if (!(entity.ProgramId > 0))
                {
                    return await Result<InvestEmployeeDto>.FailAsync("قم باختيار المشروع  ");
                }

                //التشييك على الضوابط التشغيلية
                var projectsId = await mainRepositoryManager.SysDepartmentRepository.GetOne(p => p.Id == entity.Location);
                var Allow = await pmRepositoryManager.PMProjectsOperationalControlRepository.GetAll(a => a.IsDeleted == false && a.ProjectId == projectsId.ProjectId && a.JobCatagoriesId == entity.JobCatagoriesId);
                var AllowInJob = Allow.Sum(a => a.CountOfEmplyee ?? 0);
                var Current = await hrRepositoryManager.HrEmployeeRepository.GetAll(c => (c.StatusId == 1 || c.StatusId == 10) && c.IsDeleted == false && c.Location == entity.Location && c.JobCatagoriesId == entity.JobCatagoriesId);
                var CurrentInjob = 0;
                CurrentInjob = Current.Count();
                if (AllowInJob != 0 && AllowInJob < CurrentInjob + 1)
                {
                    var resultMessage = "عدد الموظفين في الموقع للوظيفة المحددة تتجاوز العدد المسموح به في الضوابط التشغيلية"
                        + "  العدد المسموح به في الوظيفة للموقع  :" + AllowInJob.ToString()
                        + "عدد الموظفين على راس العمل / تحت الإجراء  :" + CurrentInjob.ToString();

                    return await Result<InvestEmployeeDto>.FailAsync(resultMessage);
                }

                // التشييك على رواتب الوظائف والبرامج
                decimal maxSalary = 0;
                var jobsSalary = await pmRepositoryManager.PMJobsSalaryVwRepository.GetOne(j => j.IsDeleted == false && j.ProjectId == projectsId.ProjectId && j.JobCatagoriesId == entity.JobCatagoriesId && j.ProgramId == entity.ProgramId);
                if (jobsSalary != null)
                {
                    maxSalary = jobsSalary.Maxsalary ?? 0;
                }
                if (maxSalary != 0 && maxSalary < entity.TotalSalary)
                {
                    var resultMessage = "راتب الموظف في الموقع للوظيفة المحددة تتجاوز الراتب المسموح به في الضوابط التشغيلية"
                        + "اعلى راتب للموظف في الموقع يجب الأيتجاوز الحد الأعلى وهو :" + maxSalary.ToString();
                    return await Result<InvestEmployeeDto>.FailAsync(resultMessage);
                }

                // التشييك على البدلات
                var checkAllowance = await hrRepositoryManager.HrSettingRepository.GetOne(s => s.FacilityId == session.FacilityId);
                if (checkAllowance == null || checkAllowance.HousingAllowance == 0 || checkAllowance.TransportAllowance == 0 || checkAllowance.MobileAllowance == 0 || checkAllowance.BadalatAllowance == 0 || checkAllowance.GosiDeduction == 0)
                {
                    return await Result<InvestEmployeeDto>.FailAsync("قم بضبط إعدادات البدلات والحسميات اولاً");

                }
                if (entity.Vacation2DaysYear == null || entity.Vacation2DaysYear == 0)
                {
                    entity.Vacation2DaysYear = 0;
                }
                //  ادخال بيانات التأمينات مباشرة من التسجيل للموظف
                if (entity.NationalityId == 1)
                {
                    entity.GosiRateFacility = 11.75m;
                    entity.GosiType = 1;

                }
                else
                {
                    entity.GosiRateFacility = 2m;
                    entity.GosiType = 2;
                }
                //  التأكد من قيمة الراتب ثم اسناد قيم بناءَ عليه
                if (entity.Salary != null)
                {
                    entity.GosiSalary = entity.Salary;
                    entity.GosiBiscSalary = entity.Salary;

                }
                if (entity.HousingAllowance != null && entity.HousingAllowance > 0)
                {
                    entity.GosiSalary += entity.HousingAllowance;
                    entity.GosiHouseAllowance += entity.HousingAllowance;



                }

                if (session.CalendarType == "2")
                {
                    var TrialExpiryDate = "";
                    DateHelper.Initialize(mainRepositoryManager);

                    var GDate = await DateHelper.DateFormattYYYYMMDD_H_G(entity.Doappointment);

                    if (entity.TrialType == 1)
                    {
                        int month = entity.TrialCount ?? 0;
                        var newExpiryDate = DateHelper.StringToDate(GDate).AddMonths(month);
                        TrialExpiryDate = newExpiryDate.ToString("yyyy/mm/dd", CultureInfo.InvariantCulture);

                    }
                    else if (entity.TrialType == 2)
                    {

                        int Week = entity.TrialCount ?? 0;
                        var newExpiryDate = DateHelper.StringToDate(GDate).AddDays(Week * 7);
                        TrialExpiryDate = newExpiryDate.ToString("yyyy/mm/dd", CultureInfo.InvariantCulture);

                    }
                    else if (entity.TrialType == 3)
                    {
                        int days = entity.TrialCount ?? 0;
                        var newExpiryDate = DateHelper.StringToDate(GDate).AddDays(days);
                        TrialExpiryDate = newExpiryDate.ToString("yyyy/mm/dd", CultureInfo.InvariantCulture);
                    }
                    entity.TrialExpiryDate = TrialExpiryDate;
                }

                var getCostCenterByCode = await accRepositoryManager.AccCostCenterRepository.GetOne(x => x.IsDeleted == false && x.IsActive == true && entity.FacilityId == x.FacilityId && x.CostCenterCode == entity.CostCenterCode);
                if (entity.Manager2Id > 0)
                {
                    var checkManger2Id = await mainRepositoryManager.InvestEmployeeRepository.GetOne(m => m.EmpId == entity.Manager2Id.ToString() && m.Isdel == false && m.IsDeleted == false);
                    if (checkManger2Id == null)
                    {
                        return await Result<InvestEmployeeDto>.FailAsync(" المدير الاداري1  غير موجود في قائمة الموظفين");
                    }
                    else
                    {
                        Manager2Id = checkManger2Id.Id;
                    }
                    if (checkManger2Id.StatusId == 2)
                    {
                        return await Result<InvestEmployeeDto>.FailAsync(localization.GetHrResource("EmpNotActive"));
                    }
                }
                if (entity.Manager3Id > 0)
                {
                    var checkManger3Id = await mainRepositoryManager.InvestEmployeeRepository.GetOne(m => m.EmpId == entity.Manager3Id.ToString() && m.Isdel == false && m.IsDeleted == false);
                    if (checkManger3Id == null)
                    {
                        return await Result<InvestEmployeeDto>.FailAsync(" المدير الاداري2  غير موجود في قائمة الموظفين");
                    }
                    else
                    {
                        Manager3Id = checkManger3Id.Id;
                    }
                    if (checkManger3Id.StatusId == 2)
                    {
                        return await Result<InvestEmployeeDto>.FailAsync(localization.GetHrResource("EmpNotActive"));
                    }
                }
                var addEntity = _mapper.Map<InvestEmployee>(entity);
                addEntity.FacilityId = (int)session.FacilityId;
                addEntity.CreatedBy = session.UserId;
                addEntity.CreatedOn = DateTime.Now;
                addEntity.UserId = session.UserId;
                addEntity.StatusId = 10;
                addEntity.TrialStatusId = 1;
                addEntity.ReasonStatus = localization.GetHrResource("Reason_Status_New");
                addEntity.ManagerId = managerId;
                addEntity.Manager2Id = Manager2Id;
                addEntity.Manager3Id = Manager3Id;
                addEntity.CcId = getCostCenterByCode.CcId;
                if (entity.LevelId <= 0) addEntity.LevelId = 0;

                if (entity.DegreeId <= 0) addEntity.DegreeId = 0;
                var newEntity = await mainRepositoryManager.InvestEmployeeRepository.AddAndReturn(addEntity);

                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                // اضافة بدل السكن
                if (entity.HousingAllowance > 0)
                {
                    var newAllowanceDeductionTranc = new HrAllowanceDeduction
                    {

                        EmpId = newEntity.Id,
                        AdId = checkAllowance.HousingAllowance,
                        TypeId = 1,
                        Rate = 0,
                        Amount = entity.HousingAllowance,
                        FixedOrTemporary = 1,
                        CreatedBy = session.UserId,
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,

                    };
                    await hrRepositoryManager.HrAllowanceDeductionRepository.Add(newAllowanceDeductionTranc);

                    await hrRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);


                }


                // اضافة بدل المواصلات
                if (entity.TransportAllowance > 0)
                {
                    var newAllowanceDeductionTranc = new HrAllowanceDeduction
                    {

                        EmpId = newEntity.Id,
                        AdId = checkAllowance.TransportAllowance,
                        TypeId = 1,
                        Rate = 0,
                        Amount = entity.TransportAllowance,
                        FixedOrTemporary = 1,
                        CreatedBy = session.UserId,
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,

                    };
                    await hrRepositoryManager.HrAllowanceDeductionRepository.Add(newAllowanceDeductionTranc);

                    await hrRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);


                }

                // اضافة بدل الاتصال
                if (entity.MobileAllowance > 0)
                {
                    var newAllowanceDeductionTranc = new HrAllowanceDeduction
                    {

                        EmpId = newEntity.Id,
                        AdId = checkAllowance.MobileAllowance,
                        TypeId = 1,
                        Rate = 0,
                        Amount = entity.MobileAllowance,
                        FixedOrTemporary = 1,
                        CreatedBy = session.UserId,
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,

                    };
                    await hrRepositoryManager.HrAllowanceDeductionRepository.Add(newAllowanceDeductionTranc);

                    await hrRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);


                }

                // اضافة بدل اخرى
                if (entity.OtherAllowances > 0)
                {
                    var newAllowanceDeductionTranc = new HrAllowanceDeduction
                    {

                        EmpId = newEntity.Id,
                        AdId = checkAllowance.BadalatAllowance,
                        TypeId = 1,
                        Rate = 0,
                        Amount = entity.OtherAllowances,
                        FixedOrTemporary = 1,
                        CreatedBy = session.UserId,
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,

                    };
                    await hrRepositoryManager.HrAllowanceDeductionRepository.Add(newAllowanceDeductionTranc);

                    await hrRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);


                }
                // اضافة حسم التأمينات الإجتماعية
                if (entity.GOSIDeduction > 0)
                {
                    var newAllowanceDeductionTranc = new HrAllowanceDeduction
                    {

                        EmpId = newEntity.Id,
                        AdId = checkAllowance.GosiDeduction,
                        TypeId = 2,
                        Rate = 0,
                        Amount = entity.GOSIDeduction,
                        FixedOrTemporary = 1,
                        CreatedBy = session.UserId,
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,

                    };
                    await hrRepositoryManager.HrAllowanceDeductionRepository.Add(newAllowanceDeductionTranc);

                    await hrRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);


                }

                //اضافة الوردية
                if (entity.ShiftId > 0)
                {
                    var newHRAttShiftEmployee = new HrAttShiftEmployee
                    {

                        EmpId = newEntity.Id,
                        CreatedBy = session.UserId,
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,
                        ShitId = entity.ShiftId,
                        BeginDate = entity.ContractData,
                        EndDate = "2027/01/01"


                    };
                    await hrRepositoryManager.HrAttShiftEmployeeRepository.Add(newHRAttShiftEmployee);

                    await hrRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                }


                //هل نقوم بانشاء مستخدم
                if (entity.ChkCreateUser)
                {
                    var newUser = new SysUserDto();
                    newUser.UserPkId = newEntity.BranchId;
                    newUser.UserTypeId = 1;
                    newUser.UserName = newEntity.EmpId;
                    newUser.Email = newEntity.Email;
                    newUser.UserFullname = newEntity.EmpName;
                    newUser.EmpId = (int?)newEntity.Id;
                    newUser.FacilityId = newEntity.FacilityId;
                    newUser.GroupsId = entity.GroupId.ToString();
                    newUser.StringUserPassword = newEntity.IdNo;
                    newUser.CreatedBy = session.UserId;
                    string add = await mainRepositoryManager.StoredProceduresRepository.AddUserByProcedure(newUser);
                    await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                }

                await mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);


                var entityMap = _mapper.Map<InvestEmployeeDto>(newEntity);


                return await Result<InvestEmployeeDto>.SuccessAsync(entityMap, "item added successfully");

            }
            catch (Exception exc)
            {
                return await Result<InvestEmployeeDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }

        }


        public async Task<IResult<InvestEmployeeDto2>> AddFromMainSys(InvestEmployeeDto2 entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<InvestEmployeeDto2>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");
            try
            {
                //Auto numbering
                if (string.IsNullOrEmpty(entity.EmpId))
                {
                    var investEmployees = await mainRepositoryManager.InvestEmployeeRepository.GetAll(e => e.IsSub == false);
                    long maxEmpId = Convert.ToInt64(investEmployees.Max(b => Convert.ToInt64(b.EmpId))) + 1;
                    entity.EmpId = maxEmpId.ToString();
                }
                else
                {
                    var investEmployees = await mainRepositoryManager.InvestEmployeeRepository.GetAll(e => e.EmpId == entity.EmpId && e.IsDeleted == false);
                    if (investEmployees.Any())
                    {
                        return await Result<InvestEmployeeDto2>.FailAsync($"{localization.GetMainResource("TheEmployeeNumberAlreadyExists")}");
                    }
                }
                var item = _mapper.Map<InvestEmployee>(entity);

                item.FacilityId = (int)session.FacilityId;
                item.UserId = session.UserId;
                item.CreatedBy = session.UserId;
                item.CreatedOn = DateTime.Now;
                item.JobType = 2;
                item.JobCatagoriesId = 0;
                item.StatusId = 1;
                item.CheckDevice = false;
                item.CheckDeviceActive = false;

                var newEntity = await mainRepositoryManager.InvestEmployeeRepository.AddAndReturn(item);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                var entityMap = _mapper.Map<InvestEmployeeDto2>(newEntity);
                return await Result<InvestEmployeeDto2>.SuccessAsync(entityMap, "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<InvestEmployeeDto2>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult<InvestEmployeeDto2>> UpdateFromMainSys(InvestEmployeeDto2 entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<InvestEmployeeDto2>.FailAsync($"{localization.GetMessagesResource("NoIdInUpdate")}");
            try
            {
                var item = await mainRepositoryManager.InvestEmployeeRepository.GetById(entity.Id ?? 0);

                if (item == null) return await Result<InvestEmployeeDto2>.FailAsync(localization.GetMessagesResource("NoIdInUpdate"));

                //if user clear the empId
                if (string.IsNullOrEmpty(entity.EmpId))
                {
                    return await Result<InvestEmployeeDto2>.FailAsync($"{localization.GetMainResource("PleaseEnterEmployeeNumFirst")}");
                }
                else
                {
                    var investEmployees = await mainRepositoryManager.InvestEmployeeRepository.GetAll(e => e.Id != entity.Id && e.EmpId == entity.EmpId && e.IsDeleted == false);
                    if (investEmployees.Any())
                    {
                        return await Result<InvestEmployeeDto2>.FailAsync($"{localization.GetMainResource("TheEmployeeNumberAlreadyExists")}");
                    }
                }

                _mapper.Map(entity, item);
                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;

                mainRepositoryManager.InvestEmployeeRepository.Update(item);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<InvestEmployeeDto2>.SuccessAsync(_mapper.Map<InvestEmployeeDto2>(item), localization.GetMessagesResource("success"));
            }
            catch (Exception exc)
            {
                return await Result<InvestEmployeeDto2>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public async Task<IResult<InvestEmployeeDto>> FastAdd(EmployeeFastAddDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<InvestEmployeeDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");

            try
            {
                //Auto numbering
                if (string.IsNullOrEmpty(entity.EmpId))
                {
                    var GetEmployees = await mainRepositoryManager.InvestEmployeeRepository.GetAll(e => e.IsSub == false);
                    long maxEmpId = Convert.ToInt64(GetEmployees.Max(b => Convert.ToInt64(b.EmpId))) + 1;
                    entity.EmpId = maxEmpId.ToString();
                }

                var investEmployees = await mainRepositoryManager.InvestEmployeeRepository.GetAll(e => e.EmpId == entity.EmpId && e.IsDeleted == false);
                if (investEmployees.Count() > 0)
                {
                    return await Result<InvestEmployeeDto>.FailAsync("رقم الموظف موجود مسبقا");
                }

                var checkIdNo = await hrRepositoryManager.HrEmployeeRepository.GetAll(e => e.IdNo == entity.IdNo && e.StatusId != 2 && e.IsDeleted == false);
                if (checkIdNo.Count() > 0)
                {
                    return await Result<InvestEmployeeDto>.FailAsync($"رقم الهوية للموظف موجودة مسبقاً");
                }

                var item = _mapper.Map<InvestEmployee>(entity);
                item.JobType = entity.JobType ?? 2;
                item.JobCatagoriesId = entity.JobCatagoriesId ?? 0;
                item.StatusId = 10;
                item.FacilityId = (int)session.FacilityId;
                item.NationalityId = entity.NationalityId;


                var newEntity = await mainRepositoryManager.InvestEmployeeRepository.AddAndReturn(item);

                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var entityMap = _mapper.Map<InvestEmployeeDto>(newEntity);

                return await Result<InvestEmployeeDto>.SuccessAsync(entityMap, "تم عملية الاضافة بنجاح");
            }
            catch (Exception exc)
            {
                return await Result<InvestEmployeeDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }

        }

        public async Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            try
            {

                //check if this employee has records in AccJournalDetailesVw
                var chkHasJournalRecord = await accRepositoryManager.AccJournalDetaileRepository.GetAllFromView(j => j.FlagDelete == false && j.ParentId == 8 && j.ReferenceNo == Id);
                if (chkHasJournalRecord.Count() > 0)
                {
                    return await Result<InvestEmployeeDto>.FailAsync(localization.GetMessagesResource("NOemployeedeleted"));
                }

                var item = await mainRepositoryManager.InvestEmployeeRepository.GetById(Id);
                if (item == null) return await Result<InvestEmployeeDto>.FailAsync($"{localization.GetMessagesResource("NoIdInDelete")}");



                item.IsDeleted = true;
                item.ModifiedOn = DateTime.Now;
                item.ModifiedBy = session.UserId;

                await mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);

                mainRepositoryManager.InvestEmployeeRepository.Update(item);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                //add notification
                string notMsg = "";
                notMsg = " رقم الموظف : " + item.EmpId;
                notMsg += " اسم الموظف : " + item.EmpName;
                string notUrl = "/Apps/Main/Employee/Employee?ID=" + Id;

                //get Sys_Notifications_Setting to get users that the notificaton to it
                var getSysNotifiSetting = await mainRepositoryManager.SysNotificationsSettingRepository.GetAll(s => s.IsDeleted == false && s.ScreenId == 959 && s.ActionTypeId == 3);
                if (getSysNotifiSetting.Any())
                {
                    foreach (var notificationsSetting in getSysNotifiSetting)
                    {
                        //insert into Sys_Notifications
                        SysNotification notification = new SysNotification()
                        {
                            UserId = Convert.ToInt64(notificationsSetting.Users),
                            MsgTxt = notificationsSetting.MsgTxt + " " + notMsg,
                            CreatedBy = session.UserId,
                            CreatedOn = DateTime.Now,
                            Url = notUrl
                        };
                        await mainRepositoryManager.SysNotificationRepository.Add(notification);
                        await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                    }
                }
                await mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);
                return await Result<InvestEmployeeDto>.SuccessAsync(_mapper.Map<InvestEmployeeDto>(item), localization.GetResource1("DeleteSuccess"));
            }
            catch (Exception exp)
            {
                return await Result<InvestEmployeeDto>.FailAsync($"EXP in Remove at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<InvestEmployeeEditDto>> Update(InvestEmployeeEditDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<EmployeeMainInfoDto>> UpdateMain(EmployeeMainInfoDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<EmployeeMainInfoDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");



            try
            {
                var item = await mainRepositoryManager.InvestEmployeeRepository.GetById(entity.Id);

                if (item == null) return await Result<EmployeeMainInfoDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");
                _mapper.Map(entity, item);

                mainRepositoryManager.InvestEmployeeRepository.Update(item);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<EmployeeMainInfoDto>.SuccessAsync(_mapper.Map<EmployeeMainInfoDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                return await Result<EmployeeMainInfoDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<EmployeeContactInfoDto>> UpdateContact(EmployeeContactInfoDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<EmployeeContactInfoDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");


            try
            {
                var item = await mainRepositoryManager.InvestEmployeeRepository.GetById(entity.Id);

                if (item == null) return await Result<EmployeeContactInfoDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");
                _mapper.Map(entity, item);

                mainRepositoryManager.InvestEmployeeRepository.Update(item);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<EmployeeContactInfoDto>.SuccessAsync(_mapper.Map<EmployeeContactInfoDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                return await Result<EmployeeContactInfoDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<EmployeeAdditionalPropsDto>> UpdateAdditionalProps(EmployeeAdditionalPropsDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<EmployeeAdditionalPropsDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");



            try
            {
                var item = await mainRepositoryManager.InvestEmployeeRepository.GetById(entity.Id);

                if (item == null) return await Result<EmployeeAdditionalPropsDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");
                _mapper.Map(entity, item);

                mainRepositoryManager.InvestEmployeeRepository.Update(item);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<EmployeeAdditionalPropsDto>.SuccessAsync(_mapper.Map<EmployeeAdditionalPropsDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                return await Result<EmployeeAdditionalPropsDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }


        }


        public async Task<IResult<EmployeeContractInfoDto>> UpdateContract(EmployeeContractInfoDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<EmployeeContractInfoDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");

            try
            {
                var item = await mainRepositoryManager.InvestEmployeeRepository.GetById(entity.Id);

                if (item == null) return await Result<EmployeeContractInfoDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");
                _mapper.Map(entity, item);

                mainRepositoryManager.InvestEmployeeRepository.Update(item);

                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<EmployeeContractInfoDto>.SuccessAsync(_mapper.Map<EmployeeContractInfoDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                return await Result<EmployeeContractInfoDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<EmployeeDependentsDto>> UpdateDependents(EmployeeDependentsDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<EmployeeDependentsDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");

            try
            {
                var item = await mainRepositoryManager.InvestEmployeeRepository.GetById(entity.Id);

                if (item == null) return await Result<EmployeeDependentsDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");
                _mapper.Map(entity, item);

                mainRepositoryManager.InvestEmployeeRepository.Update(item);

                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<EmployeeDependentsDto>.SuccessAsync(_mapper.Map<EmployeeDependentsDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                return await Result<EmployeeDependentsDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<EmployeeFollowersDto>> UpdateFollowers(EmployeeFollowersDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<EmployeeFollowersDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");


            try
            {
                var item = await mainRepositoryManager.InvestEmployeeRepository.GetById(entity.Id);

                if (item == null) return await Result<EmployeeFollowersDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");
                _mapper.Map(entity, item);

                mainRepositoryManager.InvestEmployeeRepository.Update(item);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<EmployeeFollowersDto>.SuccessAsync(_mapper.Map<EmployeeFollowersDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                return await Result<EmployeeFollowersDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<EmployeeJobInfoDto>> UpdateJob(EmployeeJobInfoDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<EmployeeJobInfoDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");


            try
            {
                var item = await mainRepositoryManager.InvestEmployeeRepository.GetById(entity.Id);

                if (item == null) return await Result<EmployeeJobInfoDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");
                entity.ManagerId ??= 0;
                entity.Manager2Id ??= 0;
                entity.Manager3Id ??= 0;
                if (entity.ManagerId != 0)
                {
                    var checkMangerId = await mainRepositoryManager.InvestEmployeeRepository.GetOne(m => m.EmpId == entity.ManagerId.ToString() && m.Isdel == false);
                    if (checkMangerId == null)
                    {
                        return await Result<EmployeeJobInfoDto>.FailAsync(localization.GetMessagesResource("DirectManagerNotFound"));
                    }
                    else
                    {
                        if (checkMangerId.StatusId == 2)
                        {
                            return await Result<EmployeeJobInfoDto>.FailAsync(localization.GetHrResource("EmpNotActive"));
                        }
                        entity.ManagerId = checkMangerId.Id;
                    }
                }
                if (entity.Manager2Id != 0)
                {
                    var checkManger2Id = await mainRepositoryManager.InvestEmployeeRepository.GetOne(m => m.EmpId == entity.Manager2Id.ToString() && m.Isdel == false);
                    if (checkManger2Id == null)
                    {
                        return await Result<EmployeeJobInfoDto>.FailAsync(localization.GetMessagesResource("AdministrativeManager1NotFound"));
                    }
                    else
                    {
                        if (checkManger2Id.StatusId == 2)
                        {
                            return await Result<EmployeeJobInfoDto>.FailAsync(localization.GetHrResource("EmpNotActive"));
                        }
                        entity.Manager2Id = checkManger2Id.Id;
                    }
                }
                if (entity.Manager3Id != 0)
                {
                    var checkManger3Id = await mainRepositoryManager.InvestEmployeeRepository.GetOne(m => m.EmpId == entity.Manager3Id.ToString() && m.Isdel == false);
                    if (checkManger3Id == null)
                    {
                        return await Result<EmployeeJobInfoDto>.FailAsync(localization.GetMessagesResource("AdministrativeManager2NotFound"));
                    }
                    else
                    {
                        if (checkManger3Id.StatusId == 2)
                        {
                            return await Result<EmployeeJobInfoDto>.FailAsync(localization.GetHrResource("EmpNotActive"));
                        }
                        entity.Manager3Id = checkManger3Id.Id;
                    }
                }

                _mapper.Map(entity, item);

                mainRepositoryManager.InvestEmployeeRepository.Update(item);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<EmployeeJobInfoDto>.SuccessAsync(_mapper.Map<EmployeeJobInfoDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                return await Result<EmployeeJobInfoDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<EmployeeMedicalInsuranceDto>> UpdateMedicalInsurance(EmployeeMedicalInsuranceDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<EmployeeMedicalInsuranceDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");


            try
            {
                var item = await mainRepositoryManager.InvestEmployeeRepository.GetById(entity.Id);

                if (item == null) return await Result<EmployeeMedicalInsuranceDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");
                _mapper.Map(entity, item);

                mainRepositoryManager.InvestEmployeeRepository.Update(item);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<EmployeeMedicalInsuranceDto>.SuccessAsync(_mapper.Map<EmployeeMedicalInsuranceDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                return await Result<EmployeeMedicalInsuranceDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<EmployeePreparingDto>> UpdatePreparing(EmployeePreparingDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<EmployeePreparingDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");


            try
            {
                var item = await mainRepositoryManager.InvestEmployeeRepository.GetById(entity.Id);

                if (item == null) return await Result<EmployeePreparingDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");
                _mapper.Map(entity, item);

                mainRepositoryManager.InvestEmployeeRepository.Update(item);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<EmployeePreparingDto>.SuccessAsync(_mapper.Map<EmployeePreparingDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                return await Result<EmployeePreparingDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<EmployeeSalaryInfoDto>> UpdateSalary(EmployeeSalaryInfoDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<EmployeeSalaryInfoDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");



            try
            {
                var item = await mainRepositoryManager.InvestEmployeeRepository.GetById(entity.Id);

                if (item == null) return await Result<EmployeeSalaryInfoDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");
                _mapper.Map(entity, item);

                await mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);

                mainRepositoryManager.InvestEmployeeRepository.Update(item);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                if (entity.allowances != null && entity.allowances.Any())
                {
                    foreach (var singleItem in entity.allowances)
                    {
                        if (singleItem.Id == 0 && singleItem.IsDeleted == false)
                        {

                            var Newallowance = new HrAllowanceDeduction
                            {
                                Amount = singleItem.Amount,
                                Rate = singleItem.Rate,
                                TypeId = 1,
                                EmpId = entity.Id,
                                AdId = singleItem.AdId,
                                FixedOrTemporary = 1,
                                CreatedBy = session.UserId,
                                CreatedOn = DateTime.Now,
                                IsDeleted = false,
                                //Note = singleItem.Note,
                                //DueDate = singleItem.DueDate
                            };

                            await hrRepositoryManager.HrAllowanceDeductionRepository.Add(Newallowance);
                            await hrRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                        }
                        else
                        {
                            var updateItem = await hrRepositoryManager.HrAllowanceDeductionRepository.GetOne(e => e.Id == singleItem.Id);
                            if (updateItem != null)
                            {
                                updateItem.Amount = singleItem.Amount;
                                updateItem.Rate = singleItem.Rate;
                                updateItem.IsDeleted = singleItem.IsDeleted ?? false;
                                hrRepositoryManager.HrAllowanceDeductionRepository.Update(updateItem);
                                await hrRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                            }


                        }

                    }

                }
                if (entity.deduction != null && entity.deduction.Any())
                {
                    foreach (var singleItem in entity.deduction)
                    {
                        if (singleItem.Id == 0 && singleItem.IsDeleted == false)
                        {

                            var Newallowance = new HrAllowanceDeduction
                            {
                                Amount = singleItem.Amount,
                                Rate = singleItem.Rate,
                                TypeId = 2,
                                EmpId = entity.Id,
                                AdId = singleItem.AdId,
                                FixedOrTemporary = 1,
                                CreatedBy = session.UserId,
                                CreatedOn = DateTime.Now,
                                IsDeleted = false,
                                //Note = singleItem.Note,
                                //DueDate = singleItem.DueDate
                            };

                            await hrRepositoryManager.HrAllowanceDeductionRepository.Add(Newallowance);
                            await hrRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                        }
                        else
                        {
                            var updateItem = await hrRepositoryManager.HrAllowanceDeductionRepository.GetOne(e => e.Id == singleItem.Id);
                            if (updateItem != null)
                            {
                                updateItem.Amount = singleItem.Amount;
                                updateItem.Rate = singleItem.Rate;
                                updateItem.IsDeleted = singleItem.IsDeleted ?? false;
                                hrRepositoryManager.HrAllowanceDeductionRepository.Update(updateItem);
                                await hrRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                            }


                        }

                    }

                }
                await mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);

                return await Result<EmployeeSalaryInfoDto>.SuccessAsync(_mapper.Map<EmployeeSalaryInfoDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                return await Result<EmployeeSalaryInfoDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<EmployeeSocialInsuranceDto>> UpdateSocialInsurance(EmployeeSocialInsuranceDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<EmployeeSocialInsuranceDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");


            try
            {
                var item = await mainRepositoryManager.InvestEmployeeRepository.GetById(entity.Id);

                if (item == null) return await Result<EmployeeSocialInsuranceDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");
                _mapper.Map(entity, item);
                item.SalaryInsuranceWage = entity.SalaryInsuranceWage;
                mainRepositoryManager.InvestEmployeeRepository.Update(item);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<EmployeeSocialInsuranceDto>.SuccessAsync(_mapper.Map<EmployeeSocialInsuranceDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                return await Result<EmployeeSocialInsuranceDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<EmployeeTravelInfoDto>> UpdateTravel(EmployeeTravelInfoDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<EmployeeTravelInfoDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");


            try
            {
                var item = await mainRepositoryManager.InvestEmployeeRepository.GetById(entity.Id);

                if (item == null) return await Result<EmployeeTravelInfoDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");
                _mapper.Map(entity, item);

                mainRepositoryManager.InvestEmployeeRepository.Update(item);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<EmployeeTravelInfoDto>.SuccessAsync(_mapper.Map<EmployeeTravelInfoDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                return await Result<EmployeeTravelInfoDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<bool>> ChangeEmployeesStatus(int StatusId, List<string> employeesId, string? Note, CancellationToken cancellationToken = default)
        {
            try
            {
                await mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);

                if (employeesId.Count <= 0) return await Result<bool>.FailAsync($"Error in {GetType()} : you must choose Employees.");

                foreach (var element in employeesId)
                {
                    var item = await mainRepositoryManager.InvestEmployeeRepository.GetOne(x => x.EmpId == element);
                    if (item == null) return await Result<bool>.FailAsync($"Error in {GetType()} : the employee with Id {element} IS NOt Found.");
                    var oldStatusId = item.StatusId;
                    item.ModifiedOn = DateTime.Now;
                    item.ModifiedBy = (int)session.UserId;
                    item.StatusId = StatusId;
                    mainRepositoryManager.InvestEmployeeRepository.Update(item);

                    await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                    if (!string.IsNullOrEmpty(Note))
                    {
                        var HrEmpStatusHistory = new HrEmpStatusHistory
                        {

                            EmpId = Convert.ToInt32(item.Id),
                            CreatedBy = session.UserId,
                            CreatedOn = DateTime.Now,
                            StatusId = StatusId,
                            StatusIdOld = oldStatusId,
                            Note = Note
                        };

                        await hrRepositoryManager.HrEmpStatusHistoryRepository.Add(HrEmpStatusHistory);

                        await hrRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                    }
                }

                await mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);

                return await Result<bool>.SuccessAsync(true, "Updated successfully");

            }
            catch (Exception exp)
            {

                return await Result<bool>.FailAsync($"EXP in Update at ( {GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<bool>> InsuranceUpdate(string Tdate, CancellationToken cancellationToken = default)
        {
            try
            {
                await mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);

                var items = await mainRepositoryManager.InvestEmployeeRepository
                    .GetAll(x => x.IsDeleted == false && x.Isdel == false && x.FacilityId == session.FacilityId);

                if (items == null || !items.Any())
                    return await Result<bool>.FailAsync(localization.GetMessagesResource("ThereAreNoEmployees"));

                var now = DateTime.Now;
                var userId = session.UserId;

                foreach (var element in items)
                {
                    element.ModifiedOn = now;
                    element.ModifiedBy = userId;
                    element.InsuranceDateValidity = Tdate;
                    mainRepositoryManager.InvestEmployeeRepository.Update(element);
                }

                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                await mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);

                return await Result<bool>.SuccessAsync(true, localization.GetMessagesResource("SuccessInsuranceHistoryUpdated"));
            }
            catch (Exception exp)
            {
                return await Result<bool>.FailAsync(
                    $"EXP in Update at ( {GetType()} ) , Message: {exp.Message} --- " +
                    (exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner"));
            }
        }


        public async Task<IResult<EmployeeSubDto>> AddEmployeeSub(EmployeeSubDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<EmployeeSubDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");

            try
            {

                var investEmployees = await mainRepositoryManager.InvestEmployeeRepository.GetOne(e => e.EmpId == entity.EmpCode && e.IsDeleted == false);
                if (investEmployees == null)
                {
                    return await Result<EmployeeSubDto>.FailAsync(localization.GetResource1("EmployeeNotFound"));
                }
                else
                {
                    entity.ParentId = investEmployees.Id;

                }
                var CheckManager = await mainRepositoryManager.InvestEmployeeRepository.GetOne(e => e.EmpId == entity.ManagerCode && e.IsDeleted == false);

                if (CheckManager == null)
                {
                    return await Result<EmployeeSubDto>.FailAsync(localization.GetMessagesResource("DirectManagerNotFound"));
                }
                else
                {
                    entity.ManagerId = CheckManager.Id;
                }

                var DoappointmentDate = DateHelper.StringToDate(entity.Doappointment);
                if (DoappointmentDate.Year <= 1900 || DoappointmentDate.Year >= 2100 || DoappointmentDate.Month < 1 || DoappointmentDate.Month > 12 || DoappointmentDate.Day < 1 || DoappointmentDate.Day > 31)
                {
                    return await Result<EmployeeSubDto>.FailAsync(localization.GetMessagesResource("InvalidNominationDate"));

                }
                long ccId = 0;
                if (!string.IsNullOrEmpty(entity.CostCenterCode))
                {
                    var costCenter = await accRepositoryManager.AccCostCenterRepository.GetOne(e => e.CcId, e => e.CostCenterCode == entity.CostCenterCode && e.FacilityId == session.FacilityId && e.IsActive == true && e.IsDeleted == false);
                    ccId = costCenter;
                }


                //  التشييك على الضوابط التشغيلية

                var projectsId = await mainRepositoryManager.SysDepartmentRepository.GetOne(p => p.Id == entity.Location);
                var Allow = await pmRepositoryManager.PMProjectsOperationalControlRepository.GetAll(a => a.IsDeleted == false && a.ProjectId == projectsId.ProjectId && a.JobCatagoriesId == entity.JobCatagoriesId);
                var AllowInJob = Allow.Sum(a => a.CountOfEmplyee ?? 0);
                var Current = await hrRepositoryManager.HrEmployeeRepository.GetAll(c => (c.StatusId == 1 || c.StatusId == 10) && c.IsDeleted == false && c.Location == entity.Location && c.JobCatagoriesId == entity.JobCatagoriesId);
                var CurrentInjob = 0;
                CurrentInjob = Current.Count();
                if (AllowInJob != 0 && AllowInJob < CurrentInjob + 1)
                {
                    var resultMessage = localization.GetMessagesResource("TheNumberOfEmployeesOnLocationForTheSpecifiedJobExceedsTheNumberAllowedInTheOperationalControls");
                    resultMessage += " " + localization.GetMessagesResource("NumberOfPositionsAllowedForTheSite") + AllowInJob.ToString();
                    resultMessage += " " + localization.GetMessagesResource("NumberOfEmployeesOnTheJobUnderProcess") + CurrentInjob.ToString();
                    return await Result<EmployeeSubDto>.FailAsync(resultMessage);
                }

                // التشييك على رواتب الوظائف والبرامج

                var jobsSalary = await pmRepositoryManager.PMJobsSalaryVwRepository.GetOne(j => j.IsDeleted == false && j.ProjectId == projectsId.ProjectId && j.JobCatagoriesId == entity.JobCatagoriesId && j.ProgramId == entity.ProgramId);
                var maxSalary = 0.00m;
                if (jobsSalary != null)
                {
                    maxSalary = jobsSalary.Maxsalary ?? 0.00m;
                }
                if (maxSalary != 0 && maxSalary < entity.Salary)
                {
                    var resultMessage = localization.GetMessagesResource("TheEmployeesOnLocationSalaryForTheSpecifiedPositionExceedsTheSalaryAllowedInTheOperationalControls") + " " +
                        localization.GetMessagesResource("TheHighestSalaryForAnEmployeeOnTheLocationMustNotExceedTheMaximumLimitWhichIs") + " " + maxSalary.ToString();
                    return await Result<EmployeeSubDto>.FailAsync(resultMessage);
                }

                var countSub = await mainRepositoryManager.InvestEmployeeRepository.GetAll(e => e.IsDeleted == false && e.IsSub == true && e.ParentId == entity.ParentId);
                var newInvestEmployee = new InvestEmployee
                {

                    EmpName = investEmployees.EmpName,
                    EmpName2 = investEmployees.EmpName2,
                    BranchId = entity.BranchId,
                    UserId = session.UserId,
                    JobType = 2,
                    JobCatagoriesId = entity.JobCatagoriesId,
                    StatusId = 10,
                    FacilityId = (int?)session.FacilityId,
                    Mobile = "",
                    Location = entity.Location,
                    DeptId = entity.DeptId,
                    Doappointment = entity.Doappointment,
                    BankId = entity.BankId,
                    IdNo = entity.IdNo,
                    NationalityId = entity.NationalityId,
                    Gender = entity.Gender,
                    ManagerId = entity.ManagerId,
                    OthersRequirements = "",
                    ContractTypeId = 0,
                    VacationDaysYear = 0,
                    DailyWorkingHours = entity.DailyWorkingHours,
                    ContractData = entity.ContractDate,
                    ContarctDate = entity.ContractDate,
                    ContractExpiryDate = entity.ContractExpiryDate,
                    Salary = entity.Salary,
                    Iban = entity.Iban,
                    AccountNo = entity.AccountNo,
                    ProgramId = 0,
                    ParentId = entity.ParentId,
                    SalaryGroupId = entity.SalaryGroupId,
                    EmpId = entity.EmpCode + "0" + (countSub.Count() + 1).ToString(),
                    CreatedBy = session.UserId,
                    CreatedOn = DateTime.Now,
                    IsSub = true,
                    CcId = ccId,
                };
                await mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);

                var newEntity = await mainRepositoryManager.InvestEmployeeRepository.AddAndReturn(newInvestEmployee);

                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                if (entity.ShitID > 0)
                {
                    var attEntity = new HrAttShiftEmployee
                    {
                        ShitId = entity.ShitID,
                        BeginDate = entity.Doappointment,
                        CreatedBy = session.UserId,
                        CreatedOn = DateTime.Now,
                        EmpId = newEntity.Id,
                        EndDate = "2027/01/01"
                    };
                    var hrAttShiftEmployeeEntity = await hrRepositoryManager.HrAttShiftEmployeeRepository.AddAndReturn(attEntity);
                    await hrRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                }
                ///////////////////////////////////////////////////////////////////

                if (entity.allowance != null && entity.allowance.Any())
                {
                    foreach (var singleItem in entity.allowance)
                    {

                        var Newallowance = new HrAllowanceDeduction
                        {
                            Amount = singleItem.AllowanceAmount,
                            Rate = singleItem.AllowanceRate,
                            TypeId = 1,
                            EmpId = newEntity.Id,
                            AdId = singleItem.AdId,
                            CreatedBy = session.UserId,
                            CreatedOn = DateHelper.GetCurrentDateTime(),
                            IsDeleted = false,
                            FixedOrTemporary = 1,

                        };

                        var hrAllownceDeductions = await hrRepositoryManager.HrAllowanceDeductionRepository
                            .GetAll(a => a.IsDeleted == false && a.EmpId == Newallowance.EmpId && a.TypeId == Newallowance.TypeId && a.AdId == Newallowance.AdId && a.FixedOrTemporary == 1);
                        if (hrAllownceDeductions.Any() == false)
                        {
                            await hrRepositoryManager.HrAllowanceDeductionRepository.Add(Newallowance);
                            await hrRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                        }
                        else
                        {
                            return await Result<EmployeeSubDto>.FailAsync(localization.GetResource1("RecodeExists"));
                        }

                    }

                }
                if (entity.deduction != null && entity.deduction.Any())
                {
                    foreach (var singleItem in entity.deduction)
                    {

                        var NewDeduction = new HrAllowanceDeduction
                        {
                            Amount = singleItem.DeductionAmount,
                            Rate = singleItem.DeductionRate,
                            TypeId = 2,
                            EmpId = newEntity.Id,
                            AdId = singleItem.AdId,
                            CreatedBy = session.UserId,
                            CreatedOn = DateTime.Now,
                            IsDeleted = false,
                            FixedOrTemporary = 1,

                        };
                        var hrAllownceDeductions = await hrRepositoryManager.HrAllowanceDeductionRepository
                            .GetAll(a => a.IsDeleted == false && a.EmpId == NewDeduction.EmpId && a.TypeId == NewDeduction.TypeId && a.AdId == NewDeduction.AdId && a.FixedOrTemporary == 1);
                        if (hrAllownceDeductions.Any() == false)
                        {
                            await hrRepositoryManager.HrAllowanceDeductionRepository.Add(NewDeduction);
                            await hrRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                        }
                        else
                        {
                            return await Result<EmployeeSubDto>.FailAsync(localization.GetResource1("RecodeExists"));
                        }
                    }

                }


                //////////////////////////////////////////////////////////////////
                var entityMap = _mapper.Map<EmployeeSubDto>(newEntity);

                await mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);

                return await Result<EmployeeSubDto>.SuccessAsync(entityMap, localization.GetResource1("AddSuccess"));
            }
            catch (Exception exc)
            {
                return await Result<EmployeeSubDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }

        }
        public async Task<IResult<object>> UpdateEmployeeSub(EmployeeSubDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<object>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");
            try
            {

                await mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);

                var investEmployees = await mainRepositoryManager.InvestEmployeeRepository.GetOne(e => e.EmpId == entity.EmpCode && e.IsDeleted == false);
                if (investEmployees == null)
                {
                    return await Result<object>.FailAsync(localization.GetResource1("EmployeeNotFound"));
                }
                else
                {
                    entity.ParentId = investEmployees.Id;

                }

                if (entity.ManagerId != null && entity.ManagerId != 0)
                {
                    var checkMangerId = await hrRepositoryManager.HrEmployeeRepository.GetAll(m => m.EmpId == entity.EmpCode && m.Isdel == false && m.IsDeleted == false);
                    if (!checkMangerId.Any())
                    {
                        return await Result<object>.FailAsync(localization.GetMessagesResource("DirectManagerNotFound"));
                    }
                }
                var DoappointmentDate = DateHelper.StringToDate(entity.Doappointment);
                if (DoappointmentDate.Year <= 1900 || DoappointmentDate.Year >= 2100 || DoappointmentDate.Month < 1 || DoappointmentDate.Month > 12 || DoappointmentDate.Day < 1 || DoappointmentDate.Day > 31)
                {
                    return await Result<object>.FailAsync(localization.GetMessagesResource("InvalidNominationDate"));

                }
                var projectsId = await mainRepositoryManager.SysDepartmentRepository.GetOne(p => p.Id == entity.Location);
                var Allow = await pmRepositoryManager.PMProjectsOperationalControlRepository.GetAll(a => a.IsDeleted == false && a.ProjectId == projectsId.ProjectId && a.JobCatagoriesId == entity.JobCatagoriesId);
                var AllowInJob = Allow.Sum(a => a.CountOfEmplyee ?? 0);
                var Current = await hrRepositoryManager.HrEmployeeRepository.GetAll(c => (c.StatusId == 1 || c.StatusId == 10) && c.IsDeleted == false && c.Location == entity.Location && c.JobCatagoriesId == entity.JobCatagoriesId);
                var CurrentInjob = 0;
                CurrentInjob = Current.Count();
                if (AllowInJob != 0 && AllowInJob < CurrentInjob + 1)
                {
                    var resultMessage = localization.GetMessagesResource("TheNumberOfEmployeesOnLocationForTheSpecifiedJobExceedsTheNumberAllowedInTheOperationalControls");
                    resultMessage += " " + localization.GetMessagesResource("NumberOfPositionsAllowedForTheSite") + AllowInJob.ToString();
                    resultMessage += " " + localization.GetMessagesResource("NumberOfEmployeesOnTheJobUnderProcess") + CurrentInjob.ToString();

                    return await Result<object>.FailAsync(resultMessage);
                }

                // التشييك على رواتب الوظائف والبرامج


                var jobsSalary = await pmRepositoryManager.PMJobsSalaryVwRepository.GetOne(j => j.IsDeleted == false && j.ProjectId == projectsId.ProjectId && j.JobCatagoriesId == entity.JobCatagoriesId && j.ProgramId == entity.ProgramId);
                var maxSalary = 0.00m;
                if (jobsSalary != null)
                {
                    maxSalary = jobsSalary.Maxsalary ?? 0.00m;
                }
                if (maxSalary != 0 && maxSalary < entity.Salary)
                {
                    var resultMessage = localization.GetMessagesResource("TheEmployeesOnLocationSalaryForTheSpecifiedPositionExceedsTheSalaryAllowedInTheOperationalControls") + " " +
                        localization.GetMessagesResource("TheHighestSalaryForAnEmployeeOnTheLocationMustNotExceedTheMaximumLimitWhichIs") + " " + maxSalary.ToString();
                    return await Result<object>.FailAsync(resultMessage);
                }

                long ccId = 0;
                if (!string.IsNullOrEmpty(entity.CostCenterCode))
                {
                    var costCenter = await accRepositoryManager.AccCostCenterRepository.GetOne(e => e.CcId, e => e.CostCenterCode == entity.CostCenterCode && e.FacilityId == session.FacilityId && e.IsActive == true && e.IsDeleted == false);
                    ccId = costCenter;
                }
                var CheckManager = await mainRepositoryManager.InvestEmployeeRepository.GetOne(e => e.EmpId == entity.ManagerCode && e.IsDeleted == false);

                if (CheckManager == null)
                {
                    return await Result<object>.FailAsync(localization.GetMessagesResource("DirectManagerNotFound"));
                }
                else
                {
                    entity.ManagerId = CheckManager.Id;
                }
                var subEmp = await mainRepositoryManager.InvestEmployeeRepository.GetOne(e => e.Id == entity.Id);
                _mapper.Map(entity, subEmp);

                subEmp.EmpId = entity.JobID;
                subEmp.EmpName = investEmployees.EmpName;
                subEmp.EmpName2 = investEmployees.EmpName2;
                subEmp.ModifiedBy = session.UserId;
                subEmp.BranchId = entity.BranchId;
                subEmp.JobCatagoriesId = entity.JobCatagoriesId;
                subEmp.Location = entity.Location;
                subEmp.DeptId = entity.DeptId;
                subEmp.Doappointment = entity.Doappointment;
                subEmp.BankId = entity.BankId;
                subEmp.IdNo = entity.IdNo;
                subEmp.NationalityId = entity.NationalityId;
                subEmp.Gender = entity.Gender;
                subEmp.ManagerId = entity.ManagerId;
                subEmp.Salary = entity.Salary;
                subEmp.Iban = entity.Iban;
                subEmp.AccountNo = entity.AccountNo;
                subEmp.ParentId = entity.ParentId;
                subEmp.DailyWorkingHours = entity.DailyWorkingHours;
                subEmp.ModifiedOn = DateTime.Now;
                subEmp.ContractData = entity.ContractDate;
                subEmp.ContractExpiryDate = entity.ContractExpiryDate;
                subEmp.SalaryGroupId = entity.SalaryGroupId;
                subEmp.CcId = ccId;

                mainRepositoryManager.InvestEmployeeRepository.Update(subEmp);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                //  تعديل جميع البيانات
                ///////////////////////////////////////////////////////////////////

                if (entity.allowance != null && entity.allowance.Any())
                {
                    foreach (var singleItem in entity.allowance)
                    {
                        if (singleItem.Id == 0 && singleItem.IsDeleted == false)
                        {

                            var Newallowance = new HrAllowanceDeduction
                            {
                                Amount = singleItem.AllowanceAmount,
                                Rate = singleItem.AllowanceRate,
                                TypeId = 1,
                                EmpId = subEmp.Id,
                                AdId = singleItem.AdId,
                                CreatedBy = session.UserId,
                                FixedOrTemporary = 1,
                                CreatedOn = DateHelper.GetCurrentDateTime(),
                                IsDeleted = false
                            };

                            await hrRepositoryManager.HrAllowanceDeductionRepository.Add(Newallowance);
                            await hrRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                        }
                        else
                        {
                            var updateItem = await hrRepositoryManager.HrAllowanceDeductionRepository.GetOne(e => e.Id == singleItem.Id);
                            if (updateItem != null)
                            {
                                updateItem.Amount = singleItem.AllowanceAmount;
                                updateItem.Rate = singleItem.AllowanceRate;
                                updateItem.IsDeleted = singleItem.IsDeleted;
                                hrRepositoryManager.HrAllowanceDeductionRepository.Update(updateItem);
                                await hrRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                            }
                        }
                    }
                }
                if (entity.deduction != null && entity.deduction.Any())
                {
                    foreach (var singleItem in entity.deduction)
                    {
                        if (singleItem.Id == 0 && singleItem.IsDeleted == false)
                        {

                            var Newallowance = new HrAllowanceDeduction
                            {
                                Amount = singleItem.DeductionAmount,
                                Rate = singleItem.DeductionRate,
                                TypeId = 2,
                                EmpId = subEmp.Id,
                                AdId = singleItem.AdId,
                                CreatedBy = session.UserId,
                                FixedOrTemporary = 1,
                                CreatedOn = DateHelper.GetCurrentDateTime(),
                                IsDeleted = false
                            };

                            await hrRepositoryManager.HrAllowanceDeductionRepository.Add(Newallowance);
                            await hrRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                        }
                        else
                        {
                            var updateItem = await hrRepositoryManager.HrAllowanceDeductionRepository.GetOne(e => e.Id == singleItem.Id);
                            if (updateItem != null)
                            {
                                updateItem.Amount = singleItem.DeductionAmount;
                                updateItem.Rate = singleItem.DeductionRate;
                                updateItem.IsDeleted = singleItem.IsDeleted;
                                hrRepositoryManager.HrAllowanceDeductionRepository.Update(updateItem);
                                await hrRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                            }
                        }
                    }
                }
                //////////////////////////////////////////////////////////////////

                //var entityMap = _mapper.Map<EmployeeSubDto>(investEmployees);

                await mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);

                return await Result<object>.SuccessAsync(subEmp, localization.GetResource1("CreateSuccess"));
            }
            catch (Exception exc)
            {
                return await Result<object>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }

        }

        public async Task<IResult<bool>> ChangeEmployeeManager1(string empCode, List<string> employeesCodes, CancellationToken cancellationToken = default)
        {
            var checkManager = await mainRepositoryManager.InvestEmployeeRepository.GetOne(x => x.EmpId == empCode && x.IsDeleted == false && x.Isdel == false);
            if (checkManager == null) return await Result<bool>.FailAsync(localization.GetMessagesResource("DirectManagerNotFound"));

            try
            {
                await mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);

                if (employeesCodes == null) return await Result<bool>.FailAsync(localization.GetMessagesResource("PleaseSelectAtLeastOneEmployee"));

                foreach (var element in employeesCodes)
                {
                    var item = await mainRepositoryManager.InvestEmployeeRepository.GetOne(x => x.EmpId == element && x.IsDeleted == false && x.Isdel == false);
                    if (item == null) return await Result<bool>.FailAsync(localization.GetResource1("EmployeeNotFound"));

                    item.ModifiedOn = DateTime.Now;
                    item.ModifiedBy = (int)session.UserId;
                    item.ManagerId = checkManager.Id;
                    mainRepositoryManager.InvestEmployeeRepository.Update(item);
                    await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                }
                await mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);
                return await Result<bool>.SuccessAsync(true, localization.GetResource1("UpdateSuccess"));
            }
            catch (Exception exp)
            {
                return await Result<bool>.FailAsync($"EXP in Update at ( {GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<bool>> ChangeEmployeeManager2(string empCode, List<string> empCodes, CancellationToken cancellationToken = default)
        {
            var checkManager = await mainRepositoryManager.InvestEmployeeRepository.GetOne(x => x.EmpId == empCode && x.IsDeleted == false && x.Isdel == false);
            if (checkManager == null) return await Result<bool>.FailAsync(localization.GetMessagesResource("AdministrativeManager1NotFound"));

            try
            {
                await mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);

                if (empCodes.Count() <= 0) return await Result<bool>.FailAsync(localization.GetMessagesResource("PleaseSelectAtLeastOneEmployee"));

                foreach (var element in empCodes)
                {
                    var item = await mainRepositoryManager.InvestEmployeeRepository.GetOne(x => x.EmpId == element && x.IsDeleted == false && x.Isdel == false);
                    if (item == null) return await Result<bool>.FailAsync(localization.GetResource1("EmployeeNotFound"));

                    item.ModifiedOn = DateTime.Now;
                    item.ModifiedBy = (int)session.UserId;
                    item.Manager2Id = checkManager.Id;
                    mainRepositoryManager.InvestEmployeeRepository.Update(item);
                    await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                }
                await mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);
                return await Result<bool>.SuccessAsync(true, localization.GetResource1("UpdateSuccess"));
            }
            catch (Exception exp)
            {
                return await Result<bool>.FailAsync($"EXP in Update at ( {GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<bool>> ChangeEmployeeManager3(string empId, List<string> empCodes, CancellationToken cancellationToken = default)
        {
            var checkManager = await mainRepositoryManager.InvestEmployeeRepository.GetOne(x => x.EmpId == empId && x.IsDeleted == false && x.Isdel == false);
            if (checkManager == null) return await Result<bool>.FailAsync(localization.GetMessagesResource("AdministrativeManager2NotFound"));

            try
            {
                await mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);

                if (empCodes.Count() <= 0) return await Result<bool>.FailAsync(localization.GetMessagesResource("PleaseSelectAtLeastOneEmployee"));

                foreach (var element in empCodes)
                {
                    var item = await mainRepositoryManager.InvestEmployeeRepository.GetOne(x => x.EmpId == element && x.IsDeleted == false && x.Isdel == false);
                    if (item == null) return await Result<bool>.FailAsync(localization.GetResource1("EmployeeNotFound"));
                    item.ModifiedOn = DateTime.Now;
                    item.ModifiedBy = (int)session.UserId;
                    item.Manager3Id = checkManager.Id;
                    mainRepositoryManager.InvestEmployeeRepository.Update(item);
                    await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                }
                await mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);
                return await Result<bool>.SuccessAsync(true, localization.GetResource1("UpdateSuccess"));
            }
            catch (Exception exp)
            {
                return await Result<bool>.FailAsync($"EXP in Update at ( {GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<string>> UpdateContractExpair(List<string> empCodes, string NewDate = "", CancellationToken cancellationToken = default)
        {
            try
            {
                await hrRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);

                int count = 0;
                if (string.IsNullOrEmpty(NewDate))
                {
                    foreach (var item in empCodes)
                    {

                        var checkEmp = await mainRepositoryManager.InvestEmployeeRepository.GetOne(x => x.EmpId == item && x.IsDeleted == false && x.Isdel == false);
                        if (checkEmp == null) return await Result<string>.FailAsync($"{item}" + localization.GetResource1("EmployeeNotFound"));
                        checkEmp.ContractExpiryDate = DateHelper.StringToDate(checkEmp.ContractExpiryDate).AddYears(1).ToString("yyyy/MM/dd", CultureInfo.InvariantCulture); ;
                        checkEmp.ModifiedBy = session.UserId;
                        checkEmp.ModifiedOn = DateTime.Now;
                        mainRepositoryManager.InvestEmployeeRepository.Update(checkEmp);
                        await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                        count++;
                        var getNotificationsType = await hrRepositoryManager.HrNotificationsTypeRepository.GetAllVW(x => x.IsDeleted == false && x.IsActive == true && x.FacilityId == session.FacilityId && x.SubjectType == 1);
                        if (getNotificationsType != null)
                        {
                            foreach (var newType in getNotificationsType)
                            {
                                var newNotification = new HrNotification
                                {
                                    CreatedBy = session.UserId,
                                    CreatedOn = DateTime.Now,
                                    TypeId = 1,
                                    NotificationDate = DateHelper.StringToDate(checkEmp.ContractExpiryDate).AddYears(1).ToString("yyyy/MM/dd", CultureInfo.InvariantCulture),
                                    EmpId = checkEmp.Id,
                                    FacilityId = session.FacilityId,
                                    Subject = newType.MsgSubject,
                                    Detailes = newType.Detailes,
                                    IsDeleted = false
                                };

                                await hrRepositoryManager.HrNotificationRepository.Add(newNotification);
                                await hrRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                            }

                        }
                    }
                }
                else
                {
                    if (DateHelper.StringToDate(NewDate) <= DateTime.Now) return await Result<string>.FailAsync($" هناك خطأ في تاريخ التجديد");
                    foreach (var item in empCodes)
                    {

                        var checkEmp = await mainRepositoryManager.InvestEmployeeRepository.GetOne(x => x.EmpId == item && x.IsDeleted == false && x.Isdel == false);
                        if (checkEmp == null) return await Result<string>.FailAsync($"{item}" + localization.GetResource1("EmployeeNotFound"));
                        checkEmp.ContractExpiryDate = NewDate;
                        checkEmp.ModifiedBy = session.UserId;
                        checkEmp.ModifiedOn = DateTime.Now;
                        mainRepositoryManager.InvestEmployeeRepository.Update(checkEmp);
                        await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                        count++;
                        var getNotificationsType = await hrRepositoryManager.HrNotificationsTypeRepository.GetAllVW(x => x.IsDeleted == false && x.IsActive == true && x.FacilityId == session.FacilityId && x.SubjectType == 1);
                        if (getNotificationsType != null)
                        {
                            foreach (var newType in getNotificationsType)
                            {
                                var newNotification = new HrNotification
                                {
                                    CreatedBy = session.UserId,
                                    CreatedOn = DateTime.Now,
                                    TypeId = 1,
                                    NotificationDate = NewDate,
                                    EmpId = checkEmp.Id,
                                    FacilityId = session.FacilityId,
                                    Subject = newType.MsgSubject,
                                    Detailes = newType.Detailes,
                                    IsDeleted = false
                                };

                                await hrRepositoryManager.HrNotificationRepository.Add(newNotification);
                                await hrRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                            }

                        }
                    }


                }
                await hrRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);
                return await Result<string>.SuccessAsync("تمت عملية التجديد لعدد " + count + " موظف بنجاح");

            }
            catch (Exception exp)
            {
                Console.WriteLine($"EXP in UpdateContractExpair at ( {GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
                return await Result<string>.FailAsync($"EXP in UpdateContractExpair at ( {GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");

            }
        }

        public async Task<IResult<string>> UpdateMedicalInsuranceExpair(List<string> empCodes, string NewDate = "", CancellationToken cancellationToken = default)
        {
            try
            {
                await hrRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);

                int count = 0;
                if (string.IsNullOrEmpty(NewDate))
                {
                    foreach (var item in empCodes)
                    {

                        var checkEmp = await mainRepositoryManager.InvestEmployeeRepository.GetOne(x => x.EmpId == item && x.IsDeleted == false && x.Isdel == false);
                        if (checkEmp == null) return await Result<string>.FailAsync($"{item}" + localization.GetResource1("EmployeeNotFound"));
                        checkEmp.InsuranceDateValidity = DateHelper.StringToDate(checkEmp.InsuranceDateValidity).AddYears(1).ToString("yyyy/MM/dd", CultureInfo.InvariantCulture); ;
                        checkEmp.ModifiedBy = session.UserId;
                        checkEmp.ModifiedOn = DateTime.Now;
                        mainRepositoryManager.InvestEmployeeRepository.Update(checkEmp);
                        await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                        count++;
                    }
                }
                else
                {
                    if (DateHelper.StringToDate(NewDate) <= DateTime.Now) return await Result<string>.FailAsync($"  هناك خطأ في تاريخ التجديد ");
                    foreach (var item in empCodes)
                    {

                        var checkEmp = await mainRepositoryManager.InvestEmployeeRepository.GetOne(x => x.EmpId == item && x.IsDeleted == false && x.Isdel == false);
                        if (checkEmp == null) return await Result<string>.FailAsync($"{item}" + localization.GetResource1("EmployeeNotFound"));
                        checkEmp.InsuranceDateValidity = NewDate;
                        checkEmp.ModifiedBy = session.UserId;
                        checkEmp.ModifiedOn = DateTime.Now;
                        mainRepositoryManager.InvestEmployeeRepository.Update(checkEmp);
                        await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                        count++;
                    }
                }
                await hrRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);
                return await Result<string>.SuccessAsync("تمت عملية التجديد لعدد " + count + " موظف بنجاح");

            }
            catch (Exception exp)
            {
                Console.WriteLine($"EXP in UpdateMedicalInsuranceExpair at ( {GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
                return await Result<string>.FailAsync($"EXP in UpdateContractExpair at ( {GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");

            }
        }

        public async Task<IResult<AccountConnectDto>> UpdateConnectAccounts(AccountConnectDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<AccountConnectDto>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");

            try
            {
                long AccountLoanID = 0;
                long CCID = 0;
                long CCID2 = 0;
                long CCID3 = 0;
                long CCID4 = 0;
                long CCID5 = 0;
                var item = await mainRepositoryManager.InvestEmployeeRepository.GetById(entity.Id);

                if (item == null) return await Result<AccountConnectDto>.FailAsync($"--- there is no Data with this id: {entity.Id}---");

                if (!string.IsNullOrEmpty(entity.AccAccountCode))
                {
                    var getaccountByCode = await accRepositoryManager.AccAccountsSubHelpeVwRepository.GetOne(x => x.Isdel == false && x.IsActive == true && (entity.FacilityId == 0 || entity.FacilityId == null || entity.FacilityId == x.FacilityId) && x.AccAccountCode == entity.AccAccountCode);
                    AccountLoanID = (getaccountByCode == null ? 0 : getaccountByCode.AccAccountId);

                }

                item.AccountId = AccountLoanID;
                item.SalaryGroupId = entity.SalaryGroupId;

                if (!string.IsNullOrEmpty(entity.CcId))
                {
                    var getCostCenterCode = await accRepositoryManager.AccCostCenterRepository.GetOne(x => x.IsDeleted == false && x.IsActive == true && entity.FacilityId == x.FacilityId && x.CostCenterCode == entity.CcId);
                    CCID = (getCostCenterCode == null ? 0 : getCostCenterCode.CcId);

                    if (CCID <= 0) return await Result<AccountConnectDto>.FailAsync($"{localization.GetHrResource("CostCenterNumberNotFound")} 1");
                }
                if (!string.IsNullOrEmpty(entity.CcId2))
                {
                    var getCostCenterCode = await accRepositoryManager.AccCostCenterRepository.GetOne(x => x.IsDeleted == false && x.IsActive == true && entity.FacilityId == x.FacilityId && x.CostCenterCode == entity.CcId2);
                    CCID2 = (getCostCenterCode == null ? 0 : getCostCenterCode.CcId);

                    if (CCID2 <= 0) return await Result<AccountConnectDto>.FailAsync($"{localization.GetHrResource("CostCenterNumberNotFound")} 2");
                }
                if (!string.IsNullOrEmpty(entity.CcId3))
                {
                    var getCostCenterCode = await accRepositoryManager.AccCostCenterRepository.GetOne(x => x.IsDeleted == false && x.IsActive == true && entity.FacilityId == x.FacilityId && x.CostCenterCode == entity.CcId3);
                    CCID3 = (getCostCenterCode == null ? 0 : getCostCenterCode.CcId);

                    if (CCID3 <= 0) return await Result<AccountConnectDto>.FailAsync($"{localization.GetHrResource("CostCenterNumberNotFound")} 3");
                }
                if (!string.IsNullOrEmpty(entity.CcId4))
                {
                    var getCostCenterCode = await accRepositoryManager.AccCostCenterRepository.GetOne(x => x.IsDeleted == false && x.IsActive == true && entity.FacilityId == x.FacilityId && x.CostCenterCode == entity.CcId4);
                    CCID4 = (getCostCenterCode == null ? 0 : getCostCenterCode.CcId);

                    if (CCID4 <= 0) return await Result<AccountConnectDto>.FailAsync($"{localization.GetHrResource("CostCenterNumberNotFound")} 4");
                }
                if (!string.IsNullOrEmpty(entity.CcId5))
                {
                    var getCostCenterCode = await accRepositoryManager.AccCostCenterRepository.GetOne(x => x.IsDeleted == false && x.IsActive == true && entity.FacilityId == x.FacilityId && x.CostCenterCode == entity.CcId5);
                    CCID5 = (getCostCenterCode == null ? 0 : getCostCenterCode.CcId);

                    if (CCID5 <= 0) return await Result<AccountConnectDto>.FailAsync($"{localization.GetHrResource("CostCenterNumberNotFound")} 5");
                }
                item.CcId = CCID;
                item.CcId2 = CCID2;
                item.CcId3 = CCID3;
                item.CcId4 = CCID4;
                item.CcId5 = CCID5;
                item.CcRate = entity.CcRate;
                item.CcRate2 = entity.CcRate2;
                item.CcRate3 = entity.CcRate3;
                item.CcRate4 = entity.CcRate4;
                item.CcRate5 = entity.CcRate5;
                item.FacilityId = entity.FacilityId;
                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                mainRepositoryManager.InvestEmployeeRepository.Update(item);

                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<AccountConnectDto>.SuccessAsync(_mapper.Map<AccountConnectDto>(item), "Item updated successfully");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
                return await Result<AccountConnectDto>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<IResult<string>> UpdateIDExpair(HREmpIDExpireUpdateDto obj, CancellationToken cancellationToken = default)
        {
            try
            {
                await hrRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);
                int count = 0;

                if (obj.Duration <= 0) return await Result<string>.FailAsync("لم يتم اختيار مدة التجديد ");

                var getAllEmployees = await mainRepositoryManager.InvestEmployeeRepository.GetAll(x => x.IsDeleted == false && x.Isdel == false && x.StatusId != 2);

                foreach (var item in obj.EmpCode)
                {
                    var checkEmp = getAllEmployees.FirstOrDefault(x => x.EmpId == item);

                    if (checkEmp == null)
                        return await Result<string>.FailAsync($"{item} {localization.GetResource1("EmployeeNotFound")}");

                    if (string.IsNullOrEmpty(checkEmp.IdExpireDate))
                        return await Result<string>.FailAsync($"لم يتم ادخال بيانات الهويه للموظف رقم {item}");

                    DateTime expireDate;

                    // Check if the date is Hijri
                    if (Bahsas.IsHijri(checkEmp.IdExpireDate, session))
                    {
                        expireDate = Bahsas.ConvertHijriStringToDate(checkEmp.IdExpireDate);

                        // Update expiry date based on duration
                        if (obj.Duration == 1)
                            expireDate = expireDate.AddYears(1);
                        else
                            expireDate = expireDate.AddMonths(obj.Duration);

                        // Convert back to Hijri string format
                        checkEmp.IdExpireDate = Bahsas.ConvertDateToHijriString(expireDate);
                    }
                    else
                    {
                        expireDate = DateHelper.StringToDate(checkEmp.IdExpireDate);

                        // Update expiry date based on duration
                        if (obj.Duration == 1)
                            expireDate = expireDate.AddYears(1);
                        else
                            expireDate = expireDate.AddMonths(obj.Duration);

                        // Convert back to string format
                        checkEmp.IdExpireDate = expireDate.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                    }

                    checkEmp.ModifiedBy = session.UserId;
                    checkEmp.ModifiedOn = DateTime.Now;
                    mainRepositoryManager.InvestEmployeeRepository.Update(checkEmp);
                    count++;
                }

                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                await hrRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);

                return await Result<string>.SuccessAsync($"تمت عملية التجديد لعدد {count} موظف بنجاح");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"EXP in UpdateIDExpair at ({GetType()}), Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
                return await Result<string>.FailAsync($"EXP in UpdateIDExpair at ({GetType()}), Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<string> GetTimeZone(int Id, CancellationToken cancellationToken = default)
        {
            try
            {
                var timeZone = await hrRepositoryManager.HrTimeZoneRepository.GetOne(x => x.TimeZoneId, x => x.Id == Id && x.IsDeleted == false);
                return timeZone ?? "";
            }
            catch
            {
                return "";
            }
        }

        public async Task<IResult<object>> ChangeEmployeeImage(ChangeEmployeeImageDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<object>.FailAsync($"Error in {this.GetType()} : the passed entity IS NULL.");

            try
            {
                var item = await mainRepositoryManager.InvestEmployeeRepository.GetOne(x => x.EmpId == entity.empCode);

                if (item == null) return await Result<object>.FailAsync($"--- there is no Data with this id: {entity.empCode}---");


                item.EmpPhoto = entity.imageUrl;
                item.ModifiedBy = session.UserId;
                item.ModifiedOn = DateTime.Now;
                mainRepositoryManager.InvestEmployeeRepository.Update(item);

                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                return await Result<object>.SuccessAsync(_mapper.Map<object>(item), "Image updated successfully");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
                return await Result<object>.FailAsync($"EXP in Update at ( {this.GetType()} ) , Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
            }
        }

        public async Task<long> GetManagerId(string managerEmpId, CancellationToken cancellationToken = default)
        {
            try
            {
                var emp = await mainRepositoryManager.InvestEmployeeRepository.GetOne(x => x.EmpId == managerEmpId && x.Isdel == false);
                if (emp == null) return 0L;
                return emp.Id;
            }
            catch
            {
                return 0L;
            }
        }
        public async Task<IResult<SelectList>> DDLFieldColumns()
        {
            try
            {
                var result = await mainRepositoryManager.InvestEmployeeRepository.DDLFieldColumns();
                foreach(var i in result.Data)
                {
                    i.Value = i.Text;
                }
                return result;
            }
            catch (Exception ex)
            {
                return  null;
            }
        }
    }
}