using Logix.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logix.Domain.Main
{
    [Table("Sys_Announcement")]
    public partial class SysAnnouncement
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        public int? Type { get; set; }
        public string? Subject { get; set; }
        public string? Detailes { get; set; }
        [Column("Attach_File")]
        public string? AttachFile { get; set; }
        [Column("Publish_Date")]
        [StringLength(10)]
        public string? PublishDate { get; set; }
        [Column("Users_ID")]
        [StringLength(250)]
        public string? UsersId { get; set; }
        [Column("Groups_ID")]
        [StringLength(250)]
        public string? GroupsId { get; set; }
        public long CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        [Column("Location_ID")]
        public int? LocationId { get; set; }
        public int? Language { get; set; }
        [Column("Start_Date")]
        [StringLength(10)]
        public string? StartDate { get; set; }
        [Column("End_Date")]
        [StringLength(10)]
        public string? EndDate { get; set; }
        [Column("Branch_ID")]
        public long? BranchId { get; set; }
        [Column("Dept_ID")]
        public int? DeptId { get; set; }
        [Column("Dept_Location_ID")]
        public int? DeptLocationId { get; set; }
        [Column("Facility_ID")]
        public long? FacilityId { get; set; }
    }
}
