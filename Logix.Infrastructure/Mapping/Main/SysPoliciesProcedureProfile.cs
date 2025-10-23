using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysPoliciesProcedureProfile : Profile
    {
        public SysPoliciesProcedureProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysPoliciesProcedureDto, SysPoliciesProcedure>().ReverseMap();

            //Mapping EditDto To Entity
            CreateMap<SysPoliciesProcedureEditDto, SysPoliciesProcedure>().ReverseMap();
        }
    }
}
