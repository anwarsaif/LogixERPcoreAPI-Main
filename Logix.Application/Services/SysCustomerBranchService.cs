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
    public class SysCustomerBranchService : GenericQueryService<SysCustomerBranch, SysCustomerBranchDto, SysCustomerBranchVw>, ISysCustomerBranchService
    {
        public SysCustomerBranchService(IQueryRepository<SysCustomerBranch> queryRepository, IMapper mapper) : base(queryRepository, mapper)
        {
        }

        public Task<IResult<SysCustomerBranchDto>> Add(SysCustomerBranchDto entity, CancellationToken cancellationToken = default)
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

        public Task<IResult<SysCustomerBranchEditDto>> Update(SysCustomerBranchEditDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
