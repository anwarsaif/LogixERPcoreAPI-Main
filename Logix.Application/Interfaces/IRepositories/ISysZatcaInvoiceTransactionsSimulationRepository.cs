using System.Data;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysZatcaInvoiceTransactionsSimulationRepository : IGenericRepository<SysZatcaInvoiceTransactionsSimulation>
    {
        Task<DataTable> GetTransactions_Simulation(ZatcaInvoiceFilterDto filter);
    }
}
