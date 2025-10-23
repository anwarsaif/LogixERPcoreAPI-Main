using System.Data;
using Logix.Application.DTOs.ACC;
using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysZatcaInvoiceTransactionService : IGenericQueryService<SysZatcaInvoiceTransactionDto>, IGenericWriteService<SysZatcaInvoiceTransactionDto, SysZatcaInvoiceTransactionDto>
    {
        Task<IResult<DataTable>> GetTransactions(ZatcaInvoiceFilterDto filter);
    }
}