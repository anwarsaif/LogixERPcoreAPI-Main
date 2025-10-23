using Logix.Application.DTOs.Main;
using Logix.Domain.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysCustomerTypeService : IGenericQueryService<SysCustomerTypeDto, SysCustomerType>, IGenericWriteService<SysCustomerTypeDto, SysCustomerTypeDto>
    {
    }

}
