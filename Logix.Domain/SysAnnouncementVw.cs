using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysAnnouncementVw
    {
        public bool IsDeleted { get; set; }
        [Column("USER_FULLNAME")]
        [StringLength(50)]
        public string? UserFullname { get; set; }
        [Column("ID")]
        public long Id { get; set; }
        public int? Type { get; set; }

        [Display(Name ="الموضوع")]
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
        [Column("Type_Name")]
        [StringLength(250)]
        public string? TypeName { get; set; }
        [Column("Location_ID")]
        public int? LocationId { get; set; }
        public bool? IsActive { get; set; }
        [Column("Location_Name")]
        [StringLength(250)]
        public string? LocationName { get; set; }
        public int? Language { get; set; }
        [Column("Start_Date")]
        [StringLength(10)]
        public string? StartDate { get; set; }
        [Column("End_Date")]
        [StringLength(10)]
        public string? EndDate { get; set; }
        [Column("Branch_ID")]
        public long? BranchId { get; set; }
        [Column("Dept_Location_ID")]
        public int? DeptLocationId { get; set; }
        [Column("BRA_NAME")]
        public string? BraName { get; set; }
        [Column("Dept_Location_Name")]
        [StringLength(200)]
        public string? DeptLocationName { get; set; }
        [Column("Facility_ID")]
        public long? FacilityId { get; set; }
        [Column("Dept_ID")]
        public int? DeptId { get; set; }
        [Column("Department_Name")]
        [StringLength(200)]
        public string? DepartmentName { get; set; }
    }
}
