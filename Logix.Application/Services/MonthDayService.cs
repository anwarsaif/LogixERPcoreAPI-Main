using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Helpers;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using System.Linq.Expressions;

namespace Logix.Application.Services.Main
{
    public class MonthDayService : IMonthDayService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;
        public MonthDayService(IMainRepositoryManager mainRepositoryManager, IMapper mapper, ICurrentData session, ILocalizationService localization)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.session = session;
            this.localization = localization;
        }

        public Task<IResult<IEnumerable<MonthDayDto>>> GetAll(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<IEnumerable<MonthDayDto>>> GetAll(Expression<Func<MonthDayDto, bool>> expression, CancellationToken cancellationToken = default)
        {
            try
            {
                var mapExpr = _mapper.Map<Expression<Func<MonthDay, bool>>>(expression);
                var items = await _mainRepositoryManager.MonthDayRepository.GetAll(mapExpr);
                var itemMap = _mapper.Map<IEnumerable<MonthDayDto>>(items);
                return await Result<IEnumerable<MonthDayDto>>.SuccessAsync(itemMap);
            }
            catch (Exception exp)
            {
                return await Result<IEnumerable<MonthDayDto>>.FailAsync($"EXP in {this.GetType().Name} , Message: {exp.Message}");
            }
        }

        public Task<IResult<IEnumerable<MonthDayDto>>> GetAll(Expression<Func<MonthDayDto, bool>> expression, int skip, int take, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<IEnumerable<R>>> GetAll<R>(Expression<Func<MonthDayDto, R>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<IEnumerable<R>>> GetAll<R>(Expression<Func<MonthDayDto, R>> selector, Expression<Func<MonthDayDto, bool>> expression, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedResult<IEnumerable<MonthDayDto>>> GetAllWithPagination<R>(Expression<Func<MonthDayDto, R>> selector, Expression<Func<MonthDayDto, bool>> expression, int take, R? lastSeenId = null, List<DateCondition>? dateConditions = null) where R : struct, IComparable
        {
            throw new NotImplementedException();
        }

        public Task<IResult<MonthDayDto>> GetById(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<MonthDayDto>> GetById(long Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<TEditDto>> GetForUpdate<TEditDto>(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<TEditDto>> GetForUpdate<TEditDto>(long Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<MonthDayDto>> GetOne(Expression<Func<MonthDayDto, bool>> expression, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<R>> GetOne<R>(Expression<Func<MonthDayDto, R>> selector, Expression<Func<MonthDayDto, bool>> expression, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }

}
