using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysTemplateRepository : GenericRepository<SysTemplate>, ISysTemplateRepository
    {
        private readonly ApplicationDbContext context;

        public SysTemplateRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;

        }

    }
}