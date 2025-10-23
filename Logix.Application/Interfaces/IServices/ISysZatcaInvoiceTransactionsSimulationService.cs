using System.Data;
using Logix.Application.DTOs.ACC;
using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysZatcaInvoiceTransactionsSimulationService : IGenericQueryService<SysZatcaInvoiceTransactionsSimulationDto>, IGenericWriteService<SysZatcaInvoiceTransactionsSimulationDto, SysZatcaInvoiceTransactionsSimulationDto>
    {
        Task<IResult<DataTable>> GetTransactions_Simulation(ZatcaInvoiceFilterDto filter);
    }
}