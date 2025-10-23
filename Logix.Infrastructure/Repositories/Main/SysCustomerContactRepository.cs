using Logix.Application.Common;
using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysCustomerContactRepository : GenericRepository<SysCustomerContact>, ISysCustomerContactRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ICurrentData session;
        public SysCustomerContactRepository(ApplicationDbContext context, ICurrentData session) : base(context)
        {
            this.context = context;
            this.session = session;
        }

        public async Task<IResult<List<SysCustomerContact>>> RemoveContacts(long customerId, CancellationToken cancellationToken = default)
        {
            try
            {
                var contacts = context.SysCustomerContacts.Where(x => x.CusId == customerId && x.IsDeleted == false).ToList();
                
                foreach (var contact in contacts)
                {
                    if (contact.Id > 0)
                    {
                        contact.ModifiedBy = session.UserId;
                        contact.ModifiedOn = DateTime.Now;
                        contact.IsDeleted = true;
                    }
                }

                context.SysCustomerContacts.UpdateRange(contacts);
                await context.SaveChangesAsync(cancellationToken);

                return await Result<List<SysCustomerContact>>.SuccessAsync(contacts, "Items deleted successfully");
            }
            catch (Exception ex)
            {
                return await Result<List<SysCustomerContact>>.FailAsync(ex.Message);
            }
        }

    }

}
