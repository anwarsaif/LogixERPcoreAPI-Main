using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysNotificationsSettingProfile : Profile
    {
        public SysNotificationsSettingProfile()
        {
            CreateMap<SysNotificationsSettingDto, SysNotificationsSetting>().ReverseMap();

            CreateMap<SysNotificationsSettingEditDto, SysNotificationsSetting>().ReverseMap();
        }
    }
}
