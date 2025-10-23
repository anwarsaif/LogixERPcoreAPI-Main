using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysAnnouncementRepository : GenericRepository<SysAnnouncement>, ISysAnnouncementRepository
    {
        private readonly ApplicationDbContext context;

        public SysAnnouncementRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<SysAnnouncementLocationVw>> GeSysAnnouncementLocationVw()
        {
            return await context.SysAnnouncementLocationVws.ToListAsync();
        }
    }
}
