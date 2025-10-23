using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.HR;
using Logix.Infrastructure.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Infrastructure.Repositories.Main
{
    public class InvestMonthRepository : GenericRepository<InvestMonth>, IInvestMonthRepository
    {
        public InvestMonthRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
