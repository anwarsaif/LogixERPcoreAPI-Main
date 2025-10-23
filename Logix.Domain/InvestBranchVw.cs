using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class InvestBranchVw
    {
        [Column("BRANCH_ID")]
        public int BranchId { get; set; }
        [Column("BRA_NAME")]
        public string? BraName { get; set; }
        [Column("BRA_NAME2")]
        public string? BraName2 { get; set; }
        [Column("TELEPHONE")]
        [StringLength(50)]
        public string? Telephone { get; set; }
        [Column("MOBILE")]
        [StringLength(50)]
        public string? Mobile { get; set; }
        [Column("EMAIL")]
        [StringLength(50)]
        public string? Email { get; set; }
        [Column("ADDRESS")]
        public string? Address { get; set; }
        [Column("USER_ID")]
        public long? UserId { get; set; }
        [Column("ISDEL")]
        public bool? Isdel { get; set; }
        [Column("CC_ID")]
        public int? CcId { get; set; }
        [Column("Branch_Code")]
        [StringLength(50)]
        public string? BranchCode { get; set; }
        [StringLength(150)]
        public string? MapLat { get; set; }
        [StringLength(150)]
        public string? MapLng { get; set; }
        public bool? IsActive { get; set; }
        [Column("Facility_ID")]
        public long? FacilityId { get; set; }
        [Column("CostCenter_Code")]
        [StringLength(50)]
        public string? CostCenterCode { get; set; }
        [Column("CostCenter_Name")]
        [StringLength(150)]
        public string? CostCenterName { get; set; }
        [Column("CostCenter_Name2")]
        [StringLength(150)]
        public string? CostCenterName2 { get; set; }
        [StringLength(250)]
        public string? WebSite { get; set; }
        public long? BranchTypeId { get; set; }
        [Column("CategoryID")]
        public long? CategoryId { get; set; }
    }
}
