using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysTemplateProfile : Profile
    {
        public SysTemplateProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysTemplateDto, SysTemplate>().ReverseMap();

            //Mapping EditDto To Entity
            CreateMap<SysTemplateEditDto, SysTemplate>().ReverseMap();
        }
    }
}