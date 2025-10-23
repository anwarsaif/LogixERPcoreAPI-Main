using Logix.Application.DTOs.Main;
using Logix.Domain.Main;
using System.Linq.Expressions;

namespace Logix.Application.Interfaces.IRepositories.Main
{
    public interface ISysCustomerRepository : IGenericRepository<SysCustomer>
    {
        //Task<IEnumerable<SysAnnouncementVw>> GetAllVW();
        //Task<IEnumerable<SysAnnouncementLocationVw>> GeSysAnnouncementLocationVw();
        Task<long> GetCurrencyCustomer(long CusTypeId, string code, long facilityId);
        Task<bool> AnyAsync(Expression<Func<SysCustomer, bool>> predicate);
        Task<SysCustomerMemberIdCodeDto> GetCustomerMemberIdCodeAsync(
        int cusTypeId, long facilityId, int branchId, string Code, string MemberId);
        Task<long> GetCustomerId(string Customercode, int CustomerTypeId);
        Task<long> GetCustomerAccountId(long FacilityId, string Customercode, int CustomerTypeId);
    }
}
