using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysZatcaSignedXmlProfile : Profile
    {
        public SysZatcaSignedXmlProfile()
        {
            CreateMap<SysZatcaSignedXmlDto, SysZatcaSignedXml>().ReverseMap();
        }
    }
}
