using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysPeriodRepository : GenericRepository<SysPeriod>, ISysPeriodRepository
    {
        private readonly ApplicationDbContext context;

        public SysPeriodRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<long> GetSysPeriodIdByDate(string Date, long FacilityId, int SystemId)
        {
            var periods = await context.Set<SysPeriod>().Where(p => p.IsActive == true && p.IsDeleted == false && p.FacilityId == FacilityId && p.SystemId == SystemId).AsNoTracking().ToListAsync();
            var periodRes = periods.Where(p => !string.IsNullOrEmpty(p.StartDate) && !string.IsNullOrEmpty(p.EndDate) &&
                DateTime.ParseExact(Date, "yyyy/MM/dd", CultureInfo.InvariantCulture) >= DateTime.ParseExact(p.StartDate, "yyyy/MM/dd", CultureInfo.InvariantCulture)
                && DateTime.ParseExact(Date, "yyyy/MM/dd", CultureInfo.InvariantCulture) <= DateTime.ParseExact(p.EndDate, "yyyy/MM/dd", CultureInfo.InvariantCulture)
             ).FirstOrDefault();
            if (periodRes != null)
            {
                return periodRes.Id;
            }
            else
            {
                return 0;
            }

        }
    }
}
