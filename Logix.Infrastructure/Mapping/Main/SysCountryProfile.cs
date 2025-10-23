using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysCountryProfile : Profile
    {
        public SysCountryProfile()
        {
            CreateMap<SysCountryDto, SysCountry>().ReverseMap();
            CreateMap<SysCountryEditDto, SysCountry>().ReverseMap();
        }
    }
}
