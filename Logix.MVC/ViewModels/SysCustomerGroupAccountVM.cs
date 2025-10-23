using Logix.Application.DTOs.Main;
using Logix.Domain.Main;
using System.ComponentModel.DataAnnotations;

namespace Logix.MVC.ViewModels
{
    public class SysCustomerGroupAccountVM
    {
        public SysCustomerGroupAccountVM()
        {
            sysCustomerGroupAccountDto = new SysCustomerGroupAccountDto();
            sysCustomerGroupAccountsVws = new HashSet<SysCustomerGroupAccountsVw>();            
        }
        public SysCustomerGroupAccountDto? sysCustomerGroupAccountDto { get; set; }
        public IEnumerable<SysCustomerGroupAccountsVw>? sysCustomerGroupAccountsVws { get; set; }
               
        public string AccName { get; set; }
        public string? GroupName { get; set; }
    }
}
