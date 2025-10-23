using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysScreenPermissionPropertyProfile : Profile
    {
        public SysScreenPermissionPropertyProfile()
        {
        
            CreateMap<SysScreenPermissionPropertyDto, SysScreenPermissionProperty>().ReverseMap();
        }
    }
}
