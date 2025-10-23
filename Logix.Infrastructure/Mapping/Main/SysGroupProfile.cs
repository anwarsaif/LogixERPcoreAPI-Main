using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysGroupProfile : Profile
    {
        public SysGroupProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysGroupDto, SysGroup>().ReverseMap();
            CreateMap<SysGroupEditDto, SysGroup>().ReverseMap();
        }
    }
}
