using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysSystemProfile : Profile
    {
        public SysSystemProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysSystemDto, SysSystem>().ReverseMap();
        }
    }
}
