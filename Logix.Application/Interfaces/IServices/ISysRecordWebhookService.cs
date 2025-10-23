using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysRecordWebhookService : IGenericQueryService<SysRecordWebhookDto, SysRecordWebhookVw>, IGenericWriteService<SysRecordWebhookDto, SysRecordWebhookEditDto>
    {
        Task<IResult<List<SysRecordWebhookDto>>> RemoveSelectedItems(string entity, CancellationToken cancellationToken = default);
    }
}
