namespace Logix.MVC.ViewModels
{
    public class SysGroupVM
    {
        //using this VM for Grant Permissions with checkBoxes
        public int GroupId { get; set; }
        public string? GroupName { get; set; }
        public string? GroupName2 { get; set; }
        public bool IsSelected { get; set; }
    }
}
