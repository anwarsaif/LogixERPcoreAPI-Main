using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysLicenseProfile : Profile
    {
        public SysLicenseProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysLicenseDto, SysLicense>().ReverseMap();
        }
    }
}
