using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysCustomerContactRepository : IGenericRepository<SysCustomerContact>
    {
        Task<IResult<List<SysCustomerContact>>> RemoveContacts(long CustomerId, CancellationToken cancellationToken = default);
    }

}
