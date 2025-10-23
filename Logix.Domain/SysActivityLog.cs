using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Logix.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace Logix.Domain.Main
{
    [Table("Sys_ActivityLog")]
    public partial class SysActivityLog 
    {
        [Key]
        [Column("ActivityLogID")]
        public long ActivityLogId { get; set; }
        [StringLength(50)]
        public string? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ActivityDate { get; set; }
        [Column("TableID")]
        public int? TableId { get; set; }
        [Column("Table_Primarykey")]
        public long? TablePrimarykey { get; set; }
        [Column("ActivityTypeID")]
        public int? ActivityTypeId { get; set; }
        [Column("Screen_ID")]
        public long? ScreenId { get; set; }
    }
}
