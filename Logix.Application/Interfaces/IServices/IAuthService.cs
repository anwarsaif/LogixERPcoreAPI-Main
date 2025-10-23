using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface IAuthService
    {
        Task<IResult<SysUserVw>> Login(LoginDto login);
        Task<IResult<SysUser>> GetByName(string username);
        Task<IResult<SysUser>> GetById(long id);
        Task<IResult<IEnumerable<SysUserVw>>> GetUsersByEmpId(int empId, long userId);
        string GetJWTToken(SysUserVw user, string secret, long finYear, long periodId, int lang, int currFinYearGregorian, string CalendarType,int IsAzureAuthenticated=0);
        // Task<IResult<UserViewDto>> CreateUser(CreateUserDto user);
        Task CreateUser(string UserName, string empCode, string empName, long facilityId, int branchId, string email, string password, string groupId, CancellationToken cancellationToken = default);
        Task<IResult<SysUserVw>> LoginByUserAD(string UserName, long facilityId, CancellationToken cancellationToken = default);

        Task<IResult<SysUserVw>> Login2(LoginDto login, CancellationToken cancellationToken = default);
        Task<IResult<bool>> OTP(SysUserVw entity, CancellationToken cancellationToken = default);

        Task<IResult> UpdateUSERLogTime(long UserId, string? IpAddress = "", CancellationToken cancellationToken = default);

        Task<IResult> InsertTask(long userId, long facilityId, CancellationToken cancellationToken = default);
        Task<IResult<SysUserVw>> loginWithAuthenticationCode(LoginWithAuthenticationCodeDto login, CancellationToken cancellationToken = default);
        Task<IResult<string>> ResendCode(LoginResendCodeDto entity, CancellationToken cancellationToken = default);
        Task<IResult<SysUserVw>> LoginByAzure(string email, long facilityId, string AzureIDToken = "", CancellationToken cancellationToken = default);
        Task<IResult<bool>> CreateEmpAzureAsync(string userEmail, string empName, long facilityId, string AzureIDToken="");
        Task<IResult<string>> Logout(CancellationToken cancellationToken = default);

    }
}
