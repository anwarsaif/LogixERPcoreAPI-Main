using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysCustomerFileRepository : IGenericRepository<SysCustomerFile>
    {
        Task<IResult<List<SysCustomerFile>>> RemoveFiles(long CustomerId, CancellationToken cancellationToken = default);
    }

}
