using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysZatcaReportingResultProfile : Profile
    {
        public SysZatcaReportingResultProfile()
        {
            CreateMap<SysZatcaReportingResultDto, SysZatcaReportingResult>().ReverseMap();
        }
    }
}
