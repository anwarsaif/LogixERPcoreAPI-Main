using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class ZatcaCreditDebitNoteRepository : GenericRepository<ZatcaCreditDebitNote>, IZatcaCreditDebitNoteRepository
    {
        public ZatcaCreditDebitNoteRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}