using Logix.Application.DTOs.Main;

namespace Logix.MVC.Reports.Main.ViewModels
{
	public class UserTrackingDS
	{
		public ReportBasicDataDto? BasicData { get; set; }
		public SysUserTrackingFilterDto? Filter { get; set; }
		public List<SysUserTrackingVm>? Details { get; set; }
	}
}
