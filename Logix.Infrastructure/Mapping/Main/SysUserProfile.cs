using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysUserProfile : Profile
    {
        public SysUserProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysUserDto, SysUser>().ReverseMap();
            CreateMap<SysUserEditDto, SysUser>().ReverseMap();
        }
    }
}