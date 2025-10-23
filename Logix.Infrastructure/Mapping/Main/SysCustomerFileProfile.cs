using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysCustomerFileProfile : Profile
    {
        public SysCustomerFileProfile()
        {
            CreateMap<SysCustomerFileDto, SysCustomerFile>().ReverseMap();
        }
    }
}