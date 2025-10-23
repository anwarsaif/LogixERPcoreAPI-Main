using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class ChatMessageProfile : Profile
    {
        public ChatMessageProfile()
        {
            CreateMap<ChatMessageDto, ChatMessage>().ReverseMap();
            CreateMap<ChatMessageEditDto, ChatMessage>().ReverseMap();
        }
    }
}
