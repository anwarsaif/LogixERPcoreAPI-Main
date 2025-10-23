
using Logix.Application.DTOs.Main;
using Logix.Domain.Main;

namespace Logix.MVC.Reports.Main.ViewModels
{
	public class UsersPermissionsDS
	{
		// this class will be the Data Source(DS) of the report.
		public ReportBasicDataDto? BasicData { get; set; } // spelling must be 'BasicData'.
		public UserPermissionSearchVm? Filter { get; set; }
		public List<UserPermissionSearchVm>? Details { get; set; } // any list of data, i.e Vw
	}
}
