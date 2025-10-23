using Logix.Application.DTOs.Main;

namespace Logix.MVC.ViewModels
{
    public class SysCustomerTypeGroupVM
    {
        public int TypeId { get; set; }
        public string? Name { get; set; }
        public IEnumerable<SysCustomerGroupDto>? sysCustomerGroups { get; set; }
    }
}
