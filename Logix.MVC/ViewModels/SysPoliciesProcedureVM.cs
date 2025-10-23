using Logix.Application.DTOs.Main;

namespace Logix.MVC.ViewModels
{
    public class SysPoliciesProcedureVM
    {
        public SysPoliciesProcedureDto SysPoliciesProcedureDto { get; set; }
        public List<SysGroupVM> SysGroupsList { get; set; }

        public SysPoliciesProcedureVM()
        {
            this.SysPoliciesProcedureDto = new SysPoliciesProcedureDto();
            this.SysGroupsList = new List<SysGroupVM>();
        }
    }
    public class SysPoliciesProcedureEditVM
    {
        public SysPoliciesProcedureEditDto SysPoliciesProcedureEditDto { get; set; }
        public List<SysGroupVM> SysGroupsList { get; set; }

        public SysPoliciesProcedureEditVM()
        {
            this.SysPoliciesProcedureEditDto = new SysPoliciesProcedureEditDto();
            this.SysGroupsList = new List<SysGroupVM>();
        }
    }
}
