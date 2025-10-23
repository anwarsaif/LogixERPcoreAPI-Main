using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysScreenInstalledProfile : Profile
    {
        public SysScreenInstalledProfile()
        {
            //Mapping Dto To Entity
            CreateMap<SysScreenInstalledDto, SysScreenInstalled>().ReverseMap();
        }
    }
}
