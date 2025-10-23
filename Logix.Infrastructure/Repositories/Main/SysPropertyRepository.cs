using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysPropertyRepository : GenericRepository<SysProperty, SysPropertiesVw>, ISysPropertyRepository
    {
        private readonly ApplicationDbContext context;

        public SysPropertyRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<SysPropertiesVw>> GetAllVw()
        {
            return await context.SysPropertiesVws.ToListAsync();
        }

        public async Task<SysPropertiesVw> GetByIdVw(long propertyId)
        {
            return await context.SysPropertiesVws.Where(d => d.Id == propertyId).SingleOrDefaultAsync();
        }
    }
}
