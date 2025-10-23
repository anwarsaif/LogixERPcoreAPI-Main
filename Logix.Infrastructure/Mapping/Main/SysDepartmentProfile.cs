using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysDepartmentProfile : Profile
    {
        public SysDepartmentProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysDepartmentDto, SysDepartment>().ReverseMap();
            CreateMap<SysDepartmentEditDto, SysDepartment>().ReverseMap();
        }
    }
}
