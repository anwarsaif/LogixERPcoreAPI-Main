using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysLicenseService : IGenericQueryService<SysLicenseDto, SysLicensesVw>, IGenericWriteService<SysLicenseDto, SysLicenseDto>
    {

    }
    //public interface ISysLicenseService : IGenericService<SysLicenseDto>
    //{
    //    Task<IResult<IEnumerable<SysLicensesVw>>> GetAllVW(CancellationToken cancellationToken = default);
    //}
}
