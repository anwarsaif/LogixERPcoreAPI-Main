using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysDepartmentCatagoryProfile : Profile
    {
        public SysDepartmentCatagoryProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysDepartmentCatagoryDto, SysDepartmentCatagory>().ReverseMap();
        }
    }
}
