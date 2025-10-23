using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysUserLogTimeProfile : Profile
    {
        public SysUserLogTimeProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysUserLogTimeDto, SysUserLogTime>().ReverseMap();
        }
    }
}
