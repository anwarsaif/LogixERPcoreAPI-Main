using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_Group")]
    public partial class SysGroup : TraceEntity
    {
        [Key]
        [Column("GroupID")]
        public int GroupId { get; set; }
        [StringLength(500)]
        public string? GroupName { get; set; }
        [Column("System_Id")]
        public int? SystemId { get; set; }
        //[Column("ISDEL")]
        //public bool? Isdel { get; set; }
        [Column("USER_ID")]
        public long? UserId { get; set; }
        [Column("APP_Status_From")]
        [StringLength(2555)]
        public string? AppStatusFrom { get; set; }
        [Column("APP_Status_To")]
        [StringLength(2555)]
        public string? AppStatusTo { get; set; }
        [Column("Dashboard_Widget")]
        [StringLength(2555)]
        public string? DashboardWidget { get; set; }
        [Column("Facility_ID")]
        public long? FacilityId { get; set; }
        [Column("Time_From")]
        public TimeSpan? TimeFrom { get; set; }
        [Column("Time_To")]
        public TimeSpan? TimeTo { get; set; }

        [StringLength(500)]
        public string? GroupName2 { get; set; }
    }
}
