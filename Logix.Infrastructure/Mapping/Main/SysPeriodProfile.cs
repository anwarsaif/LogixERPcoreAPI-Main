using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
	public class SysPeriodProfile : Profile
    {
        public SysPeriodProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysPeriodDto, SysPeriod>().ReverseMap();

            //Mapping EditDto To Entity
            CreateMap<SysPeriodEditDto, SysPeriod>().ReverseMap();
        }
    }
    
}
