using Logix.Application.DTOs.Main;
using Logix.Application.Wrapper;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysScreenPermissionPropertyService : IGenericQueryService<SysScreenPermissionPropertyDto, SysScreenPermissionProperty>, IGenericWriteService<SysScreenPermissionPropertyDto, SysPropertyVM>
    {
        Task<IResult<SysScreenPermissionPropertyDto>> GetByProperty(long propertyId, long userId);
        Task<IResult<SysScreenPermissionPropertiesVw>> GetByPropertyVw(long propertyId, long userId);
        //Task<IResult<IEnumerable<SysScreenPermissionPropertiesVw>>> GetAllVw();
    }
}
