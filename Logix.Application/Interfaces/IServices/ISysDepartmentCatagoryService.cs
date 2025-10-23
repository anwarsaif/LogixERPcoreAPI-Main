using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysDepartmentCatagoryService : IGenericQueryService<SysDepartmentCatagoryDto, SysDepartmentCatagory>, IGenericWriteService<SysDepartmentCatagoryDto, SysDepartmentCatagoryDto>
    {
    }
}
