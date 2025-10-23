using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class MonthDayRepository : GenericRepository<MonthDay>, IMonthDayRepository
    {
        public MonthDayRepository(ApplicationDbContext context) : base(context)
        {

        }
    }

}
