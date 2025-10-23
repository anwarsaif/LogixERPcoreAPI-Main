using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysPropertyProfile : Profile
    {
        public SysPropertyProfile()
        {
        
            CreateMap<SysPropertyDto, SysProperty>().ReverseMap();
            CreateMap<SysPropertyEditDto, SysProperty>().ReverseMap();
        }
    }
}
