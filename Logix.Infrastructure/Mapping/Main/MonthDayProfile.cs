using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class MonthDayProfile : Profile
    {
        public MonthDayProfile()
        {
            CreateMap<MonthDayDto, MonthDay>().ReverseMap();
        }
    }
}
