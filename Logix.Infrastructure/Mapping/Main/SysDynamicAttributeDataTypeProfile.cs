using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysDynamicAttributeDataTypeProfile : Profile
    {
        public SysDynamicAttributeDataTypeProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysDynamicAttributeDataTypeDto, SysDynamicAttributeDataType>().ReverseMap();
        }
    }
}
