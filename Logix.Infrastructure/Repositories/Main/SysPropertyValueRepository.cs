


using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysPropertyValueRepository : GenericRepository<SysPropertyValue, SysPropertyValuesVw>, ISysPropertyValueRepository
    {
        private readonly ApplicationDbContext context;

        public SysPropertyValueRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<SysPropertyValuesVw>> GetAllVw()
        {
            return await context.SysPropertyValuesVws.ToListAsync();
        }

        public async Task<SysPropertyValue> GetByProperty(long propertyId, long facilityId)
        {
            return await context.SysPropertyValues.Where(d => d.PropertyId == propertyId && d.FacilityId == facilityId).FirstOrDefaultAsync();
        }
        
        public async Task<SysPropertyValuesVw> GetByPropertyVw(long propertyId, long facilityId)
        {
            return await context.SysPropertyValuesVws.Where(d => d.PropertyId == propertyId && d.FacilityId == facilityId).FirstOrDefaultAsync();
        }
    }
    
    public class SysScreenPermissionPropertyRepository : GenericRepository<SysScreenPermissionProperty>, ISysScreenPermissionPropertyRepository
    {
        private readonly ApplicationDbContext context;

        public SysScreenPermissionPropertyRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<SysScreenPermissionPropertiesVw>> GetAllVw()
        {
            return await context.SysScreenPermissionPropertiesVws.ToListAsync();
        }

        public async Task<SysScreenPermissionProperty> GetByProperty(long propertyId, long userId)
        {
            return await context.SysScreenPermissionProperties.Where(d => d.PropertyId == propertyId && d.UserId == userId).FirstOrDefaultAsync();
        }
        
        public async Task<SysScreenPermissionPropertiesVw> GetByPropertyVw(long propertyId, long userId)
        {
            return await context.SysScreenPermissionPropertiesVws.Where(d => d.PropertyId == propertyId && d.UserId == userId).FirstOrDefaultAsync();
        }
    }
}
