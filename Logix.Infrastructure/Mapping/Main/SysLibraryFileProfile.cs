using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public class SysLibraryFileProfile : Profile
    {
        public SysLibraryFileProfile()
        {
            CreateMap<SysLibraryFileDto, SysLibraryFile>().ReverseMap();
            CreateMap<SysLibraryFileEditDto, SysLibraryFile>().ReverseMap();
        }
    }
}
