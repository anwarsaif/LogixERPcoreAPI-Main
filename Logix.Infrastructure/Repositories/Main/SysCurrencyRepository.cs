using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysCurrencyRepository : GenericRepository<SysCurrency>, ISysCurrencyRepository
    {
        private readonly ApplicationDbContext context;

        public SysCurrencyRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<int> GetDefaultCurrency()
        {
            try
            {
              return (int)await context.SysExchangeRates.Select(x => x.CurrencyToID).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                //  في حال حدوث أي مشاكل نرجع له العمله رقم 1
                return 1;
            }
        }
    }    
}
