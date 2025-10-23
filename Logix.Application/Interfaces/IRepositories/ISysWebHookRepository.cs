using Logix.Domain.Main;
using System.Linq.Expressions;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysWebHookRepository : IGenericRepository<SysWebHook>
    {
        Task<IEnumerable<SysWebHookVw>> GetAllFromView(Expression<Func<SysWebHookVw, bool>> expression);
    }
}
