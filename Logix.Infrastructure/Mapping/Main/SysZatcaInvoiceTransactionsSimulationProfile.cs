using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysZatcaInvoiceTransactionsSimulationProfile : Profile
    {
        public SysZatcaInvoiceTransactionsSimulationProfile()
        {
            CreateMap<SysZatcaInvoiceTransactionsSimulationDto, SysZatcaInvoiceTransactionsSimulation>().ReverseMap();
        }
    }
}
