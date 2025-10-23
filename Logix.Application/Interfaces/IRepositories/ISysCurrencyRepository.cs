using Logix.Domain.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysCurrencyRepository : IGenericRepository<SysCurrency>
    {
       
              Task<int> GetDefaultCurrency();
    }
    
}
