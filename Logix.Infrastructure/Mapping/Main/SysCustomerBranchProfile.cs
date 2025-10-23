using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysCustomerBranchProfile : Profile
    {
        public SysCustomerBranchProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysCustomerBranchDto, SysCustomerBranch>().ReverseMap();

            //Mapping EditDto To Entity
            CreateMap<SysCustomerBranchEditDto, SysCustomerBranch>().ReverseMap();
        }
    }
}
