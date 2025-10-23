using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysPackagesPropertyValueRepository : IGenericRepository<SysPackagesPropertyValue>
    {
        Task<bool> Property_exists(long Facility_ID, long Property_ID);
        Task<long> Get_Property_Values(long Facility_ID, long Property_ID);
    }
}
