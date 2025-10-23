using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysNotificationsMangProfile : Profile
    {
        public SysNotificationsMangProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysNotificationsMangDto, SysNotificationsMang>().ReverseMap();

            //Mapping EditDto To Entity
            CreateMap<SysNotificationsMangEditDto, SysNotificationsMang>().ReverseMap();
        }
    }
}
