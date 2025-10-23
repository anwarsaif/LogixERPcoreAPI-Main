using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysCustomerGroupAccountRepository : GenericRepository<SysCustomerGroupAccount>, ISysCustomerGroupAccountRepository
    {
        private readonly ApplicationDbContext context;
        public SysCustomerGroupAccountRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<SysCustomerGroupAccountsVw>> GetAllVw()
        {
            return await context.SysCustomerGroupAccountsVws.ToListAsync();
        }
    }
}
