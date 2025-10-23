using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysCustomerContactProfile : Profile
    {
        public SysCustomerContactProfile()
        {
            CreateMap<SysCustomerContactDto, SysCustomerContact>().ReverseMap();
        }
    }
}
