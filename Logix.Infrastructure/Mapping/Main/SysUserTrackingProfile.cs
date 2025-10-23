using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysUserTrackingProfile : Profile
    {
        public SysUserTrackingProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysUserTrackingDto, SysUserTracking>().ReverseMap();
        }
    }
}
