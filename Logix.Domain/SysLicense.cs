using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_Licenses")]
    public partial class SysLicense
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("Facility_ID")]
        public long? FacilityId { get; set; }
        public int? JobCat { get; set; }
        [Column("License_Type")]
        public int? LicenseType { get; set; }
        [Column("Branch_ID")]
        public int? BranchId { get; set; }
        [Column("License_No")]
        [StringLength(50)]
        public string? LicenseNo { get; set; }
        [Column("License_Former_Place")]
        [StringLength(50)]
        public string? LicenseFormerPlace { get; set; }
        [Column("Issued_Date")]
        [StringLength(10)]
        public string? IssuedDate { get; set; }
        [Column("Expiry_Date")]
        [StringLength(10)]
        public string? ExpiryDate { get; set; }
        [Column("Renewal_Date")]
        [StringLength(10)]
        public string? RenewalDate { get; set; }
        [Column("File_URL")]
        [StringLength(250)]
        public string? FileUrl { get; set; }
        public string? Note { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
