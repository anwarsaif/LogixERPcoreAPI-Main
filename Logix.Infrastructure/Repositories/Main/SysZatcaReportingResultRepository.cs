using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysZatcaReportingResultRepository : GenericRepository<SysZatcaReportingResult>, ISysZatcaReportingResultRepository
    {
        public SysZatcaReportingResultRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}