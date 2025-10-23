using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysVersionProfile : Profile
    {
        public SysVersionProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysVersionDto, SysVersion>().ReverseMap();

            //Mapping EditDto To Entity
            CreateMap<SysVersionEditDto, SysVersion>().ReverseMap();
            CreateMap<SysVersionEditDto, SysVersionDto>().ReverseMap();
        }
    }
    
}
