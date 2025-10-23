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
    public class SysSmsService : GenericQueryService<SysSms, SysSmsDto>, ISysSmsService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        private readonly ILocalizationService localization;

        public SysSmsService(IQueryRepository<SysSms> queryRepository,
            IMapper mapper,
            IMainRepositoryManager mainRepositoryManager,
            ICurrentData session,
            ILocalizationService localization) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.session = session;
            this.localization = localization;
        }

        public async Task<IResult<SysSmsDto>> Add(SysSmsDto entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) return await Result<SysSmsDto>.FailAsync($"{localization.GetMessagesResource("AddNullEntity")}");
            try
            {
                int newId = await _mainRepositoryManager.SysSmsRepository.AddByProcedure(entity);
                await _mainRepositoryManager.UnitOfWork.CompleteAsync(cancellationToken);

                var newEntity = await _mainRepositoryManager.SysSmsRepository.GetOne(s => s.Id == newId);

                return await Result<SysSmsDto>.SuccessAsync(_mapper.Map<SysSmsDto>(newEntity), "item added successfully");
            }
            catch (Exception exc)
            {
                return await Result<SysSmsDto>.FailAsync($"EXP in {this.GetType()}, Meesage: {exc.Message}");
            }
        }

        public Task<IResult> Remove(long Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Remove(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<SysSmsDto>> Update(SysSmsDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
