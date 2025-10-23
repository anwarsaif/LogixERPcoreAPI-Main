using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysLookupDataRepository : GenericRepository<SysLookupData>, ISysLookupDataRepository
    {
        private readonly ApplicationDbContext context;

        public SysLookupDataRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<SysLookupData>> GetDataByCategory(int categoryId)
        {
            return await context.SysLookupData.Where(d => d.CatagoriesId == categoryId).ToListAsync();
        }
    }
}
