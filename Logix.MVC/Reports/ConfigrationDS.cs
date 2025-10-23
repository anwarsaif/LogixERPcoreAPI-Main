using Logix.Application.DTOs.Main;

namespace Logix.MVC.Reports.Main.ViewModels
{
    public class ConfigrationDS
    {
        public ReportBasicDataDto? BasicData { get; set; } // spelling must be 'BasicData'.
        public SysPropertyValueFilterDto? Filter { get; set; }
        public List<SysPropertyValueFilterDto>? Details { get; set; }
    }
}
