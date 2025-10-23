using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysSupplierService : IGenericQueryService<SysCustomerDto, SysCustomerVw>, IGenericWriteService<SysCustomerAddVM, SysCustomerEditDto>
    {
        
    }
}
