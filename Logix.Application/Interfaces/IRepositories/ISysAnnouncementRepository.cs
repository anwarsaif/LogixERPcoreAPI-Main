using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysAnnouncementRepository : IGenericRepository<SysAnnouncement>
    {
        Task<IEnumerable<SysAnnouncementLocationVw>> GeSysAnnouncementLocationVw();
    }
}
