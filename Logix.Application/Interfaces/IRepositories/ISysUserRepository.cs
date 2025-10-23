using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using Logix.Domain.PM;
using System.Data;
using System.Linq.Expressions;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysUserRepository : IGenericRepository<SysUser, SysUserVw>
    {
        Task<SysUserVw> Login(string username, string? password, long facilityId);
        Task<IEnumerable<SysUserVw>> GetUsersByEmpId(int empId, long userId);
        Task<bool> DisableUserByEmpID(int empId);
        Task<DataTable> GetUserMailBox(int ReferralsToUserId, int ReferralsToDepId);
        Task<string?> GetUserPosting(long userId);
        //this actions was moved to IStoredProcedure Repository
        //Task<string> GetDecryptPassword(long id);
        //Task<string> AddByProcedure(SysUserDto obj);
        //Task<string> EditByProcedure(SysUserEditDto obj);
        Task<SysUserVw> Login2(string username, string password, long facilityId);
        Task<long?> CheckPassword(long userId, string? password);
        Task<bool> ChangePassword(long userId, string newPassword, string email);

        Task<bool> UpdateOTP(string otp, long userId);


    }
}
