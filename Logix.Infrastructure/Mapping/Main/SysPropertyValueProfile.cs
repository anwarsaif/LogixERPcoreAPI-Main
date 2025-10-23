using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysPropertyValueProfile : Profile
    {
        public SysPropertyValueProfile()
        {
       
            CreateMap<SysPropertyValueDto, SysPropertyValue>().ReverseMap();
        }
    }
}
