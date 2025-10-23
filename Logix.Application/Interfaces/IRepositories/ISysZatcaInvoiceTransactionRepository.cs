using System.Data;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysZatcaInvoiceTransactionRepository : IGenericRepository<SysZatcaInvoiceTransaction>
    {
        Task<DataTable> GetTransactions(ZatcaInvoiceFilterDto filter);
    }
}
