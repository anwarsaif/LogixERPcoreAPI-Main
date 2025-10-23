using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysRecordWebhookRepository : IGenericRepository<SysRecordWebhook>
    {
        Task<SysRecordWebhookVw> GetAllFromViews(long Id);
    }
}
