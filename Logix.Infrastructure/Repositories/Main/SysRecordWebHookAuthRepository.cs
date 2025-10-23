using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Application.Services.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysRecordWebHookAuthRepository : GenericRepository<SysRecordWebhookAuth>, ISysRecordWebHookAuthRepository
    {
        public SysRecordWebHookAuthRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
