using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysCustomerTypeProfile : Profile
    {
        public SysCustomerTypeProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysCustomerTypeDto, SysCustomerType>().ReverseMap();
        }
    }
}
