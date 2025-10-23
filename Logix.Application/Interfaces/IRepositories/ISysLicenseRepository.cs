using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysLicenseRepository : IGenericRepository<SysLicense>
    {
        Task<IEnumerable<SysLicensesVw>> GetAllVW();
    }
}