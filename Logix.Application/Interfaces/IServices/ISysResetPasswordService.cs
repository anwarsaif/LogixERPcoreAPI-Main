using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysResetPasswordService : IGenericQueryService<SysResetPasswordDto, SysResetPassword>, IGenericWriteService<SysResetPasswordDto, SysResetPasswordEditDto>
    {
        Task<IResult<ResetPasswordResultDto>> SendVerificationCodeByEmail(ResetPasswordByEmail entity, CancellationToken cancellationToken = default);
        Task<IResult<ResetPasswordResultDto>> SendVerificationCodeByMobile(ResetPasswordByMobile entity, CancellationToken cancellationToken = default);
        Task<IResult<ResetPasswordResultDto>> ValidateVerificationCode(string verificationCode, int verificationType, string userName, int Language, CancellationToken cancellationToken = default);
        Task<IResult<ResetPasswordResultDto>> ResetPassword(string verificationCode, string Password, string userName, int Language, CancellationToken cancellationToken = default);

    }
}
