using System.Data;
using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Services.Main
{
    public class SysZatcaInvoiceTransactionService : GenericQueryService<SysZatcaInvoiceTransaction, SysZatcaInvoiceTransactionDto>, ISysZatcaInvoiceTransactionService
    {
        private readonly IMainRepositoryManager mainRepositoryManager;

        public SysZatcaInvoiceTransactionService(IQueryRepository<SysZatcaInvoiceTransaction> queryRepository,
            IMapper mapper,
            IMainRepositoryManager mainRepositoryManager) : base(queryRepository, mapper)
        {
            this.mainRepositoryManager = mainRepositoryManager;
        }

        public Task<IResult<SysZatcaInvoiceTransactionDto>> Add(SysZatcaInvoiceTransactionDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<SysZatcaInvoiceTransactionDto>> Update(SysZatcaInvoiceTransactionDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<DataTable>> GetTransactions(ZatcaInvoiceFilterDto filter)
        {
            try
            {
                var items = await mainRepositoryManager.SysZatcaInvoiceTransactionRepository.GetTransactions(filter);
                return await Result<DataTable>.SuccessAsync(items);
            }
            catch (Exception ex)
            {
                return await Result<DataTable>.FailAsync(ex.Message);
            }
        }
    }
}
