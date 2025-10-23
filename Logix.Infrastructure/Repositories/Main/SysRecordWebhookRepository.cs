using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysRecordWebhookRepository : GenericRepository<SysRecordWebhook>, ISysRecordWebhookRepository
    {
        private readonly ApplicationDbContext context;

        public SysRecordWebhookRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        //public async Task<List<SysRecordWebhookVw>> GetAllFromView(Expression expression)
        //{
        //    var transactions = await context.SysRecordWebhookVws.AsNoTracking()
        //        .Where((Expression<Func<SysRecordWebhookVw, bool>>)expression).ToListAsync();
        //    return transactions;
        //}
        //public async Task<List<SysRecordWebhookVw>> GetAllFromViews(string stringIds)
        //{
        //    var ids = stringIds.Split(',').Select(long.Parse).ToList();

        //    var transactions = await context.SysRecordWebhookVws.AsNoTracking()
        //        .Where(x=>x.IsDeleted == false && ids.Contains(x.Id)).ToListAsync();
        //    return transactions;
        //}
        public async Task<SysRecordWebhookVw> GetAllFromViews(long id)
        {
            var record = await context.SysRecordWebhookVws.AsNoTracking()
                .Where(x=>x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
            return record;
        }
    }
}
