using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysScreenPropertyProfile : Profile
    {
        public SysScreenPropertyProfile()
        {
        
            CreateMap<SysScreenPropertyDto, SysScreenProperty>().ReverseMap();
        }
    }
}
