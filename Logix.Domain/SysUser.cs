using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("SYS_USER")]
    public partial class SysUser
    {
        [Key]
        [Column("USER_ID")]
        public long Id { get; set; }
        [Column("USER_FULLNAME")]
        [StringLength(50)]
        public string? UserFullname { get; set; }
        [Column("USER_EMAIL")]
        [StringLength(50)]
        public string? Email { get; set; }
        [Column("USER_NAME")]
        [StringLength(50)]
        public string? UserName { get; set; }
        [Column("USER_PASSWORD")]
        public byte[]? UserPassword { get; set; }
        [Column("USER_TYPE_ID")]
        public int? UserTypeId { get; set; }
        [Column("USER_PK_ID")]
        public long? UserPkId { get; set; }
        [Column("ISDEL")]
        public bool? Isdel { get; set; }
        [Column("BRANCHS_ID")]
        [StringLength(2500)]
        public string? BranchsId { get; set; }
        [Column("Emp_ID")]
        public int? EmpId { get; set; }
        public int? Enable { get; set; }
        [Column("Groups_ID")]
        [StringLength(2500)]
        public string? GroupsId { get; set; }
        [Column("Dashboard_Widget")]
        [StringLength(2555)]
        public string? DashboardWidget { get; set; }
        public bool? Isupdate { get; set; }
        [Column("Facility_ID")]
        public long? FacilityId { get; set; }
        public string? Signature { get; set; }
        [Column("IPS")]
        public string? Ips { get; set; }
        [Column("Time_From")]
        public TimeSpan? TimeFrom { get; set; }
        [Column("Time_To")]
        public TimeSpan? TimeTo { get; set; }
        [Column("ACC_Transfer")]
        public string? AccTransfer { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        [Column("Wh_Transaction_Type")]
        public string? WhTransactionType { get; set; }
        [Column("Sales_Type")]
        public int? SalesType { get; set; }
        [Column("Cus_ID")]
        public long? CusId { get; set; }
        [Column("ConnectionID")]
        [StringLength(100)]
        public string? ConnectionId { get; set; }
        [Column("Sup_ID")]
        public long? SupId { get; set; }
        [Column("User_Photo")]
        [StringLength(500)]
        public string? UserPhoto { get; set; }
        [Column("Projects_ID")]
        public string? ProjectsId { get; set; }
        public bool? IsAgree { get; set; }
        [Column("Two_factor")]
        public bool? TwoFactor { get; set; }
        [Column("Two_factor_Type")]
        public int? TwoFactorType { get; set; }
        [Column("OTP")]
        [StringLength(50)]
        public string? Otp { get; set; }
        [Column("OTP_Expiry", TypeName = "datetime")]
        public DateTime? OtpExpiry { get; set; }
        [Column("Cand_ID")]
        public long? CandId { get; set; }
        [Column("last_login", TypeName = "datetime")]
        public DateTime? LastLogin { get; set; }
        [Column("SSO_Token")]
        [StringLength(50)]
        public string? SsoToken { get; set; }
        [Column("SSO_Token_Expiry_Date", TypeName = "datetime")]
        public DateTime? SsoTokenExpiryDate { get; set; }
        [Column("User_Type2_ID")]
        public int? UserType2Id { get; set; }
        public string? PermissionsOverUserId { get; set; }
        public string? PermissionsOverCustomerGroupsId { get; set; }
        [Column("PermissionsOverAccAccountID")]
        public string? PermissionsOverAccAccountId { get; set; }
        [Column("PermissionsOverCCID")]
        public string? PermissionsOverCcid { get; set; }
        [Column("Azure_Token_Expiry_Date", TypeName = "datetime")]
        public DateTime? AzureTokenExpiryDate { get; set; }
        [Column("Azure_Token")]
        public string? AzureToken { get; set; }
    }
}
