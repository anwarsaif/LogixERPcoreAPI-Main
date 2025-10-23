using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysVatGroupProfile : Profile
    {
        public SysVatGroupProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysVatGroupDto, SysVatGroup>().ReverseMap();

            //Mapping EditDto To Entity
            CreateMap<SysVatGroupEditDto, SysVatGroup>().ReverseMap();
        }
    }
    
}
