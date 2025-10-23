using Logix.Application.DTOs.Main;

namespace Logix.MVC.ViewModels
{
    public class ScreenVM
    {
        public ScreenVM()
        {
            Children = new List<SysScreenDto>();
        }
        public SysScreenDto ParentScreen { get; set; }
        public List<SysScreenDto> Children { get; set; }
    }

    public class DashboardScreensVM
    {
        public DashboardScreensVM()
        {
            // Children = new List<SysScreenDto>();
            Children = new List<SubListDto>();
        }
        // public SysScreenDto ParentScreen { get; set; }
        public MainListDto ParentScreen { get; set; }
        public List<SubListDto> Children { get; set; }
    }
}
