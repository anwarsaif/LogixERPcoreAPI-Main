using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysLookupDataProfile : Profile
    {
        public SysLookupDataProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysLookupDataDto, SysLookupData>().ReverseMap();
        }
    }
}
