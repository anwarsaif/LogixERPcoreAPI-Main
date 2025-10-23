using Logix.Application.DTOs.Main;
using Logix.Application.DTOs.PUR;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysCustomerService : IGenericQueryService<SysCustomerDto, SysCustomerVw>, IGenericWriteService<SysCustomerAddVM, SysCustomerEditDto>
    {
        Task<IResult<SysCustomerAddQVDto>> AddQualifiedVendor(SysCustomerAddQVDto entity, CancellationToken cancellationToken = default);
        Task<IResult<SysCustomerEditQVDto>> UpdateQualifiedVendor(SysCustomerEditQVDto entity, bool approve, CancellationToken cancellationToken = default);
        Task<IResult> RemoveQualifiedVendor(long Id, CancellationToken cancellationToken = default);
        Task<IResult<SysCustomerAddPVDto>> AddPortalVendor(SysCustomerAddPVDto entity, CancellationToken cancellationToken = default);
        //Task<IResult<SysCustomerEditQVDto>> ApproveVendor(SysCustomerEditQVDto customerDto, CancellationToken cancellationToken = default);
    }
}
