using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysAnnouncementProfile : Profile
    {
        public SysAnnouncementProfile()
        {
            //Mapping CreateDto To Entity
            CreateMap<SysAnnouncementDto, SysAnnouncement>().ReverseMap();

            //Mapping EditDto To Entity
            CreateMap<SysAnnouncementEditDto, SysAnnouncement>().ReverseMap();
        }
    }
}
