using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysNotificationProfile : Profile
    {
        public SysNotificationProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysNotificationDto, SysNotification>().ReverseMap();
        }
    }
}
