using AutoMapper;
using Logix.Application.Common;
using Logix.Application.DTOs.Main;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices.Main;
using Logix.Application.Interfaces.IServices.WA;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Services.Main
{
    public class ChatMessageService : GenericQueryService<ChatMessage, ChatMessageDto, ChatMessage>, IChatMessageService
    {
        private readonly IMainRepositoryManager _mainRepositoryManager;

        private readonly IMapper _mapper;
        private readonly ICurrentData _session;
        private readonly IWhatsappBusinessService waService;
        private readonly ILocalizationService localization;

        public ChatMessageService(IQueryRepository<ChatMessage> queryRepository, IMainRepositoryManager mainRepositoryManager, IMapper mapper, ICurrentData session, IWhatsappBusinessService waService, ILocalizationService localization) : base(queryRepository, mapper)
        {
            this._mainRepositoryManager = mainRepositoryManager;
            this._mapper = mapper;

            this._session = session;
            this.waService = waService;

            this.localization = localization;
        }

        public Task<IResult<ChatMessageDto>> Add(ChatMessageDto entity, CancellationToken cancellationToken = default)
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

        public Task<IResult<ChatMessageEditDto>> Update(ChatMessageEditDto entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
    }
