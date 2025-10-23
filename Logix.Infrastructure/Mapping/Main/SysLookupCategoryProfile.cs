using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysLookupCategoryProfile : Profile
    {
        public SysLookupCategoryProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysLookupCategoryDto, SysLookupCategory>().ReverseMap();

            CreateMap<SysLookupCategoryEditDto, SysLookupCategory>().ReverseMap();
        }
    }
}
