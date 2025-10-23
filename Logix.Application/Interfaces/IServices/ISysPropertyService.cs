using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysPropertyService : IGenericQueryService<SysPropertyDto, SysPropertiesVw>, IGenericWriteService<SysPropertyDto, SysPropertyEditDto>
    {
        //Task<IResult<SysPropertiesVw>> GetByIdVw(long propertyId);
        //Task<IResult<IEnumerable<SysPropertiesVw>>> GetAllVw();
    }
}
