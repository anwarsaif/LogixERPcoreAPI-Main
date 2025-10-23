using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysPropertyRepository : IGenericRepository<SysProperty, SysPropertiesVw>
    {
        Task<SysPropertiesVw> GetByIdVw(long propertyId);
        Task<IEnumerable<SysPropertiesVw>> GetAllVw();
    }
}
