using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysScreenPermissionProfile : Profile
    {
        public SysScreenPermissionProfile()
        {
            //Mapping Dto To Entity
            CreateMap<SysScreenPermissionDto, SysScreenPermission>().ReverseMap();
            CreateMap<SysScreenPermissionDtoVM, SysScreenPermission>().ReverseMap();
        }
    }
}
