using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysDynamicValueProfile : Profile
    {
        public SysDynamicValueProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysDynamicValueDto, SysDynamicValue>().ReverseMap();

            //Mapping EditDto To Entity
            CreateMap<SysDynamicValueEditDto, SysDynamicValue>().ReverseMap();
        }
    }
    
}
