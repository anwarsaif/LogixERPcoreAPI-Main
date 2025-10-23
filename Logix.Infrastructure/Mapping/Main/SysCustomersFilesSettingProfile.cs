using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public partial class SysCustomersFilesSettingProfile : Profile
    {
        public SysCustomersFilesSettingProfile()
        {
            CreateMap<SysCustomersFilesSettingDto, SysCustomersFilesSetting>().ReverseMap();
        }
    }
}