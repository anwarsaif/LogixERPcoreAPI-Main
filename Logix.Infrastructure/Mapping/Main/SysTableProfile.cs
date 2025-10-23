using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysTableProfile : Profile
    {
        public SysTableProfile()
        {
            CreateMap<SysTableDto, SysTable>().ReverseMap();
        }
    }
}
