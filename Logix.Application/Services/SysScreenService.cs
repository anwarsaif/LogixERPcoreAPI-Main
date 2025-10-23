using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using System.Linq.Expressions;

namespace Logix.Application.Services.Main
{
    public class SysScreenService : GenericQueryService<SysScreen, SysScreenDto, SysScreenVw>, ISysScreenService
    {
        public SysScreenService(IQueryRepository<SysScreen> queryRepository, IMapper mapper) : base(queryRepository, mapper)
        {
        }

        public Task<IResult<SysScreenDto>> Add(SysScreenDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<SysScreenDto>> Update(SysScreenDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
