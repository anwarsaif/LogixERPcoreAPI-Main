using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Services.Main
{
    public class SysNotificationsSettingService : GenericQueryService<SysNotificationsSetting, SysNotificationsSettingDto, SysNotificationsSettingVw>, ISysNotificationsSettingService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentData session;
        public SysNotificationsSettingService(IQueryRepository<SysNotificationsSetting> queryRepository, IMainRepositoryManager mainRepositoryManager, IMapper mapper, ICurrentData session) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;
            this.session = session;
        }

        public Task<IResult<SysNotificationsSettingDto>> Add(SysNotificationsSettingDto entity, CancellationToken cancellationToken = default)
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

        public Task<IResult<SysNotificationsSettingEditDto>> Update(SysNotificationsSettingEditDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
    }
