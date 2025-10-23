using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysCustomerGroupAccountProfile : Profile
    {
        public SysCustomerGroupAccountProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysCustomerGroupAccountDto, SysCustomerGroupAccount>().ReverseMap();
        }
    }
}
