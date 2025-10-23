using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysLicenseRepository : GenericRepository<SysLicense>, ISysLicenseRepository
    {
        private readonly ApplicationDbContext context;

        public SysLicenseRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<SysLicensesVw>> GetAllVW()
        {
            return await context.SysLicensesVws.ToListAsync();
        }
    }
}
