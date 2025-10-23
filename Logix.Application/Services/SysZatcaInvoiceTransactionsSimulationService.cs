using System.Data;
using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Services.Main
{
    public class SysZatcaInvoiceTransactionsSimulationService : GenericQueryService<SysZatcaInvoiceTransactionsSimulation, SysZatcaInvoiceTransactionsSimulationDto>, ISysZatcaInvoiceTransactionsSimulationService
    {
        private readonly IMainRepositoryManager mainRepositoryManager;

        public SysZatcaInvoiceTransactionsSimulationService(IQueryRepository<SysZatcaInvoiceTransactionsSimulation> queryRepository,
            IMapper mapper,
            IMainRepositoryManager mainRepositoryManager) : base(queryRepository, mapper)
        {
            this.mainRepositoryManager = mainRepositoryManager;
        }

        public Task<IResult<SysZatcaInvoiceTransactionsSimulationDto>> Add(SysZatcaInvoiceTransactionsSimulationDto entity, CancellationToken cancellationToken = default)
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

        public Task<IResult<SysZatcaInvoiceTransactionsSimulationDto>> Update(SysZatcaInvoiceTransactionsSimulationDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<DataTable>> GetTransactions_Simulation(ZatcaInvoiceFilterDto filter)
        {
            try
            {
                var items = await mainRepositoryManager.SysZatcaInvoiceTransactionsSimulationRepository.GetTransactions_Simulation(filter);
                return await Result<DataTable>.SuccessAsync(items);
            }
            catch (Exception ex)
            {
                return await Result<DataTable>.FailAsync(ex.Message);
            }
        }
    }
}
