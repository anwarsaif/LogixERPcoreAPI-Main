using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.MVC.Reports.Main.ViewModels
{
	public class PermitionDS
	{
		// this class will be the Data Source(DS) of the report.
		public ReportBasicDataDto? BasicData { get; set; } // spelling must be 'BasicData'.
		public SysUserFilterDto? Filter { get; set; } 
		public List<SysUserFilterDto>? Details { get; set; } 
	}
}
