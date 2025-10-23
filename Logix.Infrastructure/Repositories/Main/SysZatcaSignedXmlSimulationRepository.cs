using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysZatcaSignedXmlSimulationRepository : GenericRepository<SysZatcaSignedXmlSimulation>, ISysZatcaSignedXmlSimulationRepository
    {
        public SysZatcaSignedXmlSimulationRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}