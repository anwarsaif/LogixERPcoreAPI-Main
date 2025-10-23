using Logix.Application.Wrapper;
using Logix.Domain.Main;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface IInvestEmployeeRepository : IGenericRepository<InvestEmployee>
    {
        Task<Result<SelectList>> DDLFieldColumns();
    }
}
