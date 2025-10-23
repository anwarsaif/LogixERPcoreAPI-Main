using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysCountryRepository : GenericRepository<SysCountry>, ISysCountryRepository
    {
        public SysCountryRepository(ApplicationDbContext context) : base(context)
        {

        }
    }

}
