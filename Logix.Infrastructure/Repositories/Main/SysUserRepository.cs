using Castle.Windsor.Installer;
using Logix.Application.DTOs.HR;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using Logix.Domain.PM;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Polly;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using WhatsappBusiness.CloudApi.Webhook;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysUserRepository : GenericRepository<SysUser, SysUserVw>, ISysUserRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;

        public SysUserRepository(ApplicationDbContext context,
            IConfiguration configuration) : base(context)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public async Task<IEnumerable<SysUserVw>> GetUsersByEmpId(int empId, long userId)
        {
            return await context.SysUserVws.Where(u => u.EmpId == empId && u.EmpId != 0 && u.UserId != userId && u.Enable == 1 && u.Isdel == false).ToListAsync();
        }

        public async Task<SysUserVw> Login(string username, string password, long facilityId)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    throw new ArgumentNullException("يرجى ادخال اسم المستخدم وكلمة السر");
                }
                var getuser = context.SysUserVws.FromSqlRaw("SELECT * FROM [SYS_USER_VW] WHERE USER_NAME={0} AND dbo.Sys_DECRYPT(USER_PASSWORD)={1} and isdel=0 and isagree=1 and enable=1", username, password);
                if (getuser == null || getuser.Count() != 1)
                {
                    throw new ArgumentException("اسم المستخدم او كلمة السر غير صحيح");
                }
                return await getuser.FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<SysUserVw> Login2(string username, string password, long facilityId)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    throw new ArgumentNullException("يرجى ادخال اسم المستخدم وكلمة السر");
                }

                var getuser = await context.SysUserVws.FromSqlRaw(
                    @"SELECT * 
              FROM [SYS_USER_VW] 
              WHERE USER_NAME = {0} 
              AND dbo.Sys_DECRYPT(USER_PASSWORD) = {1} 
              AND isdel = 0 
              AND enable = 1 
              AND IsDeleted = 0 
              AND Facility_ID = {2}  
              AND ((CAST(GETDATE() AS TIME) BETWEEN Time_From AND Time_To) 
              OR Time_From IS NULL 
              OR Time_To IS NULL)",
                    username, password, facilityId).ToListAsync();

                if (getuser == null || getuser.Count != 1)
                {
                    throw new ArgumentException("اسم المستخدم او كلمة السر غير صحيح");
                }

                return getuser.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("حدث خطأ أثناء محاولة تسجيل الدخول: " + ex.Message);
            }
        }






        public async Task<bool> DisableUserByEmpID(int empId)
        {
            try
            {
                var user = await context.Set<SysUser>().FirstOrDefaultAsync(x => x.EmpId == empId);

                if (user != null)
                {
                    user.Enable = 0;

                    await context.SaveChangesAsync();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine($"EXP in DisableUserByEmpID at ({this.GetType()}), Message: {exp.Message} --- {(exp.InnerException != null ? "InnerExp: " + exp.InnerException.Message : "no inner")}");
                return false;
            }
        }

        public async Task<DataTable> GetUserMailBox(int ReferralsToUserId, int ReferralsToDepId)
        {
            try
            {
                using (SqlConnection objCnn = new SqlConnection(configuration.GetConnectionString("LogixLocal")))
                using (SqlCommand objCmd = objCnn.CreateCommand())
                {
                    objCmd.CommandType = CommandType.Text;
                    objCmd.CommandText = "SELECT top 10 *,Reff_TBL.Is_Read,Reff_TBL.Referral_ID  FROM Adco_Transactions_VW outer apply (select top 1 Is_Read,ID as Referral_ID from Adco_Tarnsactions_Referrals where Transaction_ID=Adco_Transactions_VW.ID and IsDeleted=0 order by ID desc) as Reff_TBL where IsDeleted=0 and Status_ID in(1,2)";

                    if (ReferralsToUserId > 0)
                        objCmd.CommandText += " and (Referrals_To_User_ID=@Referrals_To_User_ID or  Referrals_To_Dep_ID in(" + ReferralsToUserId.ToString() + ") and Referrals_To_User_ID=0 )";

                    objCmd.Parameters.AddWithValue("@Referrals_To_User_ID", ReferralsToUserId);
                    objCmd.Parameters.AddWithValue("@Referrals_To_Dep_ID", ReferralsToDepId);
                    objCmd.CommandTimeout = 600;

                    await objCnn.OpenAsync();
                    var result = objCmd.ExecuteReader();
                    DataTable dt = new();
                    dt.Load(result);

                    return dt;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<string?> GetUserPosting(long userId)
        {
            try
            {
                return await context.SysUsers
                    .Where(x => x.Id == userId && x.Isdel == false)
                    .Select(x => x.AccTransfer)
                    .SingleOrDefaultAsync();
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        
        

        public async Task<long?> CheckPassword(long userId, string? password)
        {
            try
            {
                // تحقق من صحة المدخلات
                if (userId <= 0 || string.IsNullOrEmpty(password))
                {
                    return 0;
                }

                // استعلام SQL مع كيان مخصص
                var result = await context.Set<UserCountResult>()
                    .FromSqlInterpolated($@"
                SELECT COUNT(USER_ID) AS UserCount
                FROM SYS_USER
                WHERE USER_ID = {userId} 
                  AND dbo.Sys_DECRYPT(USER_PASSWORD) = {password}
                  AND isdel = 0 
                  AND enable = 1")
                    .FirstOrDefaultAsync();

                return result?.UserCount;
            }
            catch (Exception)
            {
                // إعادة رمي الخطأ للحفاظ على تفاصيل الاستثناء
                throw;
            }
        }

        public async Task<bool> ChangePassword(long userId, string? newPassword, string email)
        {
            try
            {
                // تحقق من صحة المدخلات
                if (userId <= 0 || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(email))
                {
                    throw new ArgumentException("User ID, password, and email are required.");
                }

                // استعلام SQL لتحديث كلمة المرور والبريد الإلكتروني
                var sql = @"
            UPDATE SYS_USER 
            SET 
                USER_PASSWORD = dbo.Sys_ENCRYPT(@NewPassword),
                USER_EMAIL = @Email,
                Isupdate = 1
            WHERE 
                USER_ID = @UserId 
                AND isdel = 0 
                AND [Enable] = 1";

                // تنفيذ الاستعلام
                var result = await context.Database.ExecuteSqlRawAsync(sql,
                    new SqlParameter("@NewPassword", newPassword),
                    new SqlParameter("@Email", email),
                    new SqlParameter("@UserId", userId));

                // إذا كان `result` أكبر من 0، فهذا يعني أن التحديث قد نجح
                return result > 0;
            }
            catch (Exception ex)
            {
                // سجل الاستثناء أو قم بمعالجته
                throw new InvalidOperationException("Error updating password.", ex);
            }
        }
        public async Task<bool> UpdateOTP(string otp, long userId)
        {
            try
            {
                // التحقق من المدخلات
                if (string.IsNullOrEmpty(otp) || userId <= 0)
                {
                    throw new ArgumentException("Invalid OTP or userId.");
                }

                // البحث عن المستخدم
                var user = await context.SysUsers.FirstOrDefaultAsync(x => x.Id == userId);

                if (user != null)
                {
                    // تحديث القيم
                    user.Otp = otp;
                    user.OtpExpiry = DateTime.Now.AddMinutes(2); // إضافة دقيقتين إلى الوقت الحالي

                    // حفظ التغييرات
                    await context.SaveChangesAsync();
                    return true;
                }

                // إذا لم يتم العثور على المستخدم
                return false;
            }
            catch (Exception ex)
            {
                // سجل الاستثناء أو قم بمعالجته
                throw new InvalidOperationException("Error updating OTP.", ex);
            }
        }



    }
}
