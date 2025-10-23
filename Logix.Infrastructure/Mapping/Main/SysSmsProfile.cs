using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysSmsProfile : Profile
    {
        public SysSmsProfile()
        {
            CreateMap<SysSmsDto, SysSms>().ReverseMap();
        }
    }
}
