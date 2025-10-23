using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public partial class SysCustomerProfile : Profile
    {
        public SysCustomerProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysCustomerDto, SysCustomer>().ReverseMap();

            //Mapping EditDto To Entity
            CreateMap<SysCustomerEditDto, SysCustomer>().ReverseMap();
            //Mapping EditDto To Entity
            CreateMap<SysCustomerAddQVDto, SysCustomer>().ReverseMap();
            //Mapping EditDto To Entity
            CreateMap<SysCustomerEditQVDto, SysCustomer>().ReverseMap();
        }
    }
}