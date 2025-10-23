using Logix.Application.DTOs.Main;
using Logix.Application.Services;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysLibraryFileService : IGenericQueryService<SysLibraryFileDto, SysLibraryFilesVw>, IGenericWriteService<SysLibraryFileDto, SysLibraryFileEditDto>
    {
    }
}
