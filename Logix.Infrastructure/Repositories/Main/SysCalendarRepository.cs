using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysCalendarRepository : GenericRepository<SysCalendar>, ISysCalendarRepository
    {
        public SysCalendarRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
    
}
