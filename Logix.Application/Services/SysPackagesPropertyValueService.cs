using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Application.Helpers;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using System.Linq.Expressions;

namespace Logix.Application.Services.Main
{
    public class SysPackagesPropertyValueService : GenericQueryService<SysPackagesPropertyValue, SysPackagesPropertyValueDto>, ISysPackagesPropertyValueService
    {
        public SysPackagesPropertyValueService(IQueryRepository<SysPackagesPropertyValue> queryRepository, IMapper mapper) : base(queryRepository, mapper)
        {
        }

        public Task<IResult<IEnumerable<SysPackagesPropertyValue>>> GetAllVW(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<IEnumerable<SysPackagesPropertyValue>>> GetAllVW(Expression<Func<SysPackagesPropertyValue, bool>> expression, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedResult<IEnumerable<TResult>>> GetAllWithCursorPaginationAsync<TType, R, TResult>(Expression<Func<TType, R>> selector, Expression<Func<TType, bool>> expression, int take, R? lastSeenId = null, List<DateCondition>? dateConditions = null, Func<IQueryable<TType>, IQueryable<TResult>>? groupedQuery = null)
            where TType : class
            where R : struct, IComparable
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedResult<IEnumerable<SysPackagesPropertyValue>>> GetAllWithPaginationVW<R>(Expression<Func<SysPackagesPropertyValue, R>> selector, Expression<Func<SysPackagesPropertyValue, bool>> expression, int take, R? lastSeenId = null, List<DateCondition>? dateConditions = null) where R : struct, IComparable
        {
            throw new NotImplementedException();
        }

        public Task<IResult<SysPackagesPropertyValue>> GetOneVW(Expression<Func<SysPackagesPropertyValue, bool>> expression, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
