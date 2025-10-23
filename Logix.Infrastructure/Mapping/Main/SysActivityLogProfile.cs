using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysActivityLogProfile : Profile
    {
        public SysActivityLogProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysActivityLogDto, SysActivityLog>().ReverseMap();
        }
    }
}
