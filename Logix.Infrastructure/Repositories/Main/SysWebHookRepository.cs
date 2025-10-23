using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Polly;
using System.Linq.Expressions;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysWebHookRepository : GenericRepository<SysWebHook>, ISysWebHookRepository
    {
        private readonly ApplicationDbContext context;

        public SysWebHookRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<SysWebHookVw>> GetAllFromView(Expression<Func<SysWebHookVw, bool>> expression)
        {
            return await context.Set<SysWebHookVw>().Where(expression).AsNoTracking().ToListAsync();
        }
    }    
}
