using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysWebHookAuthRepository : GenericRepository<SysWebHookAuth>, ISysWebHookAuthRepository
    {
        public SysWebHookAuthRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
