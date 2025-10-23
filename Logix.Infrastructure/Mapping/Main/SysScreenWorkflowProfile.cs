using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysScreenWorkflowProfile : Profile
    {
        public SysScreenWorkflowProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysScreenWorkflowDto, SysScreenWorkflow>().ReverseMap();

        }
    }
}
