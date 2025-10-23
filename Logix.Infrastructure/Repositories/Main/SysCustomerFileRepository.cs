using Logix.Application.Common;
using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysCustomerFileRepository : GenericRepository<SysCustomerFile>, ISysCustomerFileRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ICurrentData session;

        public SysCustomerFileRepository(ApplicationDbContext context,ICurrentData session) : base(context)
        {
            this.context = context;
            this.session = session;
        }
        public async Task<IResult<List<SysCustomerFile>>> RemoveFiles(long CustomerId, CancellationToken cancellationToken = default)
        {
            try
            {
                List<SysCustomerFile> savedFiles = new();
                var files = context.SysCustomerFiles.Where(x => x.CustomerId == CustomerId && x.IsDeleted == false).ToList();
                foreach (var item in files)
                {
                    if (item.Id > 0)
                    {
                        item.ModifiedBy = session.UserId;
                        item.ModifiedOn = DateTime.Now;
                        item.IsDeleted = true;
                        context.SysCustomerFiles.Update(item);
                    }
                }
                return await Result<List<SysCustomerFile>>.SuccessAsync(savedFiles, "items Deleted successfully");
            }
            catch (Exception ex)
            {
                return await Result<List<SysCustomerFile>>.FailAsync(ex.Message);
            }
        }

    }

}
