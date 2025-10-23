using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.Application.Interfaces.IServices.Main
{
    public interface ISysPoliciesProcedureService : IGenericQueryService<SysPoliciesProcedureDto, SysPoliciesProceduresVw>, IGenericWriteService<SysPoliciesProcedureDto, SysPoliciesProcedureEditDto>
    {

    }
}