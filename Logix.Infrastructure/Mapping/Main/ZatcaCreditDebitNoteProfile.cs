using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class ZatcaCreditDebitNoteProfile : Profile
    {
        public ZatcaCreditDebitNoteProfile()
        {
            CreateMap<ZatcaCreditDebitNoteDto, ZatcaCreditDebitNote>().ReverseMap();
        }
    }
}
