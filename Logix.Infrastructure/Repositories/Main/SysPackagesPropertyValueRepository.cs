using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysPackagesPropertyValueRepository : GenericRepository<SysPackagesPropertyValue>, ISysPackagesPropertyValueRepository
    {
        private readonly ApplicationDbContext context;

        public SysPackagesPropertyValueRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<bool> Property_exists(long Facility_ID, long Property_ID)
        {
            try
            {
                var properties = await context.Set<SysPackagesPropertyValue>().Where(p => p.IsDeleted == false && p.PropertyId == Property_ID).AsNoTracking().ToListAsync();
                if (properties.Count > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<long> Get_Property_Values(long Facility_ID, long Property_ID)
        {
            try
            {
                var properties = await context.Set<SysPackagesPropertyValue>().Where(p => p.IsDeleted == false && p.PropertyId == Property_ID).Select(x=>x.PropertyValue).ToListAsync();
                if (properties.Count > 0)
                    return Convert.ToInt64(properties.FirstOrDefault());
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}
