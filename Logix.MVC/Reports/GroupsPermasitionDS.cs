using Logix.Application.DTOs.Main;

namespace Logix.MVC.Reports.Main.ViewModels
{
	public class GroupsPermasitionDS
	{
		public ReportBasicDataDto? BasicData { get; set; }
		public SysGroupFilterDto? Filter { get; set; }
		public List<SysGroupFilterDto>? Details { get; set; }
	}
}
