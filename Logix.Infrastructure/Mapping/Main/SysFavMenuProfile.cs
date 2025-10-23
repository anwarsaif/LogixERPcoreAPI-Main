using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysFavMenuProfile : Profile
    {
        public SysFavMenuProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysFavMenuDto, SysFavMenu>().ReverseMap();
        }
    }
}