using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysSettingExportProfile : Profile
    {
        public SysSettingExportProfile()
        {
            CreateMap<SysSettingExportDto, SysSettingExport>().ReverseMap();
        }
    }
}