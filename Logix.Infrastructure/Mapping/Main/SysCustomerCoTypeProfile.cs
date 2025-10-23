using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysCustomerCoTypeProfile : Profile
    {
        public SysCustomerCoTypeProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysCustomerCoTypeDto, SysCustomerCoType>().ReverseMap();
        }
    }
}
