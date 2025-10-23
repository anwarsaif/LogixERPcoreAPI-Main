using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.DTOs.PUR;
using Logix.Application.Helpers;
using Logix.Application.Interfaces.IRepositories.Main;
using Logix.Domain.Main;
using Logix.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Polly;
using System.Globalization;
using System.Linq.Expressions;

namespace Logix.Infrastructure.Repositories.Main
{
    public class SysCustomerRepository : GenericRepository<SysCustomer>, ISysCustomerRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ICurrentData session;

        public SysCustomerRepository(ApplicationDbContext context,
            ICurrentData session) : base(context)
        {
            this.context = context;
            this.session = session;
        }

        public async Task<bool> AnyAsync(Expression<Func<SysCustomer, bool>> predicate)
        {
            return await context.SysCustomer.AnyAsync(predicate);
        }

        public async Task<long> GetCurrencyCustomer(long CusTypeId, string code, long facilityId)
        {
            try
            {
                if (CusTypeId > 0)
                {
                    return await context.SysCustomer
                        .Where(x => x.IsDeleted == false && x.FacilityId == facilityId && x.CusTypeId == CusTypeId && x.Code.Equals(code))
                        .Select(x => x.CurrencyId)
                        .SingleOrDefaultAsync() ?? 0;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while Currency AccountCode.", ex);
            }
        }

        public async Task<SysCustomerMemberIdCodeDto> GetCustomerMemberIdCodeAsync(
        int cusTypeId, long facilityId, int branchId, string Code, string MemberId)
        {
            var result = new SysCustomerMemberIdCodeDto();
            var numberingByBranch = await context.SysPropertyValues.Where(p => p.PropertyId == 221 && p.FacilityId == session.FacilityId).Select(p => p.PropertyValue).FirstOrDefaultAsync();
            var branchCode = "";
            if (string.IsNullOrEmpty(Code))
            {
                if (numberingByBranch == "1")
                {
                    var maxNoQuery = await context.SysCustomer
                        .Where(t => t.BranchId == branchId &&
                                    t.FacilityId == facilityId &&
                                    t.CusTypeId == cusTypeId).CountAsync();

                    result.Code = (maxNoQuery + 1).ToString();

                    branchCode = await context.InvestBranches
                       .Where(b => b.BranchId == branchId)
                       .Select(b => b.BranchCode)
                       .FirstOrDefaultAsync();

                    result.Code = $"{branchCode}{result.Code.PadLeft(7, '0')}";
                }
                else
                {
                    var maxCodeQuery = await context.SysCustomer
                        .Where(t =>
                            t.FacilityId == facilityId &&
                            t.CusTypeId == facilityId)
                        .MaxAsync(t => EF.Functions.IsNumeric(t.Code) ? Convert.ToInt64(t.Code) : 0);
                    result.Code = (maxCodeQuery + 1).ToString();
                }
            }
            if (string.IsNullOrEmpty(MemberId))
            {
                branchCode = await context.SysBranchVws
                   .Where(b => b.BranchId == branchId)
                   .Select(b => b.BranchCode ?? "")
                   .FirstOrDefaultAsync();

                result.MemberId = $"{branchCode}{result.Code}";
            }
            return result;
        }



        public async Task<long> GetCustomerId(string Customercode, int CustomerTypeId)
        {
            try
            {
                return await context.SysCustomer.Where(x => x.IsDeleted == false && x.FacilityId == session.FacilityId && x.CusTypeId == CustomerTypeId && x.Code == Customercode).Select(x => x.Id).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while GetCustomerId.", ex);
            }
        }
        public async Task<long> GetCustomerAccountId(long FacilityId ,string Customercode, int CustomerTypeId)
        {
            try
            {
                return await context.SysCustomer.Where(x => x.IsDeleted == false && x.FacilityId == FacilityId && x.CusTypeId == CustomerTypeId && x.Code == Customercode).Select(x => x.AccAccountId).SingleOrDefaultAsync()??0;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while GetCustomerId.", ex);
            }
        }
    }
}
