using Logix.Application.DTOs.Main;

namespace Logix.MVC.Reports.Main.ViewModels
{
    public class UsersLoginDS
    {
        // this DS used for 2 reports (userLoginsRpt and usersPermissionsRpt)
        public ReportBasicDataDto? BasicData { get; set; }
        public SysUsersLoginsVm? Filter { get; set; }
        public List<SysUsersLoginsVm>? Details { get; set; }
    }
}
