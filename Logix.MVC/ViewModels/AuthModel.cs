namespace Logix.MVC.ViewModels
{
    public class AuthModel
    {
        public long UserId { get; set; }
        public string? Username { get; set; }
        public string? UserFullName { get; set; }
        public string? UserFullName2 { get; set; }
        public string? UserEmail { get; set; }
        public int EmpId { get; set; }
        public string? EmpCode { get; set; }
        public long FacilityId { get; set; }
        public long FinYear { get; set; }
        public int GroupId { get; set; }
        public int Language { get; set; }
        public string? CalendarType { get; set; }
        public long PeriodId { get; set; }
        public string? OldBaseUrl { get; set; }
        public string? CoreBaseUrl { get; set; }
        public string? CompAddress { get; set; }
        public string? CompLogo { get; set; }
        public string? CompLogoPrint { get; set; }
        public string? CompPhone { get; set; }
        public string? CompName { get; set; }
        public string? CompName2 { get; set; }
        public string? CompImgFooter { get; set; }
        public string? CompVatNumber { get; set; }
        public string? Token { get; set; }
        public int FinyearGregorian { get; set; }

        public string? LastLogin { get; set; }
        public int DeptId { get; set; }
        public int Location { get; set; }
        public int? SalesType { get; set; }
        public bool isTwoFactorActive { get; set; }
        public bool? isAgree{ get; set; }

    }




}