using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysExchangeRateService:IGenericQueryService<SysExchangeRateDto,SysExchangeRateListVW>, IGenericWriteService<SysExchangeRateDto, SysExchangeRateEditDto>
    {
        Task<IResult<IEnumerable<SysExchangeRateVw>>> GetAllExRateVw(CancellationToken cancellationToken = default);
    }
}
