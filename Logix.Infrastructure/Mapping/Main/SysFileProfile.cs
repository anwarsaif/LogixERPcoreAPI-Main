using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysFileProfile : Profile
    {
        public SysFileProfile()
        {
            CreateMap<SysFileDto, SysFile>().ReverseMap();
        }
    }
}
