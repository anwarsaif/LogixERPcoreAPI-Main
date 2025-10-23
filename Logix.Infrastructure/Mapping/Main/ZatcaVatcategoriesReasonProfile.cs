using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class ZatcaVatcategoriesReasonProfile : Profile
    {
        public ZatcaVatcategoriesReasonProfile()
        {
            CreateMap<ZatcaVatcategoriesReasonDto, ZatcaVatcategoriesReason>().ReverseMap();
        }
    }
}
