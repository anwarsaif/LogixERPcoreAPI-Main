using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysZatcaReportingResultsSimulationRepository : GenericRepository<SysZatcaReportingResultsSimulation>, ISysZatcaReportingResultsSimulationRepository
    {
        public SysZatcaReportingResultsSimulationRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}