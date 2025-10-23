using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysZatcaReportingResultsSimulationProfile : Profile
    {
        public SysZatcaReportingResultsSimulationProfile()
        {
            CreateMap<SysZatcaReportingResultsSimulationDto, SysZatcaReportingResultsSimulation>().ReverseMap();
        }
    }
}
