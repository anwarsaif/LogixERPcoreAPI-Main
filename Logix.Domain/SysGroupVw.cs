using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Keyless]
    public partial class SysGroupVw
    {
        [Column("GroupID")]
        public int GroupId { get; set; }
        [StringLength(500)]
        public string? GroupName { get; set; }
        [Column("System_Id")]
        public int? SystemId { get; set; }
        //[Column("ISDEL")]
        //public bool? Isdel { get; set; }
        [Column("System_Name")]
        [StringLength(50)]
        public string? SystemName { get; set; }
        [Column("Facility_ID")]
        public long? FacilityId { get; set; }
        [Column("Dashboard_Widget")]
        [StringLength(2555)]
        public string? DashboardWidget { get; set; }
        [Column("APP_Status_To")]
        [StringLength(2555)]
        public string? AppStatusTo { get; set; }
        [Column("APP_Status_From")]
        [StringLength(2555)]
        public string? AppStatusFrom { get; set; }
        [Column("USER_ID")]
        public long? UserId { get; set; }

        public bool? IsDel { get; set; }


        [Column("System_Name2")]
        [StringLength(50)]
        public string? SystemName2 { get; set; }
        [StringLength(500)]
        public string? GroupName2 { get; set; }
    }
}
