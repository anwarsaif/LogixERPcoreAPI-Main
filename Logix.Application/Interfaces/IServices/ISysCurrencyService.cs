using Logix.Application.DTOs.Main;
using Logix.Domain.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysCurrencyService : IGenericQueryService<SysCurrencyDto, SysCurrencyListVw>, IGenericWriteService<SysCurrencyDto, SysCurrencyEditDto>
    {

    }
}
