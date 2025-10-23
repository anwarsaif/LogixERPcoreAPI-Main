using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysNotificationsMangRepository : IGenericRepository<SysNotificationsMang>
    {
        Task<List<SysNotificationsMangResultDto>> GetNotificationsByUserAndGroupAsync();

    }
}
