using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Helpers;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.HR;
using Logix.Domain.Main;
using System.Linq.Expressions;

namespace Logix.Application.Services.Main
{
    public class SysTableService : GenericQueryService<SysTable, SysTableDto, SysTableDto>, ISysTableService
    {

        private readonly IMapper _mapper;


        public SysTableService(IQueryRepository<SysTable> queryRepository, IMapper mapper) : base(queryRepository, mapper)
        {
            this._mapper = mapper;
        }

        public Task<IResult<IEnumerable<SysTable>>> GetAllVW(Expression<Func<SysTable, bool>> expression, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedResult<IEnumerable<SysTable>>> GetAllWithPaginationVW<R>(Expression<Func<SysTable, R>> selector, Expression<Func<SysTable, bool>> expression, int take, R? lastSeenId = null, List<DateCondition>? dateConditions = null) where R : struct, IComparable
        {
            throw new NotImplementedException();
        }

        public Task<IResult<SysTable>> GetOneVW(Expression<Func<SysTable, bool>> expression, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        Task<IResult<IEnumerable<SysTable>>> IGenericQueryService<SysTableDto, SysTable>.GetAllVW(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
