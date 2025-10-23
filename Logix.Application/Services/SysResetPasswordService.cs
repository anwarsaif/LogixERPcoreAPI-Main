using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Office;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Helpers;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using Logix.Domain.PM;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text.RegularExpressions;

namespace Logix.Application.Services.Main
{
    public class SysResetPasswordService : GenericQueryService<SysResetPassword, SysResetPasswordDto, SysResetPassword>, ISysResetPasswordService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;
        private readonly IEmailAppHelper emailAppHelper;
        private readonly ISendSmsHelper sendSmsHelper;

        public SysResetPasswordService(IQueryRepository<SysResetPassword> queryRepository,
            IMapper mapper,
            IMainRepositoryManager mainRepositoryManager,
            ICurrentData session,
            ILocalizationService localization,
            IEmailAppHelper emailAppHelper,
            ISendSmsHelper sendSmsHelper) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.session = session;
            this.localization = localization;
            this.emailAppHelper = emailAppHelper;
            this.sendSmsHelper = sendSmsHelper;
        }

        public Task<IResult<SysResetPasswordDto>> Add(SysResetPasswordDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<SysResetPasswordEditDto>> Update(SysResetPasswordEditDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<ResetPasswordResultDto>> ValidateVerificationCode(string verificationCode, int verificationType, string userName, int Language, CancellationToken cancellationToken = default)
        {
            try
            {
                // Step 1: Fetch the User By UserName

                var userResult = await _mainRepositoryManager.SysUserRepository.GetOne(u => u.UserName == userName
                && u.IsDeleted == false
                && u.Isdel == false
                && u.Enable == 1);

                if (userResult == null)
                {
                    var message = Language == 1
                        ? "اسم المستخدم غير صحيح"
                        : "Incorrect username";
                    return await Result<ResetPasswordResultDto>.FailAsync(message);
                }
                // Step 2: Fetch the verification entry
                var resetPasswordEntry = await _mainRepositoryManager.SysResetPasswordRepository.GetOne(
                    r => r.VerificationCode == verificationCode
                         && r.VerificationType == verificationType
                         && r.UserId == userResult.Id
                         && r.IsDeleted == false);

                if (resetPasswordEntry == null)
                {
                    var message = "الرمز الذي تم إدخاله غير صحيح";
                    return await Result<ResetPasswordResultDto>.FailAsync(message);
                }

                // Step 3: Check for expiry
                if (resetPasswordEntry.ExpiryTime < DateHelper.GetCurrentDateTime())
                {
                    var message = "لقد انتهى وقت التحقق. الرجاء إعادة إرسال الرمز مرة أخرى";
                    return await Result<ResetPasswordResultDto>.FailAsync(message);
                }

                var result = new ResetPasswordResultDto
                {
                    Email = resetPasswordEntry.Email,
                    VerificationCode = verificationCode,
                    ExpiryTime = resetPasswordEntry.ExpiryTime,
                    MobileNumber = resetPasswordEntry.MobileNumber,
                    VerificationType = resetPasswordEntry.VerificationType,
                    UserName = userName


                };
                return await Result<ResetPasswordResultDto>.SuccessAsync(result, "Verification code is valid.");
            }
            catch (Exception ex)
            {
                return await Result<ResetPasswordResultDto>.FailAsync($"Error during verification: {ex.Message}");
            }
        }


        public async Task<IResult<ResetPasswordResultDto>> SendVerificationCodeByEmail(ResetPasswordByEmail entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<ResetPasswordResultDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");
            try
            {
                var ExpiryTime = DateHelper.GetCurrentDateTime().AddMinutes(2);
                var result = new ResetPasswordResultDto();
                await _mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);

                var email = entity.Email?.Trim();
                if (string.IsNullOrEmpty(email))
                {
                    var message = entity.Language == 1
                        ? "البريد الإلكتروني مطلوب"
                        : "Email is required";
                    return await Result<ResetPasswordResultDto>.FailAsync(message);
                }
                if (!Regex.IsMatch(email, @"^\S+@\S+\.\S+$"))
                {
                    var emailErrorMessage = entity.Language == 1
                        ? "صيغة البريد الإلكتروني غير صحيحة."
                        : "Invalid email format.";
                    return await Result<ResetPasswordResultDto>.FailAsync(emailErrorMessage);
                }

                var userResult = await _mainRepositoryManager.SysUserRepository.GetOne(
                    u => u.UserName == entity.UserName &&
                         u.IsDeleted == false &&
                         u.Enable == 1);

                if (userResult == null)
                {
                    var message = entity.Language == 1
                        ? "اسم المستخدم غير صحيح"
                        : "Incorrect username";

                    return await Result<ResetPasswordResultDto>.FailAsync(message);

                }
                // حذف جميع التحققات السابقة
                var GetAllPreviousVerificationCode = await _mainRepositoryManager.SysResetPasswordRepository.GetAll(x => x.IsDeleted == false && x.UserId == userResult.Id);
                if (GetAllPreviousVerificationCode.Count() > 0)
                {
                    foreach (var Verification in GetAllPreviousVerificationCode)
                    {
                        Verification.IsDeleted = true;
                    }
                    _mainRepositoryManager.SysResetPasswordRepository.UpdateAll(GetAllPreviousVerificationCode);
                    await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                }

                var VerificationCode = GenerateOTP();
                var resetPassword = new SysResetPassword
                {
                    Email = email,
                    UserId = (int?)userResult.Id,
                    IsUpdate = false,
                    ModifiedBy = null,
                    CreatedBy = userResult.Id,
                    CreatedOn = DateHelper.GetCurrentDateTime(),
                    DDate = DateHelper.GetCurrentDateTime(),
                    IsDeleted = false,
                    VerificationType = 2,
                    ExpiryTime = ExpiryTime,
                    VerificationCode = VerificationCode

                };

                var addedEntity = await _mainRepositoryManager.SysResetPasswordRepository.AddAndReturn(resetPassword);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var resetUrl = $"/ResetPassword.aspx";
                var messageContent = BuildEmailMessage(entity.UserName, resetUrl, VerificationCode);

                await emailAppHelper.SendEmailAsync(email, "Logix Reset Password", messageContent);


                var successMessage = entity.Language == 1
                    ? "تم ارسال رابط اعادة تعيين كلمة المرور الى بريدك الالكتروني."
                    : "Password reset link has been sent to your email.";

                result.UserName = entity.UserName;
                result.Email = entity.Email ?? "";
                result.CodeContent = messageContent ?? "";
                result.VerificationCode = VerificationCode ?? "";

                await _mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);

                return await Result<ResetPasswordResultDto>.SuccessAsync(result, successMessage);
            }
            catch (Exception)
            {
                return await Result<ResetPasswordResultDto>.FailAsync(localization.GetResource1("ErrorOccurredDuring"));
            }
        }

        public async Task<IResult<ResetPasswordResultDto>> SendVerificationCodeByMobile(ResetPasswordByMobile entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<ResetPasswordResultDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");
            try
            {
                await _mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);

                var result = new ResetPasswordResultDto();

                var ExpiryTime = DateHelper.GetCurrentDateTime().AddMinutes(2);

                entity.Language ??= 1;
                // Step 2: Check the user mobile number
                var userResult = await _mainRepositoryManager.SysUserRepository.GetOne(
                    u => u.UserName == entity.UserName
                    && u.IsDeleted == false
                    && u.Isdel == false
                    && u.Enable == 1
                );

                if (userResult == null)
                {
                    var message = entity.Language == 1
                        ? "اسم المستخدم غير صحيح"
                        : "Incorrect username";
                    return await Result<ResetPasswordResultDto>.FailAsync(message);
                }

                // Fetch employee data based on mobile number
                var empResult = await _mainRepositoryManager.InvestEmployeeRepository.GetOne(
                    e => e.Mobile == entity.Mobile
                    && e.Id == userResult.EmpId
                );

                if (empResult == null || string.IsNullOrEmpty(empResult.Mobile))
                {
                    var message = entity.Language == 1
                        ? "رقم الهاتف غير صحيح"
                        : "Incorrect mobile number";
                    return await Result<ResetPasswordResultDto>.FailAsync(message);
                }
                if (!Regex.IsMatch(empResult.Mobile, @"^\d{10,15}$"))
                {
                    var mobileErrorMessage = entity.Language == 1
                        ? "صيغة رقم الهاتف غير صحيحة."
                        : "Invalid mobile number format.";
                    return await Result<ResetPasswordResultDto>.FailAsync(mobileErrorMessage);
                }

                // Step 3: Format and check mobile number
                var formattedMobile = FormatPhoneNumber(empResult.Mobile);
                var formattedMobileToCheck = FormatPhoneNumber(entity.Mobile);
                if (formattedMobile != formattedMobileToCheck)
                {
                    var message = entity.Language == 1
                        ? "رقم الهاتف غير صحيح"
                        : "Incorrect mobile number";
                    return await Result<ResetPasswordResultDto>.FailAsync(message);
                }

                // حذف جميع التحققات السابقة
                var GetAllPreviousVerificationCode = await _mainRepositoryManager.SysResetPasswordRepository.GetAll(x => x.IsDeleted == false && x.UserId == userResult.Id);
                if (GetAllPreviousVerificationCode.Count() > 0)
                {
                    foreach (var Verification in GetAllPreviousVerificationCode)
                    {
                        Verification.IsDeleted = true;
                    }
                    _mainRepositoryManager.SysResetPasswordRepository.UpdateAll(GetAllPreviousVerificationCode);
                    await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                }
                var VerificationCode = GenerateOTP();
                var resetPassword = new SysResetPassword
                {
                    MobileNumber = formattedMobile,
                    UserId = (int?)userResult.Id,
                    IsUpdate = false,
                    ModifiedBy = null,
                    CreatedBy = userResult.Id,
                    CreatedOn = DateHelper.GetCurrentDateTime(),
                    DDate = DateHelper.GetCurrentDateTime(),
                    IsDeleted = false,
                    VerificationType = 1,
                    ExpiryTime = ExpiryTime,
                    VerificationCode = VerificationCode,

                };

                var addedEntity = await _mainRepositoryManager.SysResetPasswordRepository.AddAndReturn(resetPassword);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);


                // send sms 
                var Message = "يرجى التحقق من الكود التالي لتسجيل الدخول إلى لوجكس:  لوجكس رمز المصادقة الثنائية: " + VerificationCode;
                var isSend = await SendSms(formattedMobile, Message, 1);
                if (!isSend)
                    return await Result<ResetPasswordResultDto>.FailAsync($"{localization.GetMainResource("SendSmsFaild")}" + $"{localization.GetCommonResource("To")}  {entity.Mobile}");




                var lastThreeDigits = formattedMobile.Substring(formattedMobile.Length - 3);

                // Step 5: Return success message
                var successMessage = entity.Language == 1
                    ? $"تم ارسال كود الى الهاتف الذي اخرة .......{lastThreeDigits}"
                    : $"An OTP has been sent to the mobile ending with .......{lastThreeDigits}";

                result.UserName = entity.UserName;
                result.MobileNumber = entity.Mobile ?? "";
                result.VerificationCode = VerificationCode ?? "";

                await _mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);

                return await Result<ResetPasswordResultDto>.SuccessAsync(result, successMessage);
            }
            catch (Exception)
            {
                return await Result<ResetPasswordResultDto>.FailAsync(localization.GetResource1("ErrorOccurredDuring"));
            }
        }


        public async Task<IResult<ResetPasswordResultDto>> ResetPassword(string verificationCode, string Password, string userName, int Language, CancellationToken cancellationToken = default)
        {
            try
            {
                // Step 1: Fetch the User By UserName

                var userResult = await _mainRepositoryManager.SysUserRepository.GetOne(u => u.UserName == userName
                && u.IsDeleted == false
                && u.Isdel == false
                && u.Enable == 1);

                if (userResult == null)
                {
                    var message = Language == 1
                        ? "اسم المستخدم غير صحيح"
                        : "Incorrect username";
                    return await Result<ResetPasswordResultDto>.FailAsync(message);
                }
                // Step 2: Fetch the verification entry
                var resetPasswordEntry = await _mainRepositoryManager.SysResetPasswordRepository.GetOne(
                    r => r.VerificationCode == verificationCode
                         && r.UserId == userResult.Id
                         && r.IsDeleted == false);

                if (resetPasswordEntry == null)
                {
                    var message = "الرمز الذي تم إدخاله غير صحيح";
                    return await Result<ResetPasswordResultDto>.FailAsync(message);
                }


                byte[] GetEncryptedPassWord = await _mainRepositoryManager.StoredProceduresRepository.EncryptUserPassword(Password);


                await _mainRepositoryManager.UnitOfWork.BeginTransactionAsync(cancellationToken);

                // Step 3: Update sys user
                userResult.ModifiedOn = DateHelper.GetCurrentDateTime();
                userResult.UserPassword = GetEncryptedPassWord;
                _mainRepositoryManager.SysUserRepository.Update(userResult);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                // Step 4: Update  SysResetPassword
                resetPasswordEntry.ModifiedOn = DateHelper.GetCurrentDateTime();
                resetPasswordEntry.ModifiedBy = userResult.Id;
                resetPasswordEntry.IsUpdate = true;
                resetPasswordEntry.IsDeleted = true;
                _mainRepositoryManager.SysResetPasswordRepository.Update(resetPasswordEntry);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);
                var result = new ResetPasswordResultDto
                {
                    Email = resetPasswordEntry.Email,
                    VerificationCode = verificationCode,
                    ExpiryTime = resetPasswordEntry.ExpiryTime,
                    MobileNumber = resetPasswordEntry.MobileNumber,
                    VerificationType = resetPasswordEntry.VerificationType,
                    UserName = userName


                };


                await _mainRepositoryManager.UnitOfWork.CommitTransactionAsync(cancellationToken);

                return await Result<ResetPasswordResultDto>.SuccessAsync(result, "تهانينا لقد تم تغيير كلمة المرور الخاصة بك بنجاح.");
            }
            catch (Exception ex)
            {
                return await Result<ResetPasswordResultDto>.FailAsync($"Error during ResetPassword: {ex.Message}");
            }
        }


        #region Auxiliary functions-----دوال  مساعدة
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
        private string FormatPhoneNumber(string phoneNumber)
        {
            // Example method to format phone numbers
            return phoneNumber.Replace("-", "").Replace(" ", "");
        }

        private string BuildEmailMessage(string userName, string resetUrl, string verificationCode)
        {
            return $"<p style='text-align: left; line-height:1.5;font-size:15px;font-weight:600'>Hello {userName}!</p>" +
                   "<p style='text-align: left; line-height:1.5;font-size:14px;font-weight:600'>يبدو أنك نسيت كلمة المرور الخاصة بك، " +
                   "لإعادة تعيين كلمة المرور الخاصة بك يرجى الضغط على الزر التالي.</p>" +
                   "<p style='text-align: left; line-height:1.5;font-size:14px;font-weight:600'>كود التحقق الخاص بك هو: " +
                   $"<strong>{verificationCode}</strong></p>" +
                   $"<a href='{resetUrl}' style='font-weight:bold;text-align:center;display:inline-block;" +
                   "border-radius:50px;text-transform:capitalize;color: #fff !important; background-color:#426eb2;" +
                   "margin:0;border-color:#426eb2;border-style:solid;border-width:10px 20px' target='_blank'>إعادة الضبط الآن</a>" +
                   "<p style='line-height:1.5;font-size:14px;font-weight:400'>" +
                   $"<a style='color:#22bce5' href='{resetUrl}' target='_blank'>here</a> إذا لم يعمل الزر، انقر فوق." +
                   "</p>";
        }


        public async Task<bool> SendSms(string ReceiverMobile, string Message, long facilityId)
        {
            try
            {
                var UserId = (int)session.UserId;

                var isSend = await sendSmsHelper.SendSms(ReceiverMobile, Message, false, false, FacilityId: facilityId);
                if (!isSend) return false;
                return true;

            }

            catch (Exception)
            {
                return false;

            }
        }
       
        
        #endregion

    }
}
