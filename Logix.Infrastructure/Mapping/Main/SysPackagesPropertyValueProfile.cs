using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysPackagesPropertyValueProfile : Profile
    {
        public SysPackagesPropertyValueProfile()
        {
            CreateMap<SysPackagesPropertyValueDto, SysPackagesPropertyValue>().ReverseMap();
        }
    }

}
