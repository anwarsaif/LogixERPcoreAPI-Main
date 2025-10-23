using Logix.Domain.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysExchangeRateRepository:IGenericRepository<SysExchangeRate>
    {
        Task<IEnumerable<SysExchangeRateListVW>> GetAllVW();
        Task<decimal> GetExchangeRateValue(long? CurrencyFromID);
    }
}
