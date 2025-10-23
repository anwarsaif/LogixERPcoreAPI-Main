using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysFilesDocumentService : IGenericQueryService<SysFilesDocumentDto, SysFilesDocumentVw>, IGenericWriteService<SysFilesDocumentDto, SysFilesDocumentDto>
    {

    }
}