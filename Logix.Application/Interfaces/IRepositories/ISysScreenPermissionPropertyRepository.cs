
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysScreenPermissionPropertyRepository : IGenericRepository<SysScreenPermissionProperty>
    {
        Task<SysScreenPermissionProperty> GetByProperty(long propertyId, long userId);
        Task<SysScreenPermissionPropertiesVw> GetByPropertyVw(long propertyId, long userId);
        Task<IEnumerable<SysScreenPermissionPropertiesVw>> GetAllVw();
    }
}
