using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface IInvestBranchRepository : IGenericRepository<InvestBranch>
    {
        Task<IEnumerable<InvestBranchVw>> GetAllVW();
    }

}
