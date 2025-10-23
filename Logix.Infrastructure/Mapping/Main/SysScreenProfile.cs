using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysScreenProfile : Profile
    {
        public SysScreenProfile()
        {
            //Mapping Dto To Entity
            CreateMap<SysScreenDto, SysScreen>().ReverseMap();
        }
    }
}
