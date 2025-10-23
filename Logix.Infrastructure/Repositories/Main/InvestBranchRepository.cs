using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Logix.Infrastructure.Repositories.Main
{
    public class InvestBranchRepository : GenericRepository<InvestBranch>, IInvestBranchRepository
    {
        private readonly ApplicationDbContext context;

        public InvestBranchRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<InvestBranchVw>> GetAllVW()
        {
            return await context.InvestBranceshVws.ToListAsync();
        }
    }
}
