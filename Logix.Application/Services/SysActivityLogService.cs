using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Application.Services.Main
{
    public class SysActivityLogService : GenericQueryService<SysActivityLog, SysActivityLogDto, SysActivityLogVw>, ISysActivityLogService
    {
        public SysActivityLogService(IQueryRepository<SysActivityLog> queryRepository,
            IMapper mapper) : base(queryRepository, mapper)
        {
        }

        public Task<IResult<SysActivityLogDto>> Add(SysActivityLogDto entity, CancellationToken cancellationToken = default)
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

        public Task<IResult<SysActivityLogDto>> Update(SysActivityLogDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
