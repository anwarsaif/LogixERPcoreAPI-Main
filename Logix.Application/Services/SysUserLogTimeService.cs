using AutoMapper;
using Logix.Application.Common;
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
    public class SysUserLogTimeService : GenericQueryService<SysUserLogTime, SysUserLogTimeDto, SysUserLogTimeVw>, ISysUserLogTimeService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;

        public SysUserLogTimeService(IQueryRepository<SysUserLogTime> queryRepository,
            IMapper mapper,
            IMainRepositoryManager mainRepositoryManager) : base(queryRepository, mapper)
        {
            this._mapper = mapper;
            this._mainRepositoryManager = mainRepositoryManager;
        }

        public Task<IResult<SysUserLogTimeDto>> Add(SysUserLogTimeDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<IEnumerable<SysUserTypeDto>>> GetAllUserTypes()
        {
            var userTypes = await _mainRepositoryManager.SysUserTypeRepository.GetAll();
            return await Result<IEnumerable<SysUserTypeDto>>.SuccessAsync(_mapper.Map<IEnumerable<SysUserTypeDto>>(userTypes));
        }

        public Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<SysUserLogTimeDto>> Update(SysUserLogTimeDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


    }
}
