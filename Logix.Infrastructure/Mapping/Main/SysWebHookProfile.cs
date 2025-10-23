using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysWebHookProfile : Profile
    {
        public SysWebHookProfile()
        {
            CreateMap<SysWebHookDto, SysWebHook>().ReverseMap();
            CreateMap<SysWebHookEditDto, SysWebHook>().ReverseMap();
        }
    }
}
