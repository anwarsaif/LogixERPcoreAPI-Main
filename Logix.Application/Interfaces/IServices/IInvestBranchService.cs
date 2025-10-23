using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    /*    public interface IInvestBranchService : IGenericService<InvestBranchDto>
        {
            Task<IResult<IEnumerable<InvestBranchVw>>> GetAllVW(CancellationToken cancellationToken = default);
        }*/

    public interface IInvestBranchService : IGenericQueryService<InvestBranchDto, InvestBranchVw>, IGenericWriteService<InvestBranchDto, InvestBranchDto>
    {
    }
}
