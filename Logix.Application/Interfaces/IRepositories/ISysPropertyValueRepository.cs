
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysPropertyValueRepository : IGenericRepository<SysPropertyValue, SysPropertyValuesVw>
    {
        Task<SysPropertyValue> GetByProperty(long propertyId, long facilityId);
        Task<SysPropertyValuesVw> GetByPropertyVw(long propertyId, long facilityId);
        Task<IEnumerable<SysPropertyValuesVw>> GetAllVw();
    }
}
