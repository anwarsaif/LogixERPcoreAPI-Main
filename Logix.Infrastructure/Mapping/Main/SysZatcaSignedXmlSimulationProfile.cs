using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysZatcaSignedXmlSimulationProfile : Profile
    {
        public SysZatcaSignedXmlSimulationProfile()
        {
            CreateMap<SysZatcaSignedXmlSimulationDto, SysZatcaSignedXmlSimulation>().ReverseMap();
        }
    }
}
