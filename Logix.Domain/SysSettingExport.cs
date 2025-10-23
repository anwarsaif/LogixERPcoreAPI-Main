using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_Setting_Export")]
    public partial class SysSettingExport : TraceEntity
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("Facility_ID")]
        public int? FacilityId { get; set; }
        [Column("System_ID")]
        public long? SystemId { get; set; }
        [Column("Screen_ID")]
        public long? ScreenId { get; set; }
        [StringLength(250)]
        public string? Name { get; set; }
        [StringLength(250)]
        public string? Name2 { get; set; }
        public int? Type { get; set; }
        public string? Query { get; set; }
       
    }
}
