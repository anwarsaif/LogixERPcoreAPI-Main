using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysMailServerProfile : Profile
    {
        public SysMailServerProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysMailServerDto, SysMailServer>().ReverseMap();

           
        }
    }
}
