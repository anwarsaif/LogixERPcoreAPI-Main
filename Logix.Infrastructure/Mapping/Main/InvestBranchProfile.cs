using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Application.DTOs.TS;
using Logix.Domain.Main;
using Logix.Domain.TS;

namespace Logix.Infrastructure.Mapping.Main
{
    public class InvestBranchProfile : Profile
    {
        public InvestBranchProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<InvestBranchDto, InvestBranch>().ReverseMap();
        }
    }
    public class SysTasksVwProfile : Profile
    {
        public SysTasksVwProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<TsTasksVwDto, TsTasksVw>().ReverseMap();
        }
    }
}
