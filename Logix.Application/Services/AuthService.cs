using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Office;
using Logix.Application.Common;
using Logix.Application.DTOs.HR;
using Logix.Application.DTOs.Main;
using Logix.Application.Helpers;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Application.Interfaces.IServices;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.HR;
using Logix.Domain.Main;
using Logix.Domain.TS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Logix.Application.Services.Main
{
    public class AuthService : IAuthService
    {
        private readonly ISysUserRepository userRepository;
        private readonly IConfiguration config;
        private readonly ICurrentData session;
        private readonly IMvcSession mvcSession;
        private readonly ISysConfigurationAppHelper sysConfigurationAppHelper;
        private readonly IMainRepositoryManager mainRepositoryManager;
        private readonly ISendSmsHelper sendSmsHelper;
        private readonly IEmailAppHelper emailAppHelper;
        private readonly ITsRepositoryManager tsRepositoryManager;
        private readonly ILocalizationService localization;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthService(ISysUserRepository userRepository,
            IConfiguration config,
            ICurrentData session,
            IMvcSession mvcSession,
            ISysConfigurationAppHelper sysConfigurationAppHelper,
            IMainRepositoryManager mainRepositoryManager,
            ISendSmsHelper sendSmsHelper,
            IEmailAppHelper emailAppHelper,
            ITsRepositoryManager tsRepositoryManager,
            ILocalizationService localization,
            IHttpContextAccessor httpContextAccessor)
        {
            this.userRepository = userRepository;
            this.config = config;
            this.session = session;
            this.mvcSession = mvcSession;
            this.sysConfigurationAppHelper = sysConfigurationAppHelper;
            this.mainRepositoryManager = mainRepositoryManager;
            this.sendSmsHelper = sendSmsHelper;
            this.emailAppHelper = emailAppHelper;
            this.tsRepositoryManager = tsRepositoryManager;
            this.localization = localization;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<IResult<SysUserVw>> Login(LoginDto login)
        {
            try
            {
                if (login == null) return await Result<SysUserVw>.FailAsync($"Error in login of: {GetType()}, the passed entity is NULL !!!.");

                if (string.IsNullOrEmpty(login.UserName))
                {
                    return await Result<SysUserVw>.FailAsync($"Username is required");
                }

                if (string.IsNullOrEmpty(login.Password) || login.Password.Count() < 3)
                {
                    return await Result<SysUserVw>.FailAsync($"Password is required");
                }

                var userVw = await userRepository.Login(login.UserName, login.Password, login.FacilityId);
                if (userVw == null)
                {
                    return await Result<SysUserVw>.FailAsync($"User not found");
                }
                /* if (!await CheckPassword(user, login.Password))
                 {
                     return await Result<AuthDto>.FailAsync($"password not correct");
                 }*/
                return await Result<SysUserVw>.SuccessAsync(userVw, "login done successfully");

            }
            catch (Exception exp)
            {
                return await Result<SysUserVw>.FailAsync($"{exp.Message}");
            }
        }

        public string GetJWTToken(SysUserVw user, string secret, long finYear, long periodId, int lang, int currFinYearGregorian, string CalendarType, int IsAzureAuthenticated = 0)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            /*            var appSetting = config.GetSection("AppSettings");
                        var secret = appSetting.GetValue<string>("Secret");*/
            var key = Encoding.UTF8.GetBytes(secret);

            List<Claim> claims = new List<Claim>
                {
                    new Claim("Username", user.UserName),
                    new Claim("FullName", user.UserFullname),
                    new Claim("EmpCode", user.EmpCode),
                    new Claim("EmpId", user.EmpId.ToString()),
                    new Claim("GroupId", user.GroupsId),
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim("DeptId", user.DeptId.ToString()),
                    new Claim("Branches", user.BranchsId),
                    new Claim("BranchId", user.UserPkId.ToString()),
                    new Claim("LocationId", user.Location.ToString()),
                    new Claim("FacilityId", user.FacilityId.ToString()),
                    new Claim("FinYear", finYear.ToString()), // this must get the fin_year 
                    new Claim("PeriodId", periodId.ToString()), // this must get the fin_year 
                    new Claim("Language", lang.ToString()), // this must get the Language 
                    new Claim("FinyearGregorian", currFinYearGregorian.ToString()),
                    new Claim("CalendarType", CalendarType.ToString()),
                    new Claim("SalesType", user.SalesType.ToString()??"0"),
                    new Claim("IsAzureAuthenticated", IsAzureAuthenticated.ToString()??"0"),
                    new Claim("isAgree", user.IsAgree.ToString()??"1"),
                };
            SigningCredentials signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken
            (

            claims: claims,
            expires: DateTime.UtcNow.AddHours(5),
            signingCredentials: signingCredentials
            );
            int groupId = 0;

            if (!string.IsNullOrEmpty(user.GroupsId))
            {
                var hasGroup = int.TryParse(user.GroupsId, out groupId);
            }
            long FacilityId = 0;
            int SalesType = 0;
            if (user.FacilityId > 0)
            {
                FacilityId = (long)user.FacilityId;
            }
            if (user.SalesType > 0)
            {
                SalesType = (int)user.SalesType;
            }
            var token = tokenHandler.WriteToken(tokenOptions);
            //session.SetMainData(user.UserId, user.EmpId ?? 0, groupId, FacilityId, finYear, lang, user.BranchsId ?? "", CalendarType, user.UserPkId ?? 0, currFinYearGregorian, user.Location, user.DeptId, SalesType, IsAzureAuthenticated, user.IsAgree ?? true);
            mvcSession.SetMainData(user.UserId, user.EmpId ?? 0, groupId, FacilityId, finYear, lang, user.BranchsId ?? "", CalendarType, user.UserPkId ?? 0, currFinYearGregorian, user.Location, user.DeptId, SalesType, periodId, IsAzureAuthenticated, user.IsAgree ?? true);

            return token;
        }



        public async Task<IResult<SysUser>> GetByName(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    return await Result<SysUser>.FailAsync($"Username is required");
                }
                var user = await userRepository.GetOne(u => u.UserName == username && u.Enable == 1 && u.Isdel == false && u.IsDeleted == false);
                if (user == null)
                {
                    return await Result<SysUser>.FailAsync($"account not found");
                }
                return await Result<SysUser>.SuccessAsync(user, $"");
            }
            catch (Exception exp)
            {
                return await Result<SysUser>.FailAsync($"EXP: {exp.Message}");
            }
        }

        public async Task<IResult<SysUser>> GetById(long id)
        {
            try
            {
                if (id == 0)
                {
                    return await Result<SysUser>.FailAsync($"userId is required");
                }
                var user = await userRepository.GetOne(u => u.Id == id && u.Enable == 1 && u.Isdel == false && u.IsAgree == true);
                if (user == null)
                {
                    return await Result<SysUser>.FailAsync($"user not found");
                }
                return await Result<SysUser>.SuccessAsync(user, $"");
            }
            catch (Exception exp)
            {
                return await Result<SysUser>.FailAsync($"EXP: {exp.Message}");
            }
        }

        public async Task<IResult<IEnumerable<SysUserVw>>> GetUsersByEmpId(int empId, long userId)
        {
            try
            {
                if (empId == 0 || userId == 0)
                {
                    return await Result<IEnumerable<SysUserVw>>.FailAsync($"Username is required");
                }
                var users = await userRepository.GetUsersByEmpId(empId, userId);
                if (users == null)
                {
                    return await Result<IEnumerable<SysUserVw>>.FailAsync($"no users found for this employee");
                }
                return await Result<IEnumerable<SysUserVw>>.SuccessAsync(users, $"");
            }
            catch (Exception exp)
            {
                return await Result<IEnumerable<SysUserVw>>.FailAsync($"EXP: {exp.Message}");
            }
        }

        /*private async Task<bool> CheckPassword(SysUser user, string password)
        {
            try
            {
                var hasher = new PasswordHasher();
                if (user == null || password == null) return false;

                return hasher.VerifyPassword(password, user.Password);
            }
            catch (Exception)
            {

                return false;
            }
        }*/


        public async Task CreateUser(string UserName, string empCode, string empName, long facilityId, int branchId, string email, string password, string groupId, CancellationToken cancellationToken = default)
        {
            // استرجاع إعدادات النظام
            var createEmp = await sysConfigurationAppHelper.GetValue(211, facilityId);

            // تهيئة كائن SysUser
            var NewUser = new SysUserDto();

            long empId = 0;

            var ChkEmpid = await mainRepositoryManager.InvestEmployeeRepository.GetAll(e => e.EmpId == UserName && e.Isdel == false && e.IsDeleted == false);
            var ChkEmpEmail = await mainRepositoryManager.InvestEmployeeRepository.GetAll(e => e.Email == email && e.Isdel == false && e.IsDeleted == false);

            var investEmployees = await mainRepositoryManager.InvestEmployeeRepository.GetAll(e => e.IsSub == false);
            long maxEmpId = Convert.ToInt64(investEmployees.Max(b => Convert.ToInt64(b.EmpId))) + 1;
            var newEmpId = maxEmpId.ToString();


            if (ChkEmpid.Count() == 0 && ChkEmpEmail.Count() == 0 && createEmp == "1")
            {
                var newEmp = new Domain.Main.InvestEmployee
                {
                    EmpId = newEmpId,
                    EmpName = empName,
                    EmpName2 = empName,
                    BranchId = branchId,
                    UserId = 1,
                    JobType = 2,
                    JobCatagoriesId = 0,
                    StatusId = 1,
                    CheckDevice = false,
                    CheckDeviceActive = false,
                    FacilityId = (int?)facilityId,
                    Email = email,
                    EmpCode2 = "",
                    IdNo = ""
                };
                var addedEmployee = await mainRepositoryManager.InvestEmployeeRepository.AddAndReturn(newEmp);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

            }
            else
            {
                var getempId = await mainRepositoryManager.InvestEmployeeRepository.GetAll(e => (e.EmpId == UserName || e.Email == email) && e.Isdel == false && e.IsDeleted == false); ;
                if (getempId != null)
                {
                    empId = getempId.Select(x => x.Id).FirstOrDefault();
                    empName = getempId.Select(x => x.EmpName).FirstOrDefault() ?? "";

                }

            }


            NewUser.UserName = empCode;
            NewUser.UserFullname = empName;
            NewUser.Email = email;
            NewUser.StringUserPassword = password;
            NewUser.UserTypeId = 1;
            NewUser.IsDeleted = false;
            NewUser.Isdel = false;
            NewUser.FacilityId = facilityId;
            NewUser.EmpId = (int?)empId;
            NewUser.GroupsId = groupId;
            NewUser.UserPkId = branchId;
            NewUser.UserType2Id = 0;
            NewUser.ProjectsId = "";
            NewUser.SupId = 0;
            NewUser.CusId = 0;
            NewUser.SalesType = 0;
            NewUser.Isupdate = true;
            NewUser.CandId = 0;
            NewUser.CreatedBy = 1;

            string add = await mainRepositoryManager.StoredProceduresRepository.AddUserByProcedure(NewUser);
            await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

        }

        public async Task<IResult<SysUserVw>> LoginByUserAD(string userName, long facilityId, CancellationToken cancellationToken = default)
        {
            try
            {
                var currentTime = DateTime.Now.TimeOfDay;

                var user = await mainRepositoryManager.SysUserRepository.GetOneVw(x => x.UserName == userName && x.FacilityId == facilityId && x.Isdel == false && x.IsDeleted == false && (x.TimeFrom == null || x.TimeTo == null || (currentTime >= x.TimeFrom && currentTime <= x.TimeTo)));

                return await Result<SysUserVw>.SuccessAsync(user, "");
            }
            catch (Exception exp)
            {
                return await Result<SysUserVw>.FailAsync($"EXP: {exp.Message}");
            }
        }

        public async Task<IResult<SysUserVw>> Login2(LoginDto login, CancellationToken cancellationToken = default)
        {
            try
            {
                if (login == null) return await Result<SysUserVw>.FailAsync($"Error in login of: {GetType()}, the passed entity is NULL !!!.");

                if (string.IsNullOrEmpty(login.UserName))
                {
                    return await Result<SysUserVw>.FailAsync($"Username is required");
                }

                if (string.IsNullOrEmpty(login.Password) || login.Password.Count() < 3)
                {
                    return await Result<SysUserVw>.FailAsync($"Password is required");
                }

                var user = await userRepository.Login2(login.UserName, login.Password, login.FacilityId);
                if (user == null)
                {
                    return await Result<SysUserVw>.FailAsync("اسم المستخدم او كلمة السر غير صحيح");
                }
                return await Result<SysUserVw>.SuccessAsync(user, "");
            }
            catch (Exception exp)
            {
                return await Result<SysUserVw>.FailAsync($"EXP: {exp.Message}");
            }
        }


        public async Task<IResult<bool>> OTP(SysUserVw entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                return await Result<bool>.FailAsync($"Error: Null entity passed to OTP.");
            }

            try
            {
                long facilityId = entity.FacilityId ?? 1;
                var active = await sysConfigurationAppHelper.GetValue(139, facilityId);
                if (active == null || active == "0")
                {
                    return await Result<bool>.SuccessAsync(false, "");
                }

                var user = await mainRepositoryManager.SysUserRepository.GetOne(x => x.UserName == entity.UserName && x.FacilityId == entity.FacilityId && x.Isdel == false && x.IsDeleted == false);
                if (user == null)
                {
                    return await Result<bool>.FailAsync("المستخدم غير موجود");
                }
                if (string.IsNullOrEmpty(user.Email))
                {
                    return await Result<bool>.FailAsync("الايميل غير موجود");
                }

                var expiryTime = DateHelper.GetCurrentDateTime().AddMinutes(2);
                var verificationCode = GenerateOTP();

                var emailMessage = $"Please verify the following code to login to Logix: \nLogix Two-Factor-Auth Code: {verificationCode}";
                var smsMessage = $"Logix Two-Factor-Auth Code: {verificationCode}";

                // إرسال الرسالة بناءً على نوع التحقق
                bool isSent = false;
                switch (active)
                {
                    case "1":
                    case "3":
                        if (entity.TwoFactorType == 1)
                        {
                            isSent = await SendSms(entity.EmpId, smsMessage, facilityId);
                        }
                        else if (entity.TwoFactorType == 2 && active == "3")
                        {
                            await emailAppHelper.SendEmailAsync(user.Email, "Logix Two-Factor-Auth Code", emailMessage);
                            isSent = true;
                        }
                        break;
                    case "2":
                        if (entity.TwoFactorType == 2)
                        {
                            await emailAppHelper.SendEmailAsync(user.Email, "Logix Two-Factor-Auth Code", emailMessage);
                            isSent = true;
                        }
                        break;
                    default:
                        return await Result<bool>.FailAsync("تأكد من طريقة ارسال بيانات التحقق بخطوتين من مدير النظام");
                }

                if (isSent)
                {
                    // تحديث الرمز في جدول المستخدمين بعد تأكيد الإرسال
                    user.Otp = verificationCode;
                    user.OtpExpiry = expiryTime;
                    mainRepositoryManager.SysUserRepository.Update(user);
                    await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                    return await Result<bool>.SuccessAsync(true, "");
                }

                return await Result<bool>.FailAsync("Please verify the phone number");
            }
            catch (Exception exp)
            {
                return await Result<bool>.FailAsync($"Error in OTP: {exp.Message}");
            }
        }


        public async Task<Wrapper.IResult> UpdateUSERLogTime(long userId, string? ipAddress = "", CancellationToken cancellationToken = default)
        {
            try
            {
                // الحصول على المستخدم من SYS_USER
                var user = await mainRepositoryManager.SysUserRepository.GetOne(x => x.Id == userId && x.Isdel == false && x.IsDeleted == false);
                if (user == null)
                {
                    return await Result.FailAsync("المستخدم غير موجود");
                }

                // تحديث وقت تسجيل الخروج وجعل المستخدم في حالة غير متصل في SYS_USER_LogTime
                var logoutTimeUpdated = await mainRepositoryManager.SysUserLogTimeRepository.GetAll(logTime => logTime.UserId == userId && logTime.LogoutTime == null);
                if (logoutTimeUpdated != null)
                {
                    foreach (var item in logoutTimeUpdated)
                    {
                        item.Offline = true;
                        item.LogoutTime = DateHelper.GetCurrentDateTime();
                    }
                    mainRepositoryManager.SysUserLogTimeRepository.UpdateAll(logoutTimeUpdated);
                    await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                }

                // تحديث آخر وقت تسجيل الدخول في SYS_USER
                user.LastLogin = DateTime.UtcNow;
                mainRepositoryManager.SysUserRepository.Update(user);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                // إدراج سجل جديد في SYS_USER_LogTime
                var logTime = new SysUserLogTime
                {
                    UserId = userId,
                    LoginTime = DateHelper.GetCurrentDateTime(),
                    Offline = false,
                    IpAddress = ipAddress
                };
                await mainRepositoryManager.SysUserLogTimeRepository.Add(logTime);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                // تأكيد نجاح العمليات الثلاث
                return await Result.SuccessAsync();
            }
            catch (Exception ex)
            {
                return await Result.FailAsync($"حدث خطأ أثناء تحديث سجل وقت تسجيل المستخدم: {ex.Message}");
            }
        }

        public async Task<Wrapper.IResult> InsertTask(long userId, long facilityId, CancellationToken cancellationToken = default)
        {
            try
            {
                var dateObj = DateHelper.GetCurrentDateTime().ToString("yyyy/MM/dd",CultureInfo.InvariantCulture);
                var currentDayOfWeek = (int)DateHelper.GetCurrentDateTime().DayOfWeek;

                var tasks = await GetSysTasksSchedulerInsert(userId);
                if (tasks == null || !tasks.Any())
                {
                    return await Result.SuccessAsync("لا توجد مهام مجدولة لهذا المستخدم.");
                }

                foreach (var task in tasks)
                {
                    int type = task.Type ?? 0;
                    var taskToInsert = new TsTask
                    {
                        AssigneeToUserId = task.AssigneeToUserId,
                        DueDate = dateObj,
                        Message = task.Message,
                        ParentId = 0,
                        DeptId = 0,
                        Priority = 3,
                        RequiredApprovalPercentage = 50,
                        SendEmailNotifications = false,
                        StatusId = 1,
                        Subject = task.Subject,
                        UserId = task.CreatedBy,
                        WorkFlowType = 1,
                        ProjectId = 0,
                        SendDate = dateObj,
                        FacilityId = facilityId
                    };

                    if (type == 1) // يومي
                    {
                        if (!await TaskExists(taskToInsert))
                        {
                            await AddTask(taskToInsert, cancellationToken);
                        }
                    }
                    else if (type == 2) // أسبوعي
                    {
                        if (task.DaysWeek.Contains(currentDayOfWeek.ToString()))
                        {
                            if (!await TaskExists(taskToInsert))
                            {
                                await AddTask(taskToInsert, cancellationToken);
                            }
                        }
                    }
                    else if (type == 3) // شهري
                    {
                        if (GetYMonthYear(dateObj) + task.MonthDay == dateObj)
                        {
                            if (!await TaskExists(taskToInsert))
                            {
                                await AddTask(taskToInsert, cancellationToken);
                            }
                        }
                    }
                }

                return await Result.SuccessAsync();
            }
            catch (Exception ex)
            {
                return await Result.FailAsync($"حدث خطأ أثناء إضافة المهام المجدولة: {ex.Message}");
            }
        }
        public async Task<IResult<SysUserVw>> loginWithAuthenticationCode(LoginWithAuthenticationCodeDto login, CancellationToken cancellationToken = default)
        {
            try
            {
                if (login == null) return await Result<SysUserVw>.FailAsync($"Error in login of: {GetType()}, the passed entity is NULL !!!.");

                if (string.IsNullOrEmpty(login.UserName))
                {
                    return await Result<SysUserVw>.FailAsync($"Username is required");
                }

                if (string.IsNullOrEmpty(login.Password) || login.Password.Count() < 3)
                {
                    return await Result<SysUserVw>.FailAsync($"Password is required");
                }
                if (string.IsNullOrEmpty(login.Code))
                {
                    return await Result<SysUserVw>.FailAsync($"كود التحقق");
                }

                var user = await userRepository.Login2(login.UserName, login.Password, login.FacilityId);
                if (user == null)
                {
                    return await Result<SysUserVw>.FailAsync("اسم المستخدم او كلمة السر غير صحيح");
                }
                if (user.Otp != login.Code)
                {
                    return await Result<SysUserVw>.FailAsync("كود التحقق غير صحيح");
                }
                if (user.OtpExpiry < DateHelper.GetCurrentDateTime())
                {
                    var message = "لقد انتهى وقت التحقق. الرجاء إعادة إرسال الرمز مرة أخرى";
                    return await Result<SysUserVw>.FailAsync(message);
                }
                return await Result<SysUserVw>.SuccessAsync(user, "");
            }
            catch (Exception exp)
            {
                return await Result<SysUserVw>.FailAsync($"EXP: {exp.Message}");
            }
        }
        public async Task<IResult<string>> ResendCode(LoginResendCodeDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                return await Result<string>.FailAsync("Error: Null entity passed to ResendCode.");
            }

            try
            {
                long facilityId = entity.FacilityId;
                var userExistResult = await userRepository.Login2(entity.UserName, entity.Password, entity.FacilityId);

                if (userExistResult == null)
                {
                    return await Result<string>.FailAsync("المستخدم غير موجود");
                }

                var user = await mainRepositoryManager.SysUserRepository.GetOne(u => u.Id == userExistResult.UserId);
                if (user == null)
                {
                    return await Result<string>.FailAsync("المستخدم غير موجود");
                }

                string verificationCode = GenerateOTP();
                string emailMessage = $"Please verify the following code to login to Logix: \n Logix Two-Factor-Auth Code: {verificationCode}";
                string smsMessage = $"Logix Two-Factor-Auth Code: {verificationCode}";

                bool isSent = false;
                string successMessage = string.Empty;
                string failMessage = entity.Language == 1 ? "تأكد من صحة رقم الهاتف" : "Please verify the phone number";

                if (userExistResult.TwoFactorType == 1)
                {
                    isSent = await SendSms(userExistResult.EmpId, smsMessage, userExistResult.FacilityId ?? 1);
                    successMessage = entity.Language == 1 ? "تم ارسال كود الى رقم الهاتف ......." : "An OTP has been sent to the mobile Number .......";
                }
                else if (userExistResult.TwoFactorType == 2)
                {
                    if (string.IsNullOrEmpty(userExistResult.UserEmail))
                    {
                        return await Result<string>.FailAsync("الايميل غير موجود");
                    }
                    await emailAppHelper.SendEmailAsync(userExistResult.UserEmail, "Logix Two-Factor-Auth Code", emailMessage);
                    isSent = true;
                    successMessage = entity.Language == 1 ? "تم ارسال رمز التحقق على الايميل." : "Verification code has been sent to your email.";
                }
                else
                {
                    return await Result<string>.FailAsync("تأكد من طريقة ارسال بيانات التحقق بخطوتين من مدير النظام");
                }

                if (isSent)
                {
                    var expiryTime = DateHelper.GetCurrentDateTime().AddMinutes(2);
                    user.Otp = verificationCode;
                    user.OtpExpiry = expiryTime;
                    mainRepositoryManager.SysUserRepository.Update(user);
                    await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                    return await Result<string>.SuccessAsync(successMessage);
                }

                return await Result<string>.FailAsync(failMessage);
            }
            catch (Exception ex)
            {
                return await Result<string>.FailAsync($"Error in ResendCode: {ex.Message}");
            }
        }
        public async Task<IResult<SysUserVw>> LoginByAzure(string email, long facilityId, string AzureIDToken = "", CancellationToken cancellationToken = default)
        {
            try
            {
                var currentTime = DateTime.Now.TimeOfDay;

                var uservw = await mainRepositoryManager.SysUserRepository.GetOneVw(x => x.UserEmail == email && x.FacilityId == facilityId && x.Isdel == false && x.IsDeleted == false && (x.TimeFrom == null || x.TimeTo == null || (currentTime >= x.TimeFrom && currentTime <= x.TimeTo)));
                if (uservw == null)
                {
                    return await Result<SysUserVw>.FailAsync($"Emp Not Found");

                }
                /// if user not null we will update azuretoken of user 

                var user = await mainRepositoryManager.SysUserRepository.GetOne(x => x.Id == uservw.UserId);
                if (user == null)
                {
                    return await Result<SysUserVw>.FailAsync($"Emp Not Found");

                }
                user.AzureToken = AzureIDToken;
                user.AzureTokenExpiryDate = DateHelper.GetCurrentDateTime().AddDays(1);
                mainRepositoryManager.SysUserRepository.Update(user);
                await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                return await Result<SysUserVw>.SuccessAsync(uservw, "");
            }
            catch (Exception exp)
            {
                return await Result<SysUserVw>.FailAsync($"EXP: {exp.Message}");
            }
        }
        public async Task<IResult<bool>> CreateEmpAzureAsync(string userEmail, string empName, long facilityId, string AzureIDToken = "")
        {
            try
            {
                int branchId = 0;
                var createEmp = await sysConfigurationAppHelper.GetValue(211, 1);
                var createUser = await sysConfigurationAppHelper.GetValue(435, 1);
                var GetbranchId = await sysConfigurationAppHelper.GetValue(170, 1);
                var groupId = await sysConfigurationAppHelper.GetValue(169, 1);

                if (string.IsNullOrEmpty(GetbranchId))
                {
                    return await Result<bool>.FailAsync(localization.GetCommonResource("BranchError"));
                }
                branchId = Convert.ToInt32(branchId);
                if (string.IsNullOrEmpty(groupId))
                {
                    return await Result<bool>.FailAsync(localization.GetCommonResource("InvalidGroupId"));

                }


                var GetEmployeeByEmail = await mainRepositoryManager.InvestEmployeeRepository.GetAll(e => e.Email == userEmail && e.Isdel == false && e.IsDeleted == false);
                var empId = GetEmployeeByEmail.Select(x => x.Id).FirstOrDefault();
                if (empId == 0 && createEmp == "1")
                {
                    var EmployeeId = await InsertHREmp(empName, empName, facilityId, branchId, userEmail, groupId);

                    await AddUserAzure(EmployeeId, empName, userEmail, facilityId, groupId, branchId, AzureIDToken);
                    return await Result<bool>.SuccessAsync(true, "Employee and User created successfully.");
                }
                else if (createUser == "1")
                {
                    await AddUserAzure(empId, empName, userEmail, facilityId, groupId, branchId, AzureIDToken);
                    return await Result<bool>.SuccessAsync(true, "User created successfully.");
                }

                return await Result<bool>.FailAsync("User creation failed.");
            }
            catch (Exception exc)
            {
                return await Result<bool>.FailAsync($"EXP in {this.GetType().Name}, Message: {exc.Message}");
            }
        }
        private async Task<long> InsertHREmp(string UserName, string empName, long facilityId, int branchId, string email, string groupId, CancellationToken cancellationToken = default)
        {


            var investEmployees = await mainRepositoryManager.InvestEmployeeRepository.GetAll(e => e.IsSub == false);
            long maxEmpId = Convert.ToInt64(investEmployees.Max(b => Convert.ToInt64(b.EmpId))) + 1;
            var newEmpId = maxEmpId.ToString();
            var newEmp = new Domain.Main.InvestEmployee
            {
                EmpId = newEmpId,
                EmpName = empName,
                EmpName2 = empName,
                BranchId = branchId,
                UserId = 1,
                JobType = 2,
                JobCatagoriesId = 0,
                StatusId = 1,
                CheckDevice = false,
                CheckDeviceActive = false,
                FacilityId = (int?)facilityId,
                Email = email,
                EmpCode2 = "",
                IdNo = ""
            };
            var addedEmployee = await mainRepositoryManager.InvestEmployeeRepository.AddAndReturn(newEmp);
            await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

            return (addedEmployee.Id);
        }


        private async Task AddUserAzure(long Id, string name, string email, long facilityId, string groupId, int branchId, string AzureIDToken = "", CancellationToken cancellationToken = default)
        {
            var NewUser = new SysUser();

            NewUser.UserName = email;
            NewUser.UserFullname = name;
            NewUser.Email = email;
            NewUser.UserPassword = null;
            NewUser.UserTypeId = 1;
            NewUser.IsDeleted = false;
            NewUser.Isdel = false;
            NewUser.FacilityId = facilityId;
            NewUser.EmpId = (int?)Id;
            NewUser.GroupsId = groupId;
            NewUser.UserPkId = branchId;
            NewUser.UserType2Id = 0;
            NewUser.ProjectsId = "";
            NewUser.SupId = 0;
            NewUser.CusId = 0;
            NewUser.SalesType = 0;
            NewUser.Isupdate = true;
            NewUser.CandId = 0;
            NewUser.CreatedBy = 1;
            NewUser.IsAgree = false;
            NewUser.AzureToken = AzureIDToken;
            // في حال تم اضافة بيانات الفروع على الازور سنقوم بتحديثها
            NewUser.BranchsId = "0";

            var add = await mainRepositoryManager.SysUserRepository.AddAndReturn(NewUser);
            await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

        }



        public async Task<IResult<string>> Logout(CancellationToken cancellationToken = default)
        {
            try
            {
                var AzureToken = "";

                // Update logout time and set user as offline in SYS_USER_LogTime
                var logoutTimeUpdated = await mainRepositoryManager.SysUserLogTimeRepository.GetAll(logTime => logTime.UserId == session.UserId && logTime.LogoutTime == null);
                if (logoutTimeUpdated != null)
                {
                    foreach (var item in logoutTimeUpdated)
                    {

                        item.Offline = true;
                        item.LogoutTime = DateHelper.GetCurrentDateTime();
                    }
                    mainRepositoryManager.SysUserLogTimeRepository.UpdateAll(logoutTimeUpdated);
                    await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                }

                var GetCurrentUser = await mainRepositoryManager.SysUserRepository.GetOne(x => x.Id == session.UserId && x.IsDeleted == false && x.Isdel == false);
                if (GetCurrentUser == null)
                {

                    return await Result<string>.FailAsync($"حدث خطأ أثناء تسجيل الخروج : يبدو ان هناك مشكله في العثور على المستخدم الحالي");

                }
                AzureToken = GetCurrentUser?.AzureToken ?? "";
                int azureLogged = session.IsAzureAuthenticated;
                // سيتم تعديلها عند اضافة الخاصية
                bool casLogged = false;
                if (azureLogged == 1)
                {
                    string tenantId = await sysConfigurationAppHelper.GetValue( 408,1);
                    if (string.IsNullOrEmpty(tenantId))
                    {
                        return await Result<string>.FailAsync("Tenant ID is not configured.");
                    }

                    var redirectUri = config["FrontEndInfo:DomainUrl"];
                    string logoutUrl = $"https://login.microsoftonline.com/{tenantId.Trim()}/oauth2/v2.0/logout?post_logout_redirect_uri={HttpUtility.UrlEncode(redirectUri)}";

                    if (!string.IsNullOrEmpty(AzureToken))
                    {
                        logoutUrl += $"&id_token_hint={HttpUtility.UrlEncode(AzureToken)}";
                    }

                    GetCurrentUser.AzureToken = "";
                    GetCurrentUser.AzureTokenExpiryDate = null;
                    mainRepositoryManager.SysUserRepository.Update(GetCurrentUser);
                    await mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                    return await Result<string>.SuccessAsync(logoutUrl);
                }



                else if (casLogged)
                {
                    string casServerUrl = await sysConfigurationAppHelper.GetValue(1, 433);
                    if (!casServerUrl.EndsWith("/"))
                    {
                        casServerUrl += "/";
                    }
                    string serviceUrl = Regex.Replace(await sysConfigurationAppHelper.GetValue(1, 434), "loginRedirect", "Default", RegexOptions.IgnoreCase);
                    string logoutUrl = $"{casServerUrl}logout?service={HttpUtility.UrlEncode(serviceUrl)}";
                    return await Result<string>.SuccessAsync(logoutUrl); // Return the CAS logout URL
                }
                else
                {
                    return await Result<string>.SuccessAsync("/"); // Return the default redirect URL
                }
            }
            catch (Exception ex)
            {
                return await Result<string>.FailAsync($"حدث خطأ أثناء تسجيل الخروج: {ex.Message}");
            }
        }



        #region Auxiliary Functions


        private string GenerateOTP()
        {
            int length = 4;
            string numbers = "0123456789";
            Random objRandom = new Random();
            string strRandom = string.Empty;
            int noOfNumbers = length;

            for (int i = 0; i < noOfNumbers; i++)
            {
                int temp = objRandom.Next(0, numbers.Length);
                strRandom += numbers[temp];
            }

            return strRandom;
        }

        public async Task<bool> SendSms(int? EmpId, string Message, long facilityId)
        {
            try
            {
                // Fetch employee data based on mobile number
                var empResult = await mainRepositoryManager.InvestEmployeeRepository.GetOne(e => e.Id == EmpId);

                if (empResult == null || string.IsNullOrEmpty(empResult.Mobile))
                    return false;
                var UserId = (int)session.UserId;
                if (!Regex.IsMatch(empResult.Mobile, @"^\d{10,15}$"))
                    return false;
                var formattedMobile = FormatPhoneNumber(empResult.Mobile);
                var isSend = await sendSmsHelper.SendSms(formattedMobile, Message, false, false, FacilityId: facilityId);
                if (!isSend) return false;
                return true;

            }

            catch (Exception)
            {
                return false;

            }
        }

        private string FormatPhoneNumber(string phoneNumber)
        {
            // Example method to format phone numbers
            return phoneNumber.Replace("-", "").Replace(" ", "");
        }
        private async Task<bool> TaskExists(TsTask task)
        {
            // تحقق مما إذا كانت المهمة موجودة بالفعل
            var existingTask = await tsRepositoryManager.TsTaskRepository.GetOne(x => x.AssigneeToUserId == task.AssigneeToUserId && x.DueDate == task.DueDate && x.Subject == task.Subject);
            return existingTask != null;
        }

        private async Task AddTask(TsTask task, CancellationToken cancellationToken)
        {
            await tsRepositoryManager.TsTaskRepository.Add(task);
            await tsRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
        }

        private async Task<IEnumerable<TsTasksScheduler>> GetSysTasksSchedulerInsert(long assigneeToUserId)
        {
            // الحصول على المهام المجدولة حيث IsDeleted = false
            var tasks = await tsRepositoryManager.TsTasksSchedulerRepository.GetAll(x => x.IsDeleted == false);

            // تطبيق الشروط الأخرى على النتيجة
            var filteredTasks = tasks.Where(task => !string.IsNullOrEmpty(task.AssigneeToUserId) && task.AssigneeToUserId.Split(',').Contains(assigneeToUserId.ToString())).ToList();

            return filteredTasks;
        }

        private string GetYMonthYear(string dateData)
        {
            if (dateData.Length >= 8)
            {
                return dateData.Substring(0, 8);
            }
            return string.Empty;
        }

        #endregion

    }
}
