using Logix.Application.DTOs.Main;
using Logix.Domain.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysWebHookAuthService : IGenericQueryService<SysWebHookAuthDto, SysWebHookAuthVw>, IGenericWriteService<SysWebHookAuthDto, SysWebHookAuthEditDto>
    {
    }
}
