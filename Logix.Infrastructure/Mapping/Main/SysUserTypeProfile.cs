using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysUserTypeProfile : Profile
    {
        public SysUserTypeProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysUserTypeDto, SysUserType>().ReverseMap();
        }
    }
}
