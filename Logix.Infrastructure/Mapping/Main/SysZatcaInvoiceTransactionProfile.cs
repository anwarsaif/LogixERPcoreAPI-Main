using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysZatcaInvoiceTransactionProfile : Profile
    {
        public SysZatcaInvoiceTransactionProfile()
        {
            CreateMap<SysZatcaInvoiceTransactionDto, SysZatcaInvoiceTransaction>().ReverseMap();
        }
    }
}
