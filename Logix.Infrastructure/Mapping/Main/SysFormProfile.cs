using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysFormProfile : Profile
    {
        public SysFormProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysFormDto, SysForm>().ReverseMap();
            //Mapping EditDto To Entity
            CreateMap<SysFormEditDto, SysForm>().ReverseMap();
        }
    }
}
