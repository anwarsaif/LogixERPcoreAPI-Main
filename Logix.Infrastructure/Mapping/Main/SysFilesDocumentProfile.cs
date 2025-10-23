using AutoMapper;
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Infrastructure.Mapping.Main
{
    public partial class SysFilesDocumentProfile : Profile
    {
        public SysFilesDocumentProfile()
        {
            CreateMap<SysFilesDocumentDto, SysFilesDocument>().ReverseMap();
        }
    }
}
