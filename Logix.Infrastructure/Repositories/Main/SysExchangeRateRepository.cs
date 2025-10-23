using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysExchangeRateRepository : GenericRepository<SysExchangeRate>, ISysExchangeRateRepository
    {
        private readonly ApplicationDbContext context;

        public SysExchangeRateRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<SysExchangeRateListVW>> GetAllVW()
        {
            return await context.SysExchangeRateListsVws.ToListAsync();
        }

        public async Task<decimal> GetExchangeRateValue(long? currencyFromId)
        {
            var data = await context.SysExchangeRates
                .Where(a => a.CurrencyFromID == currencyFromId && a.IsDeleted == false)
                .OrderBy(s => s.ExchangeDate)
                .FirstOrDefaultAsync();

            if (data != null)
            {
                return data.ExchangeRate ?? 0;
            }

            return 0;
        }
    }
}
