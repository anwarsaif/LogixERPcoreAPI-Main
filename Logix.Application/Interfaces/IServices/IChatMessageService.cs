using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface IChatMessageService : IGenericQueryService<ChatMessageDto, ChatMessage>, IGenericWriteService<ChatMessageDto, ChatMessageEditDto>
    {
    }
}
