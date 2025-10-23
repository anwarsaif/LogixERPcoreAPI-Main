using System.Data;
using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysUserService : IGenericQueryService<SysUserDto, SysUserVw>, IGenericWriteService<SysUserDto, SysUserEditDto>
    {
        Task<IResult<SysUserLoginTimeDto>> UpdateUserLoginTime(SysUserLoginTimeDto entity, CancellationToken cancellationToken = default);
        Task<IResult<DataTable>> GetUserMailBox(int ReferralsToUserId, int ReferralsToDepId, CancellationToken cancellationToken = default);

        Task<string?> GetUserPosting(long userId);
        Task<string> GetDecryptUserPassword(long userId);

        Task<IResult<long>> GetPackageId();
        Task<IResult> ActivateTwoFactor(string UsersId, int TwoFactorType, CancellationToken cancellationToken = default);
        Task<IResult> DeactivateTwoFactor(string UsersId, CancellationToken cancellationToken = default);
        Task<IResult<string>> CreateUserRequst(CreateUserRequstDto entity, CancellationToken cancellationToken = default);
        Task<IResult<string>> DisableUserByUserName(string UserName, int language = 1, CancellationToken cancellationToken = default);

        Task<long?> CheckPassword(long userId, string? password);
        Task<bool> ChangePassword(long userId, string? newPassword, string email);
        Task<bool> UpdateOTP(string otp, long userId);
        Task<IResult<bool>> ApproveAgreement(CancellationToken cancellationToken = default);
        Task<IResult> UpdateTwoFactor(long userId, CancellationToken cancellationToken = default);
        Task<IResult<string>> AddActiveDirectoryUser(ActiveDirectoryUserAddDto entity, CancellationToken cancellationToken = default);
        Task<IResult<List<SysUserFilterDto>>> Search(SysUserFilterDto filter, CancellationToken cancellationToken = default);
        Task<IResult<List<SysUsersLoginsVm>>> GetUsersLoginsRpt(SysUsersLoginsVm filter, CancellationToken cancellationToken = default);
        Task<IResult<List<SysUsersLoginsVm>>> GetUsersPermissionsRpt(SysUsersLoginsVm filter, CancellationToken cancellationToken = default);
    }
}
