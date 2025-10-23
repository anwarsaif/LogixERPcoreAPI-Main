using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysDynamicAttributeProfile : Profile
    {
        public SysDynamicAttributeProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysDynamicAttributeDto, SysDynamicAttribute>().ReverseMap();

            //Mapping EditDto To Entity
            CreateMap<SysDynamicAttributeEditDto, SysDynamicAttribute>().ReverseMap();
        }
    }
}
