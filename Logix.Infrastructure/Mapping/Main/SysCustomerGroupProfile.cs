using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysCustomerGroupProfile : Profile
    {
        public SysCustomerGroupProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysCustomerGroupDto, SysCustomerGroup>().ReverseMap();
            CreateMap<SysCustomerGroupEditDto, SysCustomerGroup>().ReverseMap();
        }
    }
}
